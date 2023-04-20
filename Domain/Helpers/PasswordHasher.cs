using System.Security.Cryptography;

namespace Domain.Helpers;

internal static class PasswordHasher
{
    private const int _saltSize = 16;
    private const int _keySize = 32;
    private const int _iterations = 100000;
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;

    private const char segmentDelimiter = ':';

    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            _iterations,
            _algorithm,
            _keySize
        );

        return string.Join(
            segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            _iterations,
            _algorithm
        );
    }

    public static bool Verify(string password, string hashString)
    {
        string[] segments = hashString.Split(segmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
}
