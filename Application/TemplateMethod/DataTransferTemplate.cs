using Application.DTO;
using Application.UI;

namespace Application.TemplateMethod;

public abstract class DataTransferTemplate
{
    public void ExportToFile(string filePath, DataTransferDto data)
    {
        if (filePath.Length == 0)
        {
            "Невалидное имя файла.".WriteLineWithColor(ConsoleColor.Red);
            return;
        }
        
        string serializedData = SerializeData(data);
        File.WriteAllText(filePath, serializedData);
        $"Экспорт завершён. Файл сохранён по пути: {filePath}".WriteLineWithColor(ConsoleColor.Green);
    }

    public DataTransferDto ImportFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            "Файл не найден.".WriteLineWithColor(ConsoleColor.Red);
            return null;
        }
        string rawData = File.ReadAllText(filePath);
        return DeserializeData(rawData);
    }

    protected abstract string SerializeData(DataTransferDto data);

    protected abstract DataTransferDto DeserializeData(string rawData);
}