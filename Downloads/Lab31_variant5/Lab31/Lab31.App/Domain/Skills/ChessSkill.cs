namespace Lab31.App.Domain.Skills;
// Реалізація навички шахіста
public sealed class ChessSkill : IChessPlayer
{
    public int GamesPlayed { get; private set; }
    public void PlayChess() => GamesPlayed++;
}
