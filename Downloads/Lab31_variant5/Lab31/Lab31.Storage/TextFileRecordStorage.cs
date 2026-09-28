using System;
using System.IO;

namespace Lab31.Storage;

// Реалізація сховища даних на базі єдиного текстового файлу
public sealed class TextFileRecordStorage : IRecordStorage
{
    private readonly string _path;     // Основний шлях до файлу сховища
    private readonly string _tempPath; // Тимчасовий шлях для безпечного перезапису файлу

    // Конструктор: приймає шлях до файлу та ініціалізує шлях для тимчасового файлу (.tmp)
    public TextFileRecordStorage(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("File path must not be empty.", nameof(path));

        _path = path;
        _tempPath = path + ".tmp";
    }

    // Перевіряє, чи існує основний файл сховища на диску
    public bool Exists => File.Exists(_path);

    // Відкриває файл для зчитання записів
    public IRecordReader OpenReader() =>
        new TextRecordReader(new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read));

    // Відкриває файл для дописування нових записів у кінець
    public IRecordWriter OpenAppender()
    {
        EnsureDirectory(); // Гарантуємо наявність директорії
        return new TextRecordWriter(new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.None));
    }

    // Починає процес перезапису: створює новий тимчасовий файл (.tmp)
    public IRecordWriter BeginRewrite()
    {
        EnsureDirectory(); // Гарантуємо наявність директорії
        return new TextRecordWriter(new FileStream(_tempPath, FileMode.Create, FileAccess.Write, FileShare.None));
    }

    // Завершує процес перезапису: замінює основний файл тимчасовим
    public void CommitRewrite() => File.Move(_tempPath, _path, overwrite: true);

    // Допоміжний метод: створює папку для файлу, якщо вона ще не існує
    private void EnsureDirectory()
    {
        string? directory = Path.GetDirectoryName(Path.GetFullPath(_path));
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
    }
}