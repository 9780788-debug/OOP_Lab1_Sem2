using Lab31.App.Domain.Skills;

namespace Lab31.App.Domain;

public sealed class McdonaldsWorker : Person
{
    // Значення за замовчуванням для назви ресторану
    public const string DefaultRestaurant = "Unknown";

    // Конструктор працівника МакДональдза
    public McdonaldsWorker(string firstName, string lastName, string employeeId, string? restaurant, ISkill[]? skills)
        : base(firstName, lastName, skills) // Передаємо спільні поля в Person
    {
        EmployeeId = RequireText(employeeId, nameof(employeeId));

        // Якщо ресторан не вказано — ставимо "Unknown"
        Restaurant = string.IsNullOrWhiteSpace(restaurant) ? DefaultRestaurant : restaurant.Trim();
    }

    // Табельний номер
    public string EmployeeId { get; }
    // Назва закладу/ресторану
    public string Restaurant { get; }
    // Унікальним ідентифікатором є табельний номер
    public override string UniqueId => EmployeeId;
}