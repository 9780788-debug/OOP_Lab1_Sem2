using System;
using Lab31.App.Domain;
using Lab31.App.Domain.Skills;

namespace Lab31.App.Actions;

// Клас реалізує дію "Зіграти в шахи" над особами
public sealed class PlayChessAction : IPersonAction
{
    // Назва дії для відображення у меню користувача
    public string Name => "PlayChess() – грати в шахи";

    // Перевіряє, чи має особа навичку гри в шахи (реалізує інтерфейс IChessPlayer)
    public bool AppliesTo(Person person) => person.Can<IChessPlayer>();

    // Виконує дію гри в шахи для особи та повертає оновлену кількість партій
    public string Execute(Person person)
    {
        // Отримуємо інтерфейс навички шахіста
        IChessPlayer player = person.GetSkill<IChessPlayer>()
            ?? throw new InvalidOperationException("The person cannot play chess.");

        player.PlayChess(); // Збільшуємо лічильник зіграних партій
        return $"Зіграно партій у шахи: {player.GamesPlayed}";
    }
}