using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.AggregateRoots;

public interface IHasExtraProperties
{
    ExtraPropertyDictionary ExtraProperties { get; }
}

[Serializable]
public class ExtraPropertyDictionary : Dictionary<string, object?>
{
    public ExtraPropertyDictionary() { }
    public ExtraPropertyDictionary(IDictionary<string, object?> dictionary) : base(dictionary) { }
    
    public T? GetProperty<T>(string key)
    {
        if (TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }
    public void SetProperty<T>(string key, T value)
    {
        this[key] = value;
    }
}

public class ExtraPropertyDictionaryConverter : ValueConverter<ExtraPropertyDictionary, string>
{
    public ExtraPropertyDictionaryConverter() : base(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
        v => JsonSerializer.Deserialize<ExtraPropertyDictionary>(v, (JsonSerializerOptions?)null) ?? new ExtraPropertyDictionary())
    { }
}