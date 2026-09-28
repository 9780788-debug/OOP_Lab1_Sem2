using System;
using System.IO;
using Lab31.App.Actions;
using Lab31.App.Demo;
using Lab31.App.Domain;
using Lab31.App.Metadata;
using Lab31.App.Services;

namespace Lab31.App.UI;

// Головний клас текстового інтерфейсу користувача (меню)
public sealed class ConsoleMenu
{
    private const int TargetCourse = 3; // Цільовий курс для виконання Варіанта 5

    private readonly IPersonService _service;         // Сервіс бізнес-логіки
    private readonly IPersonTypeRegistry _types;      // Реєстр доступних типів сутностей
    private readonly IPersonAction[] _actions;        // Список дій над особами
    private readonly IDemoDataProvider _demoData;      // Провайдер тестових даних
    private readonly string _dataSourceName;           // Назва джерела даних (файлу)
    private readonly TextReader _input;               // Потік введення даних (наприклад, Console.In)
    private readonly TextWriter _output;              // Потік виведення даних (наприклад, Console.Out)

    // Конструктор з ін'єкцією залежностей для гнучкості та тестування
    public ConsoleMenu(
        IPersonService service,
        IPersonTypeRegistry types,
        IPersonAction[] actions,
        IDemoDataProvider demoData,
        string dataSourceName,
        TextReader input,
        TextWriter output)
    {
        _service = service;
        _types = types;
        _actions = actions;
        _demoData = demoData;
        _dataSourceName = dataSourceName;
        _input = input;
        _output = output;
    }

    // Запуск головного циклу обробки команд
    public void Run()
    {
        try
        {
            while (true)
            {
                ShowMenu();
                string choice = Ask("Ваш вибір");
                if (choice == "0")
                    return; // Вихід із програми

                Execute(choice);
            }
        }
        catch (EndOfStreamException)
        {
            // Зупинка роботи, якщо потік введення завершився
        }
    }

    // Виведення списку пунктів головного меню
    private void ShowMenu()
    {
        _output.WriteLine();
        _output.WriteLine($"=== База даних осіб (файл: {_dataSourceName}) ===");
        _output.WriteLine("1. Показати всі записи з файлу");
        _output.WriteLine("2. Додати особу (введення поелементно)");
        _output.WriteLine("3. Завантажити демонстраційні дані (10 осіб)");
        _output.WriteLine("4. Знайти за прізвищем");
        _output.WriteLine("5. Знайти за унікальним ідентифікатором");
        _output.WriteLine("6. Видалити за унікальним ідентифікатором");
        _output.WriteLine($"7. Варіант 5: студенти {TargetCourse}-го курсу, які проживають в Україні");
        _output.WriteLine("8. Виконати дію особи (Study / Teach / PlayChess)");
        _output.WriteLine("0. Вихід");
    }

    // Маршрутизація обраного пункту меню до відповідного методу
    private void Execute(string choice)
    {
        try
        {
            switch (choice)
            {
                case "1": ShowAll(); break;
                case "2": AddPerson(); break;
                case "3": LoadDemoData(); break;
                case "4": FindByLastName(); break;
                case "5": FindById(); break;
                case "6": DeleteById(); break;
                case "7": ShowVariantTask(); break;
                case "8": PerformActions(); break;
                default: _output.WriteLine("Невідомий пункт меню."); break;
            }
        }
        catch (Exception ex) when (ex is not EndOfStreamException)
        {
            _output.WriteLine($"Помилка: {ex.Message}");
        }
    }

    // Виведення всіх осіб з бази даних
    private void ShowAll()
    {
        Person[] all = _service.GetAll();
        PrintPersons(all, "У файлі немає записів.");
    }

    // Інтерактивне додавання нової особи
    private void AddPerson()
    {
        IPersonDescriptor descriptor = ChooseType();
        AttributeSpec[] specs = descriptor.Attributes;

        string[] values = new string[specs.Length];
        for (int i = 0; i < specs.Length; i++)
            values[i] = AskAttribute(specs[i]); // Поелементне зчитування та валідація

        Person person = _service.Add(descriptor, values);
        _output.WriteLine($"Додано: {descriptor.TypeName} {person.UniqueId}");
    }

    // Автоматичне завантаження демонстраційного набору записів
    private void LoadDemoData()
    {
        DemoRecord[] records = _demoData.GetRecords();
        int added = 0;

        foreach (DemoRecord record in records)
        {
            IPersonDescriptor? descriptor = _types.FindByTypeName(record.TypeName);
            if (descriptor == null)
            {
                _output.WriteLine($"- пропущено: невідомий тип {record.TypeName}");
                continue;
            }

            try
            {
                Person person = _service.Add(descriptor, record.Values);
                added++;
                _output.WriteLine($"+ {record.TypeName} {person.UniqueId}");
            }
            catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
            {
                _output.WriteLine($"- пропущено: {ex.Message}");
            }
        }

        _output.WriteLine($"Додано {added} з {records.Length}.");
    }

