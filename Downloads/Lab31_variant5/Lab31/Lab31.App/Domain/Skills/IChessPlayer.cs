namespace Lab31.App.Domain.Skills;

// Інтерфейс для навички гри в шахи
public interface IChessPlayer : ISkill
{
    int GamesPlayed { get; } 

    void PlayChess(); 
}
