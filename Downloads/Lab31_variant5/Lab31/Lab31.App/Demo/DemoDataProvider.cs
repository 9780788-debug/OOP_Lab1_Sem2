namespace Lab31.App.Demo;

// Провайдер демонстраційних даних для тестування програми (містить 10 осіб різних типів)
public sealed class DemoDataProvider : IDemoDataProvider
{
    // Повертає масив тестових записів для завантаження в базу даних
    public DemoRecord[] GetRecords() => new[]
    {
        // Студенти (Student): ім'я, прізвище, курс, ID студента, середній бал, країна, номер залікової книжки
        new DemoRecord("Student", new[] { "Ivan", "Petrenko", "3", "KB123456", "85.5", "Україна", "20210001" }),
        new DemoRecord("Student", new[] { "Oksana", "Shevchenko", "3", "KB123457", "92", "Україна", "20210002" }),
        new DemoRecord("Student", new[] { "Andrii", "Kovalenko", "2", "KB123458", "74.25", "Україна", "20220003" }),
        new DemoRecord("Student", new[] { "John", "Smith", "3", "KB123459", "88", "Poland", "20210004" }),
        new DemoRecord("Student", new[] { "Maria", "Bondarenko", "3", "KB123460", "95", "Ukraine", "20210005" }),
        new DemoRecord("Student", new[] { "Dmytro", "Melnyk", "1", "KB123461", "79", "Україна", "20230006" }),

        // Працівники McDonald's (McdonaldsWorker): ім'я, прізвище, ID працівника, назва ресторану (необов'язково)
        new DemoRecord("McdonaldsWorker", new[] { "Olena", "Tkachenko", "MC00101", "McDonald's Khreshchatyk" }),
        new DemoRecord("McdonaldsWorker", new[] { "Serhii", "Marchenko", "MC00102", "" }),

        // Менеджери (Manager): ім'я, прізвище, ID працівника, відділ (необов'язково)
        new DemoRecord("Manager", new[] { "Natalia", "Kravchenko", "MG00001", "Operations" }),
        new DemoRecord("Manager", new[] { "Viktor", "Lysenko", "MG00002", "" }),
    };
}