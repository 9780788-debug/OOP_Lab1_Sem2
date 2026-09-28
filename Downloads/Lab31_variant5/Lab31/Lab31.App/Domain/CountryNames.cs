using System;

namespace Lab31.App.Domain;

public static class CountryNames
{
    public const string Ukraine = "Україна";

    // Метод перевіряє, чи дві назви країн означають одне й те саме
    public static bool IsSame(string first, string second) =>
        string.Equals(Canonical(first), Canonical(second), StringComparison.OrdinalIgnoreCase);

    // Внутрішній метод канонізації рядка
    private static string Canonical(string name)
    {
        string trimmed = name.Trim(); // Очищаємо пробіли
        // Якщо написано англійською "Ukraine" — зводимо до українського "Україна"
        return string.Equals(trimmed, "Ukraine", StringComparison.OrdinalIgnoreCase) ? Ukraine : trimmed;
    }
}