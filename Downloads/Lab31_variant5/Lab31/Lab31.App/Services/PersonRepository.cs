using System;
using System.IO;
using Lab31.App.Domain;
using Lab31.App.Metadata;
using Lab31.Storage;

namespace Lab31.App.Services;

/// <summary>
/// Translates between persons and the abstract records of <see cref="IRecordStorage"/>.
/// Everything is written and read attribute by attribute, never as a whole object.
/// </summary>
public sealed class PersonRepository : IPersonRepository
{
    private readonly IRecordStorage _storage;
    private readonly IPersonTypeRegistry _types;

    public PersonRepository(IRecordStorage storage, IPersonTypeRegistry types)
    {
        _storage = storage;
        _types = types;
    }

    public void Add(Person person)
    {
        using IRecordWriter writer = _storage.OpenAppender();
        WritePerson(writer, person);
    }

    public Person[] Find(Predicate<Person> match)
    {
        Person[] result = Array.Empty<Person>();
        if (!_storage.Exists)
            return result;

        using IRecordReader reader = _storage.OpenReader();
        Person? person;
        while ((person = ReadPerson(reader)) != null)
        {
            if (match(person))
            {
                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = person;
            }
        }

        return result;
    }

    public int Remove(Predicate<Person> match)
    {
        if (!_storage.Exists)
            return 0;

        int removed = 0;
        using (IRecordReader reader = _storage.OpenReader())
        using (IRecordWriter writer = _storage.BeginRewrite())
        {
            Person? person;
            while ((person = ReadPerson(reader)) != null)
            {
                if (match(person))
                    removed++;
                else
                    WritePerson(writer, person);
            }
        }

        _storage.CommitRewrite();
        return removed;
    }

    private void WritePerson(IRecordWriter writer, Person person)
    {
        IPersonDescriptor descriptor = _types.GetFor(person);

        writer.BeginRecord(descriptor.TypeName, (person.FirstName + person.LastName).Replace(" ", string.Empty));
        foreach (AttributeSpec spec in descriptor.Attributes)
            writer.WriteAttribute(spec.Name, spec.GetValue(person));
        writer.EndRecord();
    }

    private Person? ReadPerson(IRecordReader reader)
    {
        if (!reader.ReadRecordHeader(out string typeName, out string objectName))
            return null;

        IPersonDescriptor descriptor = _types.FindByTypeName(typeName)
            ?? throw new InvalidDataException($"Невідомий тип сутності '{typeName}' (запис '{objectName}').");

        AttributeSpec[] specs = descriptor.Attributes;
        string?[] raw = new string?[specs.Length];

        while (reader.ReadAttribute(out string name, out string value))
        {
            int index = IndexOf(specs, name);
            if (index < 0)
                throw new InvalidDataException($"Невідомий атрибут '{name}' у записі '{typeName} {objectName}'.");

            raw[index] = value;
        }

        string[] values = new string[specs.Length];
        for (int i = 0; i < specs.Length; i++)
        {
            string? error = specs[i].Validate(raw[i]);
            if (error != null)
                throw new InvalidDataException($"Запис '{typeName} {objectName}': {error}");

            values[i] = specs[i].Normalize(raw[i]);
        }

        return descriptor.Create(values);
    }

    private static int IndexOf(AttributeSpec[] specs, string name)
    {
        for (int i = 0; i < specs.Length; i++)
        {
            if (string.Equals(specs[i].Name, name, StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1;
    }
}
