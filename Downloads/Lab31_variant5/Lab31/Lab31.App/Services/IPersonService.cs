using Lab31.App.Domain;
using Lab31.App.Metadata;

namespace Lab31.App.Services;

// Інтерфейс сервісу бізнес-логіки для управління об'єктами осіб
public interface IPersonService
{
    // Валідує вхідні значення, створює особу та зберігає її у сховище
    Person Add(IPersonDescriptor descriptor, string[] rawValues);

    // Повертає масив усіх збережених осіб
    Person[] GetAll();

    // Шукає осіб за прізвищем
    Person[] FindByLastName(string lastName);

    // Шукає особу за її унікальним ідентифікатором
    Person? FindById(string uniqueId);

    // Видаляє особу за її унікальним ідентифікатором та повертає кількість видалених записів
    int DeleteById(string uniqueId);

    // Завдання Варіанта 5: повертає студентів вказаного курсу, які проживають у зазначеній країні
    Student[] FindStudents(int course, string country);
}