using My_Schedule.AuthService.DTO.Auth;
using My_Schedule.Shared.Interfaces.AppSettings;
using System.Security.Cryptography;
using System.Text;
using BCryptClass = BCrypt.Net.BCrypt;

namespace My_Schedule.AuthService.Services.Auth
{
    public class HashService
    {
        private readonly IUserSettings _appSettings;

        public HashService(IUserSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// Generates a salt and a hashed password using BCrypt and Sha.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A HashDTO containing the hashed password and salt.</returns>
        public async Task<HashDTO> GenerateSaltAndHash(string password)
        {
            // Generate a salt
            string salt = BCryptClass.GenerateSalt();

            // Generate the hashed password
            string hash = await GenerateHash(password, salt);

            // Return the hashed password and salt
            return new HashDTO { PasswordHash = hash, Salt = salt };
        }

        /// <summary>
        /// Generates a double-hashed password with added security.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        /// <returns>The double-hashed password.</returns>
        public async Task<string> GenerateHash(string password, string salt)
        {
            // Prepare additional salt
            string saltTwo = password + _appSettings.Pepper;

            // Combine salt, password, and pepper
            string stringToHash = salt + password + _appSettings.Pepper;

            // First hash
            string hashedPassword = await GenerateHashSha(stringToHash);

            // Combine the hash with the second salt
            string stringToHashTwo = hashedPassword + saltTwo;

            // Second hash using BCrypt
            string hashedPasswordTwo = await GenerateHashBcrypt(stringToHashTwo);

            return hashedPasswordTwo;
        }

        /// <summary>
        /// Generates a hash using BCrypt.
        /// </summary>
        /// <param name="stringToHash">The string to hash.</param>
        /// <returns>The BCrypt hash of the input string.</returns>
        public async Task<string> GenerateHashBcrypt(string stringToHash)
        {
            // Hash the string using BCrypt
            return BCryptClass.HashPassword(stringToHash, "$2b$10$12jQodrEnt0B4KUxH89Pce");
        }

        /// <summary>
        /// Generates a hash using SHA-256.
        /// </summary>
        /// <param name="stringToHash">The string to hash.</param>
        /// <returns>The SHA-256 hash of the input string.</returns>
        public async Task<string> GenerateHashSha(string stringToHash)
        {
            // Prepare the bytes for hashing
            byte[] bytesToHash = Encoding.UTF8.GetBytes(stringToHash);

            // Hash the bytes using SHA-256
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedByte = sha256.ComputeHash(bytesToHash);
                return Convert.ToBase64String(hashedByte);
            }
        }
    }
}