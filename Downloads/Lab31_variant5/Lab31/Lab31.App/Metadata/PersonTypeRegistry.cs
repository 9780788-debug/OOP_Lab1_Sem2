using System;
using Lab31.App.Domain;

namespace Lab31.App.Metadata;

// Реалізація каталогу типів сутностей
public sealed class PersonTypeRegistry : IPersonTypeRegistry
{
    // Масив усіх зареєстрованих дескрипторів
    private readonly IPersonDescriptor[] _descriptors;

    // Конструктор: приймає список дескрипторів
    public PersonTypeRegistry(params IPersonDescriptor[] descriptors)
    {
        _descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));
    }

    // Повертає загальну кількість типів
    public int Count => _descriptors.Length;

    // Доступ до дескриптора за індексом
    public IPersonDescriptor this[int index] => _descriptors[index];

    // Пошук дескриптора за назвою типу у файлі
    public IPersonDescriptor? FindByTypeName(string typeName)
    {
        string name = (typeName ?? string.Empty).Trim();

        foreach (var descriptor in _descriptors)
        {
            if (string.Equals(descriptor.TypeName, name, StringComparison.OrdinalIgnoreCase))
                return descriptor;
        }

        return null; // Якщо тип не знайдено
    }

    // Пошук дескриптора, який відповідає переданому об'єкту person
    public IPersonDescriptor GetFor(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));

        foreach (var descriptor in _descriptors)
        {
            if (descriptor.Describes(person))
                return descriptor;
        }

        // Якщо для об'єкта немає відповідного дескриптора
        throw new InvalidOperationException($"No descriptor registered for type {person.GetType().Name}.");
    }
}