namespace Lab31.Storage;

// Інтерфейс абстракції над фізичним сховищем даних (файлом)
public interface IRecordStorage
{
    // Перевіряє, чи існує файл сховища на диску
    bool Exists { get; }
    // Відкриває потік для послідовного зчитування даних із файлу
    IRecordReader OpenReader();
    // Відкриває потік для дописування нових записів у кінець існуючого файлу
    IRecordWriter OpenAppender();
    // Починає процес повного перезапису файлу (записує дані у тимчасовий об'єкт)
    IRecordWriter BeginRewrite();
    // Фіксує перезапис: замінює оригінальний файл новим після виклику BeginRewrite
    void CommitRewrite();
}