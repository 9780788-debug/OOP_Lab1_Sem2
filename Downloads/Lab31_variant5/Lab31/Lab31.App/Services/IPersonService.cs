using Lab31.App.Domain;
using Lab31.App.Metadata;

namespace Lab31.App.Services;

public interface IPersonService
{
    /// <summary>Validates the raw values, creates the person and stores it. Throws if the identifier is already used.</summary>
    Person Add(IPersonDescriptor descriptor, string[] rawValues);

    Person[] GetAll();

    Person[] FindByLastName(string lastName);

    Person? FindById(string uniqueId);

    /// <summary>Deletes the person with the given identifier; returns how many records were removed.</summary>
    int DeleteById(string uniqueId);

    /// <summary>Variant 5: students of the given course that live in the given country, read from the file.</summary>
    Student[] FindStudents(int course, string country);
}
