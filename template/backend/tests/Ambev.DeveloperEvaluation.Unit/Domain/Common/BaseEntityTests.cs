using Ambev.DeveloperEvaluation.Domain.Common;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Common;

public class BaseEntityTests
{
    [Fact(DisplayName = "Given two entities with same Id, when Equals called, then returns true")]
    public void Given_SameId_When_Equals_Then_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var e1 = new BaseEntity { Id = id };
        var e2 = new BaseEntity { Id = id };

        // Act
        var result = e1.Equals(e2);

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Given two entities with different Ids, when Equals called, then returns false")]
    public void Given_DifferentIds_When_Equals_Then_ReturnsFalse()
    {
        // Arrange
        var e1 = new BaseEntity { Id = Guid.NewGuid() };
        var e2 = new BaseEntity { Id = Guid.NewGuid() };

        // Act
        var result = e1.Equals(e2);

        // Assert
        Assert.False(result);
    }

    [Fact(DisplayName = "Given null object, when Equals called, then returns false")]
    public void Given_NullObject_When_Equals_Then_ReturnsFalse()
    {
        // Arrange
        var e1 = new BaseEntity { Id = Guid.NewGuid() };

        // Act
        var result = e1.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact(DisplayName = "Given two entities with same Id, when GetHashCode called, then hash codes are equal")]
    public void Given_SameId_When_GetHashCode_Then_HashCodesEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var e1 = new BaseEntity { Id = id };
        var e2 = new BaseEntity { Id = id };

