using Lab31.App.Domain;

namespace Lab31.App.Metadata;

// Інтерфейс каталогу всіх доступних типів сутностей у системі
public interface IPersonTypeRegistry
{
    // Кількість зареєстрованих типів сутностей
    int Count { get; }

    // Індексатор для отримання дескриптора за його порядковим номером
    IPersonDescriptor this[int index] { get; }

    // Знаходить дескриптор за системною назвою типу (наприклад, "Student")
    IPersonDescriptor? FindByTypeName(string typeName);

    // Повертає дескриптор для конкретного об'єкта особи
    IPersonDescriptor GetFor(Person person);
}