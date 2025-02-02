using System.Reflection;

namespace keycontrol.Domain.Enums;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    public int Id { get; protected init; }
    public string Name { get; protected init; }
    private static readonly Dictionary<int, TEnum> _Enumerations = CreateEnumerations();


    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static TEnum? FromValue(int value)
    {
        return _Enumerations.TryGetValue(value, out TEnum enumeration) ? enumeration : default;
    }

    public static TEnum? FromName(string name)
    {
        return _Enumerations.Values.SingleOrDefault(enumeration => enumeration.Name == name);
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);
        var fieldsForType = enumerationType.GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
            
        return fieldsForType.ToDictionary(x => x.Id);
    }
    public static IReadOnlyCollection<TEnum> GetValues() => _Enumerations.Values.ToList();
}