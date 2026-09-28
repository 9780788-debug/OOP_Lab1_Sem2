using System;

namespace Lab31.App.Domain.Skills;
// Реалізація навички навчання
public sealed class StudySkill : ILearner
{
    // Конструктор: приймає режим навчання (наприклад, "самонавчання")
    public StudySkill(string mode)
    {
        // Перевіряємо, щоб режим не був порожнім
        if (string.IsNullOrWhiteSpace(mode))
            throw new ArgumentException("Study mode must be specified.", nameof(mode));

        Mode = mode; // Записуємо режим у властивість
    }

    public string Mode { get; } 

    public int SessionsCompleted { get; private set; } 

    public void Study() => SessionsCompleted++;
}
