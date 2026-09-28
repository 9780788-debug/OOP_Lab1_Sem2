using System;

namespace Lab31.Storage;

// Інтерфейс для послідовного запису сутностей та їх атрибутів у файл даних
public interface IRecordWriter : IDisposable
{
    // Починає новий запис, записуючи назву типу та ім'я об'єкта
    void BeginRecord(string typeName, string objectName);

    // Записує один атрибут (назву та його значення) у поточний запис
    void WriteAttribute(string name, string value);

    // Завершує формування поточного запису
    void EndRecord();
}