    // Пошук осіб за прізвищем
    private void FindByLastName()
    {
        string lastName = Ask("Прізвище");
        PrintPersons(_service.FindByLastName(lastName), "Осіб з таким прізвищем не знайдено.");
    }

    // Пошук за унікальним номером/ідентифікатором
    private void FindById()
    {
        string id = Ask("Унікальний ідентифікатор");
        Person? person = _service.FindById(id);
        if (person == null)
            _output.WriteLine("Особу не знайдено.");
        else
            PrintPerson(person);
    }

    // Видалення особи за унікальним номером
    private void DeleteById()
    {
        string id = Ask("Унікальний ідентифікатор");
        int removed = _service.DeleteById(id);
        _output.WriteLine(removed > 0 ? $"Видалено записів: {removed}" : "Особу не знайдено.");
    }

    // Виконання спеціального завдання за варіантом 5
    private void ShowVariantTask()
    {
        Student[] students = _service.FindStudents(TargetCourse, CountryNames.Ukraine);
        _output.WriteLine($"Кількість студентів {TargetCourse}-го курсу, які проживають в Україні: {students.Length}");

        foreach (Student student in students)
            PrintPerson(student);
    }

    // Меню виклику поведінкових дій особи (наприклад, зіграти в шахи)
    private void PerformActions()
    {
        string id = Ask("Унікальний ідентифікатор особи");
        Person? person = _service.FindById(id);
        if (person == null)
        {
            _output.WriteLine("Особу не знайдено.");
            return;
        }

        _output.WriteLine($"Особа: {person.FirstName} {person.LastName}");

        while (true)
        {
            // Вибір лише тих дій, які доступні для цієї конкретної особи
            int[] available = new int[_actions.Length];
            int count = 0;
            for (int i = 0; i < _actions.Length; i++)
            {
                if (_actions[i].AppliesTo(person))
                    available[count++] = i;
            }

            if (count == 0)
            {
                _output.WriteLine("Для цієї особи немає доступних дій.");
                return;
            }

            _output.WriteLine("Доступні дії (0 – повернутися до меню):");
            for (int i = 0; i < count; i++)
                _output.WriteLine($"  {i + 1}. {_actions[available[i]].Name}");

            int choice = AskNumber("Ваш вибір", 0, count);
            if (choice == 0)
                return;

            // Виконання обраної дії
            _output.WriteLine(_actions[available[choice - 1]].Execute(person));
        }
    }

    // Підменю для вибору типу особи при її створенні
    private IPersonDescriptor ChooseType()
    {
        _output.WriteLine("Оберіть тип сутності:");
        for (int i = 0; i < _types.Count; i++)
            _output.WriteLine($"  {i + 1}. {_types[i].TypeName}");

        int number = AskNumber("Номер типу", 1, _types.Count);
        return _types[number - 1];
    }

    // Форматоване виведення масиву осіб
    private void PrintPersons(Person[] persons, string emptyMessage)
    {
        if (persons.Length == 0)
        {
            _output.WriteLine(emptyMessage);
            return;
        }

        _output.WriteLine($"Знайдено записів: {persons.Length}");
        foreach (Person person in persons)
            PrintPerson(person);
    }

    // Виведення одного об'єкта особи та всіх її атрибутів
    private void PrintPerson(Person person)
    {
        IPersonDescriptor descriptor = _types.GetFor(person);

        _output.WriteLine($"[{descriptor.TypeName}] {person.UniqueId}");
        foreach (AttributeSpec spec in descriptor.Attributes)
            _output.WriteLine($"    {spec.Label}: {spec.GetValue(person)}");
    }

    // Запит значення атрибута з повтором спроб у разі помилки валідації
    private string AskAttribute(AttributeSpec spec)
    {
        string prompt = spec.IsRequired ? spec.Label : $"{spec.Label} [за замовчуванням: {spec.DefaultValue}]";

        while (true)
        {
            string input = Ask(prompt);
            string? error = spec.Validate(input);
            if (error == null)
                return input; // Успішна валідація

            _output.WriteLine(error); // Виведення повідомлення про помилку
        }
    }

    // Допоміжний метод для зчитування числа в заданому діапазоні
    private int AskNumber(string prompt, int min, int max)
    {
        while (true)
        {
            string text = Ask(prompt);
            if (int.TryParse(text, out int number) && number >= min && number <= max)
                return number;

            _output.WriteLine($"Введіть ціле число від {min} до {max}.");
        }
    }

    // Базовий метод для зчитування рядка з консолі/потоку з підказкою
    private string Ask(string prompt)
    {
        _output.Write($"{prompt}: ");
        string? line = _input.ReadLine();
        if (line == null)
            throw new EndOfStreamException();

        return line.Trim();
    }
}