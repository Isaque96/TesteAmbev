using Ambev.DeveloperEvaluation.Common.Security;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Security;

public class BCryptPasswordHasherTests
{
    private readonly BCryptPasswordHasher _hasher = new();

    [Fact(DisplayName = "Given valid password, when HashPassword called, then returns non-empty hash")]
    public void Given_ValidPassword_When_HashPassword_Then_ReturnsNonEmptyHash()
    {
        const string password = "MySecurePassword123";

        var hash = _hasher.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
    }

    [Fact(DisplayName = "Given same password hashed twice, when HashPassword called, then generates different hashes")]
    public void Given_SamePasswordHashedTwice_When_HashPassword_Then_GeneratesDifferentHashes()
    {
        const string password = "MySecurePassword123";

        var hash1 = _hasher.HashPassword(password);
        var hash2 = _hasher.HashPassword(password);

        Assert.NotEqual(hash1, hash2);
    }

    [Fact(DisplayName = "Given correct password and hash, when VerifyPassword called, then returns true")]
    public void Given_CorrectPasswordAndHash_When_VerifyPassword_Then_ReturnsTrue()
    {
        const string password = "MySecurePassword123";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(password, hash);

        Assert.True(result);
    }

    [Fact(DisplayName = "Given incorrect password and hash, when VerifyPassword called, then returns false")]
    public void Given_IncorrectPasswordAndHash_When_VerifyPassword_Then_ReturnsFalse()
    {
        const string password = "MySecurePassword123";
        const string wrongPassword = "WrongPassword456";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(wrongPassword, hash);

        Assert.False(result);
    }

    [Fact(DisplayName = "Given password and known BCrypt hash, when VerifyPassword called, then returns true")]
    public void Given_PasswordAndKnownBCryptHash_When_VerifyPassword_Then_ReturnsTrue()
    {
        const string password = "test123";

        // Gera um hash real para garantir compatibilidade
        var realHash = _hasher.HashPassword(password);
        var result = _hasher.VerifyPassword(password, realHash);

        Assert.True(result);
    }

    [Fact(DisplayName = "Given empty password, when HashPassword called, then returns valid hash")]
    public void Given_EmptyPassword_When_HashPassword_Then_ReturnsValidHash()
    {
        var password = string.Empty;

        var hash = _hasher.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.True(_hasher.VerifyPassword(password, hash));
    }

    [Fact(DisplayName = "Given special characters password, when HashPassword called, then hash and verify work correctly")]
    public void Given_SpecialCharactersPassword_When_HashPassword_Then_HashAndVerifyWorkCorrectly()
    {
        const string password = "P@ssw0rd!#$%&*()";

        var hash = _hasher.HashPassword(password);
        var result = _hasher.VerifyPassword(password, hash);

        Assert.True(result);
    }

    [Fact(DisplayName = "Given very long password, when HashPassword called, then hash and verify work correctly")]
    public void Given_VeryLongPassword_When_HashPassword_Then_HashAndVerifyWorkCorrectly()
    {
        var password = new string('a', 500);

        var hash = _hasher.HashPassword(password);
        var result = _hasher.VerifyPassword(password, hash);

        Assert.True(result);
    }
}