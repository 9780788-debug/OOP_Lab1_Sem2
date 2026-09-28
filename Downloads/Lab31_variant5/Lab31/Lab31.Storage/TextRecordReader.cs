using System;
using System.IO;
using System.Text;

namespace Lab31.Storage;

// Клас реалізує зчитування записів у текстовому форматі із потоку
public sealed class TextRecordReader : IRecordReader
{
    private readonly StreamReader _reader; // Потік для зчитування файлу в кодуванні UTF-8
    private bool _inRecord;                 // Прапорець, який вказує, чи знаходимося ми зараз усередині запису

    // Конструктор: приймає потік і налаштовує StreamReader у UTF-8
    public TextRecordReader(Stream stream)
    {
        _reader = new StreamReader(stream, new UTF8Encoding(false));
    }

    // Зчитує заголовок запису (наприклад, "Student KB123456") і відкриваючу дужку '{'
    public bool ReadRecordHeader(out string typeName, out string objectName)
    {
        typeName = string.Empty;
        objectName = string.Empty;

        // Якщо попередній запис не дочитано до кінця — пропускаємо всі його залишені атрибути
        while (_inRecord && ReadAttribute(out _, out _))
        {
        }

        // Шукаємо перший непорожній рядок заголовка
        string? line;
        do
        {
            line = _reader.ReadLine();
            if (line == null)
                return false; // Кінець файлу
            line = line.Trim();
        }
        while (line.Length == 0);

        // Шукаємо пробіл між назвою типу та ім'ям об'єкта
        int space = IndexOfWhiteSpace(line);
        if (space < 0)
            throw new InvalidDataException($"Record header '{line}' must look like 'Type ObjectName'.");

        typeName = line.Substring(0, space);          // Витягуємо тип (наприклад, "Student")
        objectName = line.Substring(space + 1).Trim(); // Витягуємо ім'я об'єкта

        SkipWhiteSpace();
        // Перевіряємо наявність дужки '{', що знаменує початок блоку атрибутів
        if (_reader.Read() != '{')
            throw new InvalidDataException($"Expected '{{' after the header '{line}'.");

        _inRecord = true; // Позначаємо, що ми усередині запису
        return true;
    }

    // Зчитує один атрибут у форматі "назва": "значення"
    public bool ReadAttribute(out string name, out string value)
    {
        name = string.Empty;
        value = string.Empty;

        if (!_inRecord)
            return false;

        SkipSeparators(); // Пропускаємо пробіли та коми
        int next = _reader.Peek();
        if (next < 0)
            throw new InvalidDataException("Unexpected end of file inside a record.");

        // Якщо зустріли закриваючу дужку '}' — запис закінчився
        if (next == '}')
        {
            _reader.Read();
            SkipWhiteSpace();
            if (_reader.Peek() == ';')
                _reader.Read(); // Пропускаємо крапку з комою за наявності
            _inRecord = false;  // Запис завершено
            return false;
        }

        name = ReadQuotedString(); // Зчитуємо назву атрибута в лапках
        SkipWhiteSpace();
        if (_reader.Read() != ':')
            throw new InvalidDataException($"Expected ':' after the attribute name '{name}'.");

        SkipWhiteSpace();
        value = ReadQuotedString(); // Зчитуємо значення атрибута в лапках
        return true;
    }

    // Звільняє ресурси потоку
    public void Dispose() => _reader.Dispose();

    // Допоміжний метод: зчитує рядок, обмежений лапками, з урахуванням екранування (\n, \r)
    private string ReadQuotedString()
    {
        if (!IsQuote(_reader.Read()))
            throw new InvalidDataException("Expected a quote character.");

        var text = new StringBuilder();
        while (true)
        {
            int c = _reader.Read();
            if (c < 0)
                throw new InvalidDataException("Unexpected end of file inside a quoted string.");

            if (IsQuote(c))
                return text.ToString(); // Знайшли закриваючу лапку

            // Обробка екранованих символів (\n, \r)
            if (c == '\\')
            {
                int escaped = _reader.Read();
                if (escaped < 0)
                    throw new InvalidDataException("Unexpected end of file after '\\'.");

                char ch = (char)escaped;
                text.Append(ch == 'n' ? '\n' : ch == 'r' ? '\r' : ch);
            }
            else
            {
                text.Append((char)c);
            }
        }
    }

    // Перевіряє, чи є символ лапкою (звичайною " або типографічною “ ”)
    private static bool IsQuote(int c) => c == '"' || c == '\u201C' || c == '\u201D';

    // Шукає індекс першого пробільного символу в рядку
    private static int IndexOfWhiteSpace(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsWhiteSpace(text[i]))
                return i;
        }
        return -1;
    }

    // Пропускає пробільні символи в потоці
    private void SkipWhiteSpace()
    {
        while (_reader.Peek() >= 0 && char.IsWhiteSpace((char)_reader.Peek()))
            _reader.Read();
    }

    // Пропускає пробільні символи та коми між атрибутами
    private void SkipSeparators()
    {
        while (_reader.Peek() >= 0 && (char.IsWhiteSpace((char)_reader.Peek()) || _reader.Peek() == ','))
            _reader.Read();
    }
}