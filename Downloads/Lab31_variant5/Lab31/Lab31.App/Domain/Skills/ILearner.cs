namespace Lab31.App.Domain.Skills;

// Інтерфейс для здатності вчитися
public interface ILearner : ISkill
{
    string Mode { get; } // Форма навчання (університет / самонавчання)
    int SessionsCompleted { get; } 
    void Study(); 
}
