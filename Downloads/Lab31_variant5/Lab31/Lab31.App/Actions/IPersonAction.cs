using Lab31.App.Domain;

namespace Lab31.App.Actions;

// Інтерфейс для поведінкових дій, які може виконувати особа
public interface IPersonAction
{
    // Назва дії для відображення у меню користувача
    string Name { get; }

    // Перевіряє, чи підтримує дане екземпляр особи цю дію (за наявністю відповідної навички)
    bool AppliesTo(Person person);

    // Виконує дію, змінює стан особи та повертає короткий опис результату
    string Execute(Person person);
}