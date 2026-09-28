using System; // Підключаємо базові типи системної бібліотеки .NET
using Lab31.App.Domain.Skills; // Підключаємо простір імен навичок (ISkill)

namespace Lab31.App.Domain; // Вказуємо простір імен для сутностей предметної області

/// Базовий абстрактний клас для будь-якої людини (Student, Manager, McdonaldsWorker).
/// Поведінка не успадковується, а складається з об'єктів ISkill (композиція).

public abstract class Person // abstract означає, що екземпляр "просто Person" створити неможливо
{
    // Приватне поле для збереження масиву навичок, якими володіє людина
    private readonly ISkill[] _skills;

    // Захищений конструктор, який викликається лише з конструкторів дочірніх класів
    protected Person(string firstName, string lastName, ISkill[]? skills)
    {
        // Перевіряємо та записуємо ім'я
        FirstName = RequireText(firstName, nameof(firstName));
        // Перевіряємо та записуємо прізвище
        LastName = RequireText(lastName, nameof(lastName));
        // Якщо передано null у skills, присвоюємо порожній масив, інакше збережемо переданий масив
        _skills = skills ?? Array.Empty<ISkill>();
    }

    // Автоматична властивість імені (тільки для зчитання ззовні)
    public string FirstName { get; }

    // Автоматична властивість прізвища (тільки для зчитання ззовні)
    public string LastName { get; }

    // Абстрактний унікальний ідентифікатор особи (студентський, табельний номер тощо)
    public abstract string UniqueId { get; }

    // Шукає та повертає навичку заданого типу T або null, якщо її немає
    public T? GetSkill<T>() where T : class, ISkill
    {
        // Проходимо по всьому масиву навичок об'єкта
        foreach (ISkill skill in _skills)
        {
            // Перевіряємо, чи поточна навичка є типом T (або реалізує інтерфейс T)
            if (skill is T match)
                return match; // Знайшли — повертаємо
        }

        return null; // Навичку заданого типу не знайдено
    }

    //Перевіряє, чи має особа навичку типу T
    public bool Can<T>() where T : class, ISkill => GetSkill<T>() != null;

    //Допоміжний захищений метод для перевірки обов'язковості текстових полів
    protected static string RequireText(string? value, string parameterName)
    {
        // Якщо рядок порожній або містить лише пробіли — кидаємо помилку
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A mandatory attribute has no value.", parameterName);

        // Повертаємо очищений від крайових пробілів рядок
        return value.Trim();
    }
}