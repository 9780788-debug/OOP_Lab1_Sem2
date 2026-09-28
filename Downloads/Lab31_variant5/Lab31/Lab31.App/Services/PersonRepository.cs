using System;
using System.IO;
using Lab31.App.Domain;
using Lab31.App.Metadata;
using Lab31.Storage;

namespace Lab31.App.Services;

// Репозиторій для транслювання об'єктів Person у поатрибутні записи сховища IRecordStorage
public sealed class PersonRepository : IPersonRepository
{
    // Посилання на абстрактне файлове сховище
    private readonly IRecordStorage _storage;

    // Реєстр типів осіб для роботи з метаданими
    private readonly IPersonTypeRegistry _types;

    // Конструктор (приймає залежності сховища та реєстру типів)
    public PersonRepository(IRecordStorage storage, IPersonTypeRegistry types)
    {
        _storage = storage;
        _types = types;
    }

    // Додає новий об'єктPerson у кінець файлу сховища
    public void Add(Person person)
    {
        using IRecordWriter writer = _storage.OpenAppender();
        WritePerson(writer, person);
    }

    // Шукає та повертає масив осіб, що відповідають умові предикату match
    public Person[] Find(Predicate<Person> match)
    {
        Person[] result = Array.Empty<Person>();
        if (!_storage.Exists)
            return result;

        using IRecordReader reader = _storage.OpenReader();
        Person? person;
        // Зчитуємо файл пооб'єктно до кінця потоку
        while ((person = ReadPerson(reader)) != null)
        {
            if (match(person))
            {
                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = person;
            }
        }

        return result;
    }

    // Видаляє записи, що відповідають умові match, за допомогою безпечного атомарного перезапису
    public int Remove(Predicate<Person> match)
    {
        if (!_storage.Exists)
            return 0;

        int removed = 0;
        using (IRecordReader reader = _storage.OpenReader())
        using (IRecordWriter writer = _storage.BeginRewrite())
        {
            Person? person;
            while ((person = ReadPerson(reader)) != null)
            {
                // Якщо умова виконується — пропускаємо запис (видаляємо), інакше переписуємо у тимчасовий файл
                if (match(person))
                    removed++;
                else
                    WritePerson(writer, person);
            }
        }

        // Завершуємо перезапис, підміняючи оригінальний файл новим
        _storage.CommitRewrite();
        return removed;
    }

    // Поатрибутно записує сутність у файл за допомогою IRecordWriter
    private void WritePerson(IRecordWriter writer, Person person)
    {
        IPersonDescriptor descriptor = _types.GetFor(person);

        // Початок запису об'єкта (назва типу та згенероване ім'я об'єкта)
        writer.BeginRecord(descriptor.TypeName, (person.FirstName + person.LastName).Replace(" ", string.Empty));

        // Послідовний запис усіх атрибутів
        foreach (AttributeSpec spec in descriptor.Attributes)
            writer.WriteAttribute(spec.Name, spec.GetValue(person));

        writer.EndRecord();
    }

    // Поатрибутно зчитує один об'єкт з потоку IRecordReader та конструює екземпляр Person
    private Person? ReadPerson(IRecordReader reader)
    {
        // Зчитування заголовка об'єкта (тип та ім'я)
        if (!reader.ReadRecordHeader(out string typeName, out string objectName))
            return null;

        // Пошук дескриптора за прочитаною назвою типу
        IPersonDescriptor descriptor = _types.FindByTypeName(typeName)
            ?? throw new InvalidDataException($"Невідомий тип сутності '{typeName}' (запис '{objectName}').");

        AttributeSpec[] specs = descriptor.Attributes;
        string?[] raw = new string?[specs.Length];

        // Зчитування всіх пар "атрибут-значення"
        while (reader.ReadAttribute(out string name, out string value))
        {
            int index = IndexOf(specs, name);
            if (index < 0)
                throw new InvalidDataException($"Невідомий атрибут '{name}' у записі '{typeName} {objectName}'.");

            raw[index] = value;
        }

        // Перевірка та валідація отриманих даних
        string[] values = new string[specs.Length];
        for (int i = 0; i < specs.Length; i++)
        {
            string? error = specs[i].Validate(raw[i]);
            if (error != null)
                throw new InvalidDataException($"Запис '{typeName} {objectName}': {error}");

            values[i] = specs[i].Normalize(raw[i]);
        }

        // Створення типізованого об'єкта
        return descriptor.Create(values);
    }

    // Допоміжний метод для пошуку індексу атрибута за його назвою
    private static int IndexOf(AttributeSpec[] specs, string name)
    {
        for (int i = 0; i < specs.Length; i++)
        {
            if (string.Equals(specs[i].Name, name, StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1;
    }
}