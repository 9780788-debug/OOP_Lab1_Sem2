using System;
using System.Text.RegularExpressions;
using Lab31.App.Domain;

namespace Lab31.App.Metadata;

// Клас описує правило валідації, відображення та зчитування одного поля сутності
public sealed class AttributeSpec
{
    // Збережений об'єкт регулярного виразу для перевірки введеного тексту
    private readonly Regex _pattern;

    // Делегат-функція, яка зчитує значення цього поля з об'єкта Person
    private readonly Func<Person, string> _getValue;

    // Конструктор: приймає параметри правила і формує Regex та делегат
    public AttributeSpec(
        string name,
        string label,
        string pattern,
        string formatHint,
        bool isRequired,
        string defaultValue,
        Func<Person, string> getValue)
    {
        // Перевіряємо, щоб системне ім'я не було порожнім
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Attribute name cannot be empty.", nameof(name));

        Name = name.Trim(); // Зберігаємо ключ атрибута (наприклад, "studentId")
        Label = string.IsNullOrWhiteSpace(label) ? Name : label.Trim(); // Назва для екрану
        FormatHint = formatHint ?? string.Empty; // Текст підказки про правильний формат
        IsRequired = isRequired; // Прапор обов'язковості
        DefaultValue = defaultValue ?? string.Empty; // Дефолтне значення для необов'язкових полів

        // Компілюємо регулярний вираз для подальших перевірок
        _pattern = new Regex(pattern ?? string.Empty, RegexOptions.CultureInvariant);

        // Зберігаємо функцію-геттер значення
        _getValue = getValue ?? throw new ArgumentNullException(nameof(getValue));
    }

    // Системна назва ключа для файлу збереження
    public string Name { get; }

    // Зрозуміла назва поля для виводу в консоль
    public string Label { get; }

    // Підказка про коректний формат
    public string FormatHint { get; }

    // Чи обов'язкове поле для заповнення
    public bool IsRequired { get; }

    // Значення за замовчуванням
    public string DefaultValue { get; }

    // Зчитує значення атрибута з переданого об'єкта особи
    public string GetValue(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));

        // Викликаємо функцію-геттер
        return _getValue(person);
    }

    // Валідує введений рядок: повертає null (якщо все ок) або текст помилки
    public string? Validate(string? input)
    {
        string text = (input ?? string.Empty).Trim();

        // Якщо ввели порожній рядок — перевіряємо чи поле обов'язкове
        if (text.Length == 0)
            return IsRequired ? $"Поле «{Label}» є обов'язковим." : null;

        // Перевіряємо регулярним виразом
        return _pattern.IsMatch(text)
            ? null
            : $"Поле «{Label}»: некоректне значення. {FormatHint}";
    }

    // Підставляє значення за замовчуванням, якщо необов'язкове поле залишили порожнім
    public string Normalize(string? input)
    {
        string text = (input ?? string.Empty).Trim();
        return text.Length == 0 ? DefaultValue : text;
    }
}