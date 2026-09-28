using System;
using Lab31.App.Domain;

namespace Lab31.App.Services;

// Інтерфейс репозиторію для доступу до збережених осіб у джерелі даних
public interface IPersonRepository
{
    // Додає нову особу до сховища
    void Add(Person person);

    // Попотоково зчитує джерело даних і повертає тільки тих осіб, які відповідають предикату
    Person[] Find(Predicate<Person> match);

    // Видаляє всіх осіб, що відповідають предикату, та повертає кількість видалених записів
    int Remove(Predicate<Person> match);
}