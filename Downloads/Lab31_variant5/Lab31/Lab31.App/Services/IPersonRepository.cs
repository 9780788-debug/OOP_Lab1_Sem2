using System;
using Lab31.App.Domain;

namespace Lab31.App.Services;

/// <summary>Access to the persons stored in the data source. Arrays are used instead of collections on purpose.</summary>
public interface IPersonRepository
{
    void Add(Person person);

    /// <summary>Reads the data source element by element and returns only the persons that satisfy the predicate.</summary>
    Person[] Find(Predicate<Person> match);

    /// <summary>Removes all persons that satisfy the predicate and returns how many were removed.</summary>
    int Remove(Predicate<Person> match);
}
