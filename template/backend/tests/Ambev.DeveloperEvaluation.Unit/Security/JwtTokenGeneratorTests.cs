using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Security;

public class JwtTokenGeneratorTests
    {
        private const string SecretKey = "super-secret-key-for-tests-1234567890";

        private static IConfiguration CreateConfiguration(string? secret = SecretKey)
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "Jwt:SecretKey", secret }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private static IUser CreateTestUser() =>
            new TestUser
            {
                Id = "user-123",
                Username = "test.user",
                Role = "Admin"
            };

        [Fact(DisplayName = "Given valid user and secret, when GenerateToken called, then returns non-empty token")]
        public void Given_ValidUserAndSecret_When_GenerateToken_Then_ReturnsNonEmptyToken()
        {
            var configuration = CreateConfiguration();
            var generator = new JwtTokenGenerator(configuration);
            var user = CreateTestUser();

            var token = generator.GenerateToken(user);

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact(DisplayName = "Given generated token, when decoded, then contains expected claims")]
        public void Given_GeneratedToken_When_Decoded_Then_ContainsExpectedClaims()
        {
            var configuration = CreateConfiguration();
            var generator = new JwtTokenGenerator(configuration);
            var user = CreateTestUser();

            var token = generator.GenerateToken(user);

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);

            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false // aqui não testamos expiração ainda
            }, out var validatedToken);

            Assert.NotNull(validatedToken);

            Assert.Equal(user.Id, principal.FindFirstValue(ClaimTypes.NameIdentifier));
            Assert.Equal(user.Username, principal.FindFirstValue(ClaimTypes.Name));
            Assert.Equal(user.Role, principal.FindFirstValue(ClaimTypes.Role));
        }

        [Fact(DisplayName = "Given generated token, when inspected, then expiration is 8 hours from now (approx)")]
        public void Given_GeneratedToken_When_Inspected_Then_ExpirationIsEightHoursFromNow()
        {
            var configuration = CreateConfiguration();
            var generator = new JwtTokenGenerator(configuration);
            var user = CreateTestUser();

            var beforeGeneration = DateTime.UtcNow;
            var token = generator.GenerateToken(user);
            var afterGeneration = DateTime.UtcNow;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Esperado: entre before+8h e after+8h (pequena janela de tolerância)
            var minExpected = beforeGeneration.AddHours(8).AddMinutes(-1);
            var maxExpected = afterGeneration.AddHours(8).AddMinutes(1);

            Assert.InRange(jwtToken.ValidTo, minExpected, maxExpected);
        }

        [Fact(DisplayName = "Given missing secret key, when GenerateToken called, then throws exception")]
        public void Given_MissingSecretKey_When_GenerateToken_Then_ThrowsException()
        {
            var configuration = CreateConfiguration(secret: null);
            var generator = new JwtTokenGenerator(configuration);
            var user = CreateTestUser();

            Assert.ThrowsAny<Exception>(() => generator.GenerateToken(user));
        }

        [Fact(DisplayName = "Given token generated with valid secret, when validated with wrong secret, then validation fails")]
        public void Given_TokenWithValidSecret_When_ValidatedWithWrongSecret_Then_ValidationFails()
        {
            var configuration = CreateConfiguration();
            var generator = new JwtTokenGenerator(configuration);
            var user = CreateTestUser();

            var token = generator.GenerateToken(user);

            var handler = new JwtSecurityTokenHandler();
            var wrongKey = "another-secret-key-different"u8.ToArray();

            Assert.Throws<SecurityTokenSignatureKeyNotFoundException>(() =>
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(wrongKey),
                    ValidateLifetime = false
                }, out _);
            });
        }

        private class TestUser : IUser
        {
            public string Id { get; init; } = null!;
            public string Username { get; init; } = null!;
            public string Role { get; init; } = null!;
        }
    }

    internal static class ClaimsPrincipalExtensions
    {
        public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }