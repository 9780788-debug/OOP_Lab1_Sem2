using System;
using Lab31.App.Domain;
using Lab31.App.Domain.Skills;

namespace Lab31.App.Actions;

// Клас реалізує дію "Навчатися" над особами
public sealed class StudyAction : IPersonAction
{
    // Назва дії для відображення у меню користувача
    public string Name => "Study() – навчатися";

    // Перевіряє, чи має особа навичку навчання (реалізує інтерфейс ILearner)
    public bool AppliesTo(Person person) => person.Can<ILearner>();

    // Виконує дію навчання для особи та повертає оновлений стан
    public string Execute(Person person)
    {
        // Отримуємо інтерфейс навички студента/учня
        ILearner learner = person.GetSkill<ILearner>()
            ?? throw new InvalidOperationException("The person cannot study.");

        learner.Study(); // Збільшуємо лічильник проведених занять
        return $"{learner.Mode}: проведено занять – {learner.SessionsCompleted}";
    }
}