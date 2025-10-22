// CRTP (Curiously Recurring Template Pattern) is used here to:
// 1. Ensure operators (==, !=) are centralized in the base class.
// 2. Enforce type-safe behavior for derived types (e.g., OrderId == OrderId).
// This avoids duplicating operator definitions in every derived class.
// This is why TDerived is necessary.
// To create a new derived type, specify it like so ->  class OrderId : IdValue<int, OrderId>
// This will make the OrderId class automatically pick up the equality operators and stuff,
// which it wouldn't with normal inheritance.
// See: https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern

public abstract class IdValue<TValue, TDerived>
: IComparable<IdValue<TValue, TDerived>>, IEquatable<IdValue<TValue, TDerived>>
where TValue : IComparable<TValue>, IEquatable<TValue>
where TDerived : IdValue<TValue, TDerived>
{
    public TValue Value { get; }

    protected IdValue(TValue value)
    {
        Value = value;
    }

    public override bool Equals(object obj)
    {
        return obj is TDerived other && Equals(other);
    }

    public bool Equals(IdValue<TValue, TDerived> other)
    {
        if (other == null) return false;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(IdValue<TValue, TDerived> left, IdValue<TValue, TDerived> right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(IdValue<TValue, TDerived> left, IdValue<TValue, TDerived> right)
        => !(left == right);

    public int CompareTo(IdValue<TValue, TDerived> other)
    {
        if (other == null) return 1; // Nulls are considered less than any value
        if (GetType() != other.GetType()) throw new ArgumentException("Cannot compare different types");
        return Value.CompareTo(other.Value);
    }
}