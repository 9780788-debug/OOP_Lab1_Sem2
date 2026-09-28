namespace Lab31.App.Domain.Skills;

// Інтерфейс для здатності вчити інших
public interface ITeacher : ISkill
{
    int LessonsGiven { get; }
    void Teach(); 
}
