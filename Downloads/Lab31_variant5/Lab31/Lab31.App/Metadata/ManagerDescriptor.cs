using Lab31.App.Domain;
using Lab31.App.Domain.Skills;

namespace Lab31.App.Metadata;

// Дескриптор описує тип "Manager" для системи валідації та створення об'єктів
public sealed class ManagerDescriptor : PersonDescriptor<Manager>
{
    // Приватний масив для збереження списку атрибутів (полів) менеджера
    private readonly AttributeSpec[] _attributes;

    // Конструктор: ініціалізує список атрибутів для менеджера
    public ManagerDescriptor()
    {
        _attributes = new[]
        {
            FirstNameAttribute(), // Поле імені (успадковане з базового класу)
            LastNameAttribute(),  // Поле прізвища (успадковане з базового класу)
            // Опис поля "Табельний номер" (обов'язкове)
            Attr("employeeId", "Табельний номер", Patterns.ManagerEmployeeId, Patterns.ManagerEmployeeIdHint, true, string.Empty,
                m => m.EmployeeId),
            // Опис поля "Відділ" (необов'язкове, має дефолтне значення)
            Attr("department", "Відділ (необов'язково)", Patterns.FreeText, Patterns.FreeTextHint, false, Manager.DefaultDepartment,
                m => m.Department),
        };
    }

    // Системне ім'я типу для збереження у файл
    public override string TypeName => "Manager";

    // Властивість, що повертає збережений масив атрибутів
    public override AttributeSpec[] Attributes => _attributes;

    // Метод створює готовий об'єкт Manager із масиву текстових значень
    protected override Manager CreateTyped(string[] values) => new Manager(
        values[0], // firstName
        values[1], // lastName
        values[2], // employeeId
        values[3], // department
                   // Передаємо масив навичок менеджера (самонавчання, викладання, шахи)
        new ISkill[] { new StudySkill("самонавчання"), new TeachSkill(), new ChessSkill() });
}