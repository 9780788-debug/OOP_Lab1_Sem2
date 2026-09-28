namespace Lab31.App.Demo;

// Інтерфейс для провайдера демонстраційних (тестових) даних
public interface IDemoDataProvider
{
    // Повертає масив записів з демонстраційними даними
    DemoRecord[] GetRecords();
}