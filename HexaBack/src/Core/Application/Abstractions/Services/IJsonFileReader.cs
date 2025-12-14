#nullable enable
namespace Application.Abstractions.Services;

public interface IJsonFileReader
{
    Task<T?> DeserializeFileAsync<T>(string filePath);
}
