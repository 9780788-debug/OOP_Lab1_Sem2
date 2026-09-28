using System;
using System.IO;
using System.Text;
using Lab31.App.Actions;
using Lab31.App.Demo;
using Lab31.App.Metadata;
using Lab31.App.Services;
using Lab31.App.UI;
using Lab31.Storage;

// Встановлюємо кодування UTF-8 для коректного відображення кирилиці в консолі
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Визначаємо шлях до файлу бази даних (із аргументів командного рядка або за замовчуванням "data/people.txt")
string dataFile = args.Length > 0 ? args[0] : Path.Combine("data", "people.txt");

// 1. Ініціалізуємо текстове сховище записів
IRecordStorage storage = new TextFileRecordStorage(dataFile);

// 2. Реєструємо всі доступні типи сутностей та їхні дескриптори
IPersonTypeRegistry types = new PersonTypeRegistry(
    new StudentDescriptor(),
    new McdonaldsWorkerDescriptor(),
    new ManagerDescriptor());

// 3. Створюємо репозиторій для зчитання та збереження даних
IPersonRepository repository = new PersonRepository(storage, types);

// 4. Ініціалізуємо сервіс бізнес-логіки
IPersonService service = new PersonService(repository);

// 5. Формуємо список доступних дій над особами
IPersonAction[] actions = { new StudyAction(), new TeachAction(), new PlayChessAction() };

// 6. Створюємо екземпляр консольного меню та передаємо всі залежності
var menu = new ConsoleMenu(service, types, actions, new DemoDataProvider(), dataFile, Console.In, Console.Out);

// 7. Запускаємо головний цикл роботи користувача з програмою
menu.Run();