        // Act
        var hash1 = e1.GetHashCode();
        var hash2 = e2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact(DisplayName = "Given entity with empty Guid, when GetHashCode called, then returns zero")]
    public void Given_EmptyGuid_When_GetHashCode_Then_ReturnsZero()
    {
        // Arrange
        var e = new BaseEntity { Id = Guid.Empty };

        // Act
        var hash = e.GetHashCode();

        // Assert
        Assert.Equal(0, hash);
    }

    [Fact(DisplayName = "Given same instance, when == operator used, then returns true")]
    public void Given_SameInstance_When_EqualityOperator_Then_ReturnsTrue()
    {
        // Arrange
        var e1 = new BaseEntity { Id = Guid.NewGuid() };
        var e2 = e1;

        // Act
        var result = e1 == e2;

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Given two entities with same Id, when == operator used, then returns true")]
    public void Given_SameId_When_EqualityOperator_Then_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var e1 = new BaseEntity { Id = id };
        var e2 = new BaseEntity { Id = id };

        // Act
        var result = e1 == e2;

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Given two entities with different Ids, when == operator used, then returns false")]
    public void Given_DifferentIds_When_EqualityOperator_Then_ReturnsFalse()
    {
        // Arrange
        var e1 = new BaseEntity { Id = Guid.NewGuid() };
        var e2 = new BaseEntity { Id = Guid.NewGuid() };

        // Act
        var result = e1 == e2;

        // Assert
        Assert.False(result);
    }

    [Fact(DisplayName = "Given one null and one not null, when == operator used, then returns false")]
    public void Given_OneNull_When_EqualityOperator_Then_ReturnsFalse()
    {
        // Arrange
        BaseEntity e1 = new() { Id = Guid.NewGuid() };
        BaseEntity? e2 = null;

        // Act
        var result1 = e1 == e2;
        var result2 = e2 == e1;

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact(DisplayName = "Given both null, when == operator used, then returns true")]
    public void Given_BothNull_When_EqualityOperator_Then_ReturnsTrue()
    {
        // Arrange
        BaseEntity? e1 = null;
        BaseEntity? e2 = null;

        // Act
        var result = e1 == e2;

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Given two entities with different Ids, when != operator used, then returns true")]
    public void Given_DifferentIds_When_InequalityOperator_Then_ReturnsTrue()
    {
        // Arrange
        var e1 = new BaseEntity { Id = Guid.NewGuid() };
        var e2 = new BaseEntity { Id = Guid.NewGuid() };

        // Act
        var result = e1 != e2;

        // Assert
        Assert.True(result);
    }

    [Fact(DisplayName = "Given two entities with same Id, when != operator used, then returns false")]
    public void Given_SameId_When_InequalityOperator_Then_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var e1 = new BaseEntity { Id = id };
        var e2 = new BaseEntity { Id = id };

        // Act
        var result = e1 != e2;

        // Assert
        Assert.False(result);
    }

    [Fact(DisplayName = "Given other is null, when CompareTo called, then returns 1")]
    public void Given_OtherNull_When_CompareTo_Then_ReturnsOne()
    {
        // Arrange
        var e1 = new BaseEntity { Id = Guid.NewGuid() };

        // Act
        var result = e1.CompareTo(null);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact(DisplayName = "Given two entities, when CompareTo called, then compares by Id")]
    public void Given_TwoEntities_When_CompareTo_Then_ComparesById()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var e1 = new BaseEntity { Id = id1 };
        var e2 = new BaseEntity { Id = id2 };

        // Act
        var result1 = e1.CompareTo(e2);
        var result2 = e2.CompareTo(e1);
        var result3 = e1.CompareTo(new BaseEntity { Id = id1 });

        // Assert
        Assert.Equal(-result1, result2);
        Assert.Equal(0, result3);
    }

    [Fact(DisplayName = "Given left and right not null, when < and > operators used, then they follow CompareTo")]
    public void Given_Entities_When_LessThanAndGreaterThanOperators_Then_FollowCompareTo()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        // Garante ordem estável nos testes
        var smallerId = id1.CompareTo(id2) < 0 ? id1 : id2;
        var greaterId = id1.CompareTo(id2) < 0 ? id2 : id1;

        var smaller = new BaseEntity { Id = smallerId };
        var greater = new BaseEntity { Id = greaterId };

        // Act + Assert
        Assert.True(smaller < greater);
        Assert.False(greater < smaller);

        Assert.True(greater > smaller);
        Assert.False(smaller > greater);
    }

    [Fact(DisplayName = "Given entities, when <= and >= operators used, then follow CompareTo including equality")]
    public void Given_Entities_When_LessThanOrEqualAndGreaterThanOrEqualOperators_Then_RespectEquality()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var e1 = new BaseEntity { Id = id1 };
        var e2 = new BaseEntity { Id = id2 };
        var e1Copy = new BaseEntity { Id = id1 };

        // Act + Assert
        if (id1.CompareTo(id2) < 0)
        {
            Assert.True(e1 <= e2);
            Assert.False(e2 <= e1);

            Assert.True(e2 >= e1);
            Assert.False(e1 >= e2);
        }
        else if (id1.CompareTo(id2) > 0)
        {
            Assert.True(e2 <= e1);
            Assert.False(e1 <= e2);

            Assert.True(e1 >= e2);
            Assert.False(e2 >= e1);
        }

        // <= e >= com entidades equivalentes (mesmo Id)
        Assert.True(e1 <= e1Copy);
        Assert.True(e1Copy <= e1);
        Assert.True(e1 >= e1Copy);
        Assert.True(e1Copy >= e1);
    }

    [Fact(DisplayName = "Given null left entity, when <= and >= used, then behave according to implementation")]
    public void Given_NullLeft_When_LessThanOrEqualAndGreaterThanOrEqual_Then_BehaveAsImplemented()
    {
        // Arrange
        BaseEntity? left = null;
        var right = new BaseEntity { Id = Guid.NewGuid() };

        // Act + Assert
        Assert.True(left <= right);

        Assert.False(left >= right);

        // null vs null
        BaseEntity? nullLeft = null;
        BaseEntity? nullRight = null;

        Assert.True(nullLeft <= nullRight); // left is null => returns true (right is null)
        Assert.True(nullLeft >= nullRight); // left is null and right is null => true
    }
}