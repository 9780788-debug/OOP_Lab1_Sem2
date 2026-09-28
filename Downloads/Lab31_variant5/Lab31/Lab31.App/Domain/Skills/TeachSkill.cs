namespace Lab31.App.Domain.Skills;

// Навичка викладання
public sealed class TeachSkill : ITeacher
{
    public int LessonsGiven { get; private set; } 

    public void Teach() => LessonsGiven++; 
}
