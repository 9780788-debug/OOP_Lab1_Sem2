using System;
using Lab31.App.Domain;
using Lab31.App.Metadata;

namespace Lab31.App.Services;

// Сервіс бізнес-логіки для управління об'єктами осіб
public sealed class PersonService : IPersonService
{
    // Посилання на репозиторій для виконання операцій зі сховищем
    private readonly IPersonRepository _repository;

    // Конструктор сервісу (приймає репозиторій через Dependency Injection)
    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    // Додає нову особу на основі дескриптора та сирих текстових значень
    public Person Add(IPersonDescriptor descriptor, string[] rawValues)
    {
        // Отримуємо специфікації атрибутів для даного типу
        AttributeSpec[] specs = descriptor.Attributes;

        // Перевіряємо відповідність кількості переданих значень кількості атрибутів
        if (rawValues.Length != specs.Length)
            throw new ArgumentException("Кількість значень не відповідає кількості атрибутів.", nameof(rawValues));

        // Масив для збереження перевірених та нормалізованих значень
        string[] values = new string[specs.Length];
        for (int i = 0; i < specs.Length; i++)
        {
            // Валідація кожного атрибута
            string? error = specs[i].Validate(rawValues[i]);
            if (error != null)
                throw new ArgumentException(error, nameof(rawValues));

            // Нормалізація введеного значення (видалення зайвих пробілів тощо)
            values[i] = specs[i].Normalize(rawValues[i]);
        }

        // Створення екземпляра Person за допомогою дескриптора
        Person person = descriptor.Create(values);

        // Перевірка унікальності ідентифікатора особи перед додаванням
        if (FindById(person.UniqueId) != null)
            throw new InvalidOperationException($"Особа з ідентифікатором {person.UniqueId} вже існує.");

        // Збереження об'єкта в репозиторій
        _repository.Add(person);
        return person;
    }

    // Повертає масив усіх осіб зі сховища
    public Person[] GetAll() => _repository.Find(_ => true);

    // Шукає осіб за прізвищем (без урахування регістру)
    public Person[] FindByLastName(string lastName)
    {
        string wanted = lastName.Trim();
        return _repository.Find(p => string.Equals(p.LastName, wanted, StringComparison.OrdinalIgnoreCase));
    }

    // Шукає особу за її унікальним ідентифікатором
    public Person? FindById(string uniqueId)
    {
        string wanted = uniqueId.Trim();
        Person[] found = _repository.Find(p => string.Equals(p.UniqueId, wanted, StringComparison.OrdinalIgnoreCase));
        return found.Length > 0 ? found[0] : null;
    }

    // Видаляє особу за її унікальним ідентифікатором
    public int DeleteById(string uniqueId)
    {
        string wanted = uniqueId.Trim();
        return _repository.Remove(p => string.Equals(p.UniqueId, wanted, StringComparison.OrdinalIgnoreCase));
    }

    // Виконання Варіанта 5: шукає студентів вказаного курсу з вибраної країни
    public Student[] FindStudents(int course, string country)
    {
        // Фільтрація записів за допомогою предиката
        Person[] found = _repository.Find(p =>
            p is Student student
            && student.Course == course
            && CountryNames.IsSame(student.Country, country));

        // Приведення знайдених об'єктів до типу Student[]
        Student[] students = new Student[found.Length];
        for (int i = 0; i < found.Length; i++)
            students[i] = (Student)found[i];

        return students;
    }
}