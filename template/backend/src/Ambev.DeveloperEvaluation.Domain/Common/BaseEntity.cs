using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// The Unique Identifier
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets the date and time when the category was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null) return 1;
        return Id.CompareTo(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseEntity be && Id.Equals(be.Id);
    }

    private int Hash()
    {
        return Id == Guid.Empty ? 0 : Id.GetHashCode();
    }
    
    public override int GetHashCode()
    {
        return Hash();
    }
    
    public static bool operator ==(BaseEntity? left, BaseEntity? right) =>
        ReferenceEquals(left, right) || (left is not null && right is not null && left.Id == right.Id);

    public static bool operator !=(BaseEntity? left, BaseEntity? right) => !(left == right);

    public static bool operator <(BaseEntity? left, BaseEntity? right) =>
        left is not null && right is not null && left.CompareTo(right) < 0;

    public static bool operator <=(BaseEntity? left, BaseEntity? right) =>
        left is null || (right is not null && left.CompareTo(right) <= 0);

    public static bool operator >(BaseEntity? left, BaseEntity? right) =>
        left is not null && right is not null && left.CompareTo(right) > 0;

    public static bool operator >=(BaseEntity? left, BaseEntity? right) =>
        left is null ? right is null : right is not null && left.CompareTo(right) >= 0;
}
