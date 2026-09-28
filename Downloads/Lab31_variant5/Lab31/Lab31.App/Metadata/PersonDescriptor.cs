using System;
using Lab31.App.Domain;

namespace Lab31.App.Metadata;

// Дженерик-клас T, де T обов'язково є дочірнім від Person
public abstract class PersonDescriptor<T> : IPersonDescriptor where T : Person
{
    // Абстрактна назва типу (реалізується у StudentDescriptor)
    public abstract string TypeName { get; }

    // Абстрактний масив атрибутів (реалізується у StudentDescriptor)
    public abstract AttributeSpec[] Attributes { get; }

    // Перевіряє, чи збігається точний тип об'єкта 'person' з типом T (наприклад, чи є person точно Student)
    public bool Describes(Person person) => person.GetType() == typeof(T);

    // Точка входу створення: перевіряє кількість переданих значень та викликає CreateTyped
    public Person Create(string[] values)
    {
        // Якщо кількість значень з файлу не збігається з кількістю полів у дескрипторі — кидаємо помилку
        if (values.Length != Attributes.Length)
            throw new ArgumentException($"{TypeName} expects {Attributes.Length} values but got {values.Length}.", nameof(values));

        // Викликаємо строго типізований метод створення
        return CreateTyped(values);
    }

    // Абстрактний метод створення конкретного T (реалізує кожен дочірній дескриптор)
    protected abstract T CreateTyped(string[] values);

    // Допоміжний метод-фабрика для побудови AttributeSpec без сирого кастингу типом (T замість Person)
    protected static AttributeSpec Attr(
        string name,
        string label,
        string pattern,
        string hint,
        bool required,
        string defaultValue,
        Func<T, string> getter) =>
        new AttributeSpec(name, label, pattern, hint, required, defaultValue, person => getter((T)person));

    // Готове спільне поле "Ім'я" для всіх сутностей
    protected static AttributeSpec FirstNameAttribute() =>
        Attr("firstname", "Ім'я", Patterns.Name, Patterns.NameHint, true, string.Empty, p => p.FirstName);

    // Готове спільне поле "Прізвище" для всіх сутностей
    protected static AttributeSpec LastNameAttribute() =>
        Attr("lastname", "Прізвище", Patterns.Name, Patterns.NameHint, true, string.Empty, p => p.LastName);
}