using System; // Підключаємо системні типи (ArgumentOutOfRangeException)
using Lab31.App.Domain.Skills; // Підключаємо простір імен навичок

namespace Lab31.App.Domain;

public sealed class Student : Person // sealed забороняє успадкування від класу Student
{
    public const int MinCourse = 1;
    public const int MaxCourse = 6;
    public const double MaxAverageGrade = 100.0;

    // Конструктор студента
    public Student(
        string firstName,
        string lastName,
        int course,
        string studentId,
        double averageGrade,
        string country,
        string gradebookNumber,
        ISkill[]? skills)
        : base(firstName, lastName, skills) // Передаємо ім'я, прізвище та навички в базовий конструктор Person
    {
        if (course < MinCourse || course > MaxCourse)
            throw new ArgumentOutOfRangeException(nameof(course), $"Course must be in range {MinCourse}..{MaxCourse}.");
        
        if (averageGrade < 0 || averageGrade > MaxAverageGrade)
            throw new ArgumentOutOfRangeException(nameof(averageGrade), $"Average grade must be in range 0..{MaxAverageGrade}.");

        // Записуємо валідовані значення у властивості
        Course = course;
        StudentId = RequireText(studentId, nameof(studentId));
        AverageGrade = averageGrade;
        Country = RequireText(country, nameof(country));
        GradebookNumber = RequireText(gradebookNumber, nameof(gradebookNumber));
    }

    public int Course { get; }

    public string StudentId { get; }

    public double AverageGrade { get; }

    public string Country { get; }

    public string GradebookNumber { get; }

    // Перевизначаємо UniqueId: для студента це номер його студентського квитка
    public override string UniqueId => StudentId;
}
