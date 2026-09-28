using System;
using System.IO;
using System.Text;

namespace Lab31.Storage;

// Клас реалізує форматований текстовий запис об'єктів та їхніх атрибутів у файл
public sealed class TextRecordWriter : IRecordWriter
{
    private readonly StreamWriter _writer; // Потік для запису символів у кодуванні UTF-8
    private bool _inRecord;                 // Прапорець, що вказує, чи формується зараз запис
    private int _attributeCount;            // Лічильник записаних атрибутів для поточного об'єкта

    // Конструктор: відкриває потік запису в кодуванні UTF-8 без BOM
    public TextRecordWriter(Stream stream)
    {
        _writer = new StreamWriter(stream, new UTF8Encoding(false));
    }

    // Починає новий запис, записуючи заголовок (наприклад, "Student KB123456")
    public void BeginRecord(string typeName, string objectName)
    {
        if (_inRecord)
            throw new InvalidOperationException("The previous record has not been closed with EndRecord().");
        if (string.IsNullOrWhiteSpace(typeName) || typeName.Trim().IndexOf(' ') >= 0)
            throw new ArgumentException("Type name must be a non-empty word without spaces.", nameof(typeName));
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("Object name must not be empty.", nameof(objectName));

        _writer.WriteLine(typeName.Trim() + " " + objectName.Trim()); // Запис заголовка
        _inRecord = true;
        _attributeCount = 0; // Скидаємо лічильник атрибутів
    }

    // Записує одну пару "назва": "значення" з форматуванням
    public void WriteAttribute(string name, string value)
    {
        if (!_inRecord)
            throw new InvalidOperationException("WriteAttribute() is allowed only between BeginRecord() and EndRecord().");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Attribute name must not be empty.", nameof(name));

        // Перший атрибут відкривається дужкою '{ ', а наступні діляться комою з новим рядком
        _writer.Write(_attributeCount == 0 ? "{ " : "," + Environment.NewLine + "  ");
        _writer.Write('"');
        _writer.Write(Escape(name)); // Екрануємо та записуємо назву атрибута
        _writer.Write("\": \"");
        _writer.Write(Escape(value ?? string.Empty)); // Екрануємо та записуємо значення
        _writer.Write('"');
        _attributeCount++;
    }

    // Завершує формування поточного запису
    public void EndRecord()
    {
        if (!_inRecord)
            throw new InvalidOperationException("EndRecord() called without BeginRecord().");

        // Закриваємо фігурну дужку та додаємо крапку з комою
        _writer.Write(_attributeCount == 0 ? "{ };" : "};");
        _writer.WriteLine();
        _writer.WriteLine(); // Відступ між записами
        _inRecord = false;
    }

    // Звільняє ресурси потоку запису
    public void Dispose() => _writer.Dispose();

    // Допоміжний метод: екранує спецсимволи (\, ", \r, \n) для збереження у текстовому вигляді
    private static string Escape(string text) => text
        .Replace("\\", "\\\\")
        .Replace("\"", "\\\"")
        .Replace("\u201C", "\\\u201C")
        .Replace("\u201D", "\\\u201D")
        .Replace("\r", "\\r")
        .Replace("\n", "\\n");
}