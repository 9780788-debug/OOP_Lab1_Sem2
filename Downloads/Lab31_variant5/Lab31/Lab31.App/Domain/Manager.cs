using Lab31.App.Domain.Skills; // Підключаємо інтерфейси навичок

namespace Lab31.App.Domain;

public sealed class Manager : Person // Забороняємо успадкування від Manager
{
    // Значення за замовчуванням для відділу, якщо його не вказано
    public const string DefaultDepartment = "General";

    // Конструктор менеджера
    public Manager(string firstName, string lastName, string employeeId, string? department, ISkill[]? skills)
        : base(firstName, lastName, skills)
    {
        // Табельний номер є обов'язковим
        EmployeeId = RequireText(employeeId, nameof(employeeId));
        // Якщо відділ порожній — присвоюємо "General", інакше обрізаємо пробіли
        Department = string.IsNullOrWhiteSpace(department) ? DefaultDepartment : department.Trim();
    }

    // Табельний номер працівника
    public string EmployeeId { get; }
    // Назва відділу
    public string Department { get; }
    // Унікальним ідентифікатором менеджера є його табельний номер
    public override string UniqueId => EmployeeId;
}
