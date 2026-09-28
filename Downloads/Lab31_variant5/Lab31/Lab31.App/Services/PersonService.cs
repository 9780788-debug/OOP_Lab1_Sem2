using System;
using Lab31.App.Domain;
using Lab31.App.Metadata;

namespace Lab31.App.Services;

public sealed class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public Person Add(IPersonDescriptor descriptor, string[] rawValues)
    {
        AttributeSpec[] specs = descriptor.Attributes;
        if (rawValues.Length != specs.Length)
            throw new ArgumentException("Кількість значень не відповідає кількості атрибутів.", nameof(rawValues));

        string[] values = new string[specs.Length];
        for (int i = 0; i < specs.Length; i++)
        {
            string? error = specs[i].Validate(rawValues[i]);
            if (error != null)
                throw new ArgumentException(error, nameof(rawValues));

            values[i] = specs[i].Normalize(rawValues[i]);
        }

        Person person = descriptor.Create(values);

        if (FindById(person.UniqueId) != null)
            throw new InvalidOperationException($"Особа з ідентифікатором {person.UniqueId} вже існує.");

        _repository.Add(person);
        return person;
    }

    public Person[] GetAll() => _repository.Find(_ => true);

    public Person[] FindByLastName(string lastName)
    {
        string wanted = lastName.Trim();
        return _repository.Find(p => string.Equals(p.LastName, wanted, StringComparison.OrdinalIgnoreCase));
    }

    public Person? FindById(string uniqueId)
    {
        string wanted = uniqueId.Trim();
        Person[] found = _repository.Find(p => string.Equals(p.UniqueId, wanted, StringComparison.OrdinalIgnoreCase));
        return found.Length > 0 ? found[0] : null;
    }

    public int DeleteById(string uniqueId)
    {
        string wanted = uniqueId.Trim();
        return _repository.Remove(p => string.Equals(p.UniqueId, wanted, StringComparison.OrdinalIgnoreCase));
    }

    public Student[] FindStudents(int course, string country)
    {
        Person[] found = _repository.Find(p =>
            p is Student student
            && student.Course == course
            && CountryNames.IsSame(student.Country, country));

        Student[] students = new Student[found.Length];
        for (int i = 0; i < found.Length; i++)
            students[i] = (Student)found[i];

        return students;
    }
}
