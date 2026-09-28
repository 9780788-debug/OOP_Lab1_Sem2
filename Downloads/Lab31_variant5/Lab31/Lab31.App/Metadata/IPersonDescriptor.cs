using Lab31.App.Domain;

namespace Lab31.App.Metadata;

public interface IPersonDescriptor
{
    // Назва типу у текстовому файлі (наприклад, "Student")
    string TypeName { get; }

    // Масив атрибутів (полів) цієї сутності
    AttributeSpec[] Attributes { get; }

    // Перевіряє, чи належить передана особа цьому дескриптору
    bool Describes(Person person);

    // Створює об'єкт сутності із масиву текстових значень
    Person Create(string[] values);
}