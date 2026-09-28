using Lab31.App.Domain;
using Lab31.App.Domain.Skills;

namespace Lab31.App.Metadata;

// Дескриптор для типу "Student"
public sealed class StudentDescriptor : PersonDescriptor<Student>
{
    private readonly AttributeSpec[] _attributes;

    public StudentDescriptor()
    {
        _attributes = new[]
        {
            FirstNameAttribute(),
            LastNameAttribute(),
            Attr("course", "Курс", Patterns.Course, Patterns.CourseHint, true, string.Empty, s => s.Course.ToString()),
            Attr("studentId", "Студентський квиток", Patterns.StudentId, Patterns.StudentIdHint, true, string.Empty, s => s.StudentId),
            Attr("averageGrade", "Середній бал", Patterns.AverageGrade, Patterns.AverageGradeHint, true, string.Empty, s => s.AverageGrade.ToString("F1")),
            Attr("country", "Країна", Patterns.Country, Patterns.CountryHint, true, string.Empty, s => s.Country),
            Attr("gradebookNumber", "Залікова книжка", Patterns.GradebookNumber, Patterns.GradebookNumberHint, true, string.Empty, s => s.GradebookNumber)
        };
    }

    public override string TypeName => "Student";

    public override AttributeSpec[] Attributes => _attributes;

    protected override Student CreateTyped(string[] values) => new Student(
        values[0],               // firstName
        values[1],               // lastName
        int.Parse(values[2]),    // course
        values[3],               // studentId
        double.Parse(values[4]), // averageGrade
        values[5],               // country
        values[6],               // gradebookNumber
        new ISkill[] { new StudySkill("університет"), new ChessSkill() }); // Навички студента
}