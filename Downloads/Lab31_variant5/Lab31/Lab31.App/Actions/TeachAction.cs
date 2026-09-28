using System;
using Lab31.App.Domain;
using Lab31.App.Domain.Skills;

namespace Lab31.App.Actions;

// Клас реалізує дію "Навчати інших" над особами
public sealed class TeachAction : IPersonAction
{
    // Назва дії для відображення у меню користувача
    public string Name => "Teach() – навчати інших";

    // Перевіряє, чи має особа навичку викладання (реалізує інтерфейс ITeacher)
    public bool AppliesTo(Person person) => person.Can<ITeacher>();

    // Виконує дію викладання для особи та повертає оновлений стан
    public string Execute(Person person)
    {
        // Отримуємо інтерфейс навички викладача
        ITeacher teacher = person.GetSkill<ITeacher>()
            ?? throw new InvalidOperationException("The person cannot teach.");

        teacher.Teach(); // Збільшуємо лічильник проведених уроків/тренінгів
        return $"Проведено тренінгів/уроків: {teacher.LessonsGiven}";
    }
}