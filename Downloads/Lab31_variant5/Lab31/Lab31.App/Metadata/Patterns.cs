namespace Lab31.App.Metadata;

// Клас із регулярними виразами (Regex) та підказками для перевірки введених даних
public static class Patterns
{
    // Алфавіт: латинські та українські літери (великі й малі)
    private const string Letters = "A-Za-zА-Яа-яІіЇїЄєҐґ";

    // Шаблон для імені/прізвища (2–50 символів)
    public const string Name = "^[" + Letters + "][" + Letters + "'’\\-]{1,49}$";
    // Підказка до формату імені/прізвища
    public const string NameHint = "Допустимі лише літери, апостроф і дефіс (2–50 символів).";

    // Шаблон для назви країни (2–55 символів)
    public const string Country = "^[" + Letters + "][" + Letters + " '’\\-]{1,55}$";
    // Підказка до формату країни
    public const string CountryHint = "Допустимі лише літери, пробіл, апостроф і дефіс (наприклад, Україна).";

    // Шаблон для курсу (число від 1 до 6)
    public const string Course = "^[1-6]$";
    // Підказка до формату курсу
    public const string CourseHint = "Курс – ціле число від 1 до 6.";

    // Шаблон для студентського квитка (2 латинські літери + 6 цифр)
    public const string StudentId = "^[A-Z]{2}[0-9]{6}$";
    // Підказка до формату студентського квитка
    public const string StudentIdHint = "Формат: 2 великі латинські літери та 6 цифр (наприклад, KB123456).";

    // Шаблон для номера заліковки (8 цифр)
    public const string GradebookNumber = "^[0-9]{8}$";
    // Підказка до формату номера заліковки
    public const string GradebookNumberHint = "Формат: 8 цифр (наприклад, 20210001).";

    // Шаблон для середнього балу (число від 0 до 100)
    public const string AverageGrade = "^(100([.]0{1,2})?|[0-9]{1,2}([.][0-9]{1,2})?)$";
    // Підказка до формату середнього балу
    public const string AverageGradeHint = "Число від 0 до 100, дробова частина через крапку (наприклад, 87.5).";

    // Шаблон для табельного номера працівника МакДональдза (MC + 5 цифр)
    public const string WorkerEmployeeId = "^MC[0-9]{5}$";
    // Підказка до табельного номера працівника МакДональдза
    public const string WorkerEmployeeIdHint = "Формат: MC та 5 цифр (наприклад, MC00101).";

    // Шаблон для табельного номера менеджера (MG + 5 цифр)
    public const string ManagerEmployeeId = "^MG[0-9]{5}$";
    // Підказка до табельного номера менеджера
    public const string ManagerEmployeeIdHint = "Формат: MG та 5 цифр (наприклад, MG00001).";

    // Шаблон для довільних текстових полів, таких як відділ чи ресторан (2–60 символів)
    public const string FreeText = "^[" + Letters + "0-9][" + Letters + "0-9 .,'’\\-]{1,59}$";
    // Підказка до формату довільного тексту
    public const string FreeTextHint = "Допустимі літери, цифри, пробіл та символи . , ' - (2–60 символів).";
}