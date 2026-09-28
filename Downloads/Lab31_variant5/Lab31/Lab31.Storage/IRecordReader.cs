using System;

namespace Lab31.Storage;

/// Послідовний засіб читання записів із файлу даних (елемент за елементом).
/// Він не має жодних відомостей про бізнес-сутності: запис складається з назви типу,
/// назви об'єкта та послідовності пар «назва атрибута — значення атрибута».

// Інтерфейс для послідовного зчитування записів із файлу даних
public interface IRecordReader : IDisposable
{
    // Зчитує заголовок нового запису (назву типу та ім'я об'єкта); повертає false, якщо досягнуто кінця файлу
    bool ReadRecordHeader(out string typeName, out string objectName);
    // Зчитує наступну пару "назва атрибута – значення" для поточного запису; повертає false, коли атрибути запису закінчилися    bool ReadAttribute(out string name, out string value);
    bool ReadAttribute(out string name, out string value);
}
