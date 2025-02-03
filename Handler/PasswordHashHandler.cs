using System;
using System.Security.Cryptography;

namespace Institute_Management.Handler
{
    public class PasswordHashHandler
    {
        private static int iterationCount = 100000; 
        private static RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();

        // Hash the password using PBKDF2
        public static string HashPassword(string password)
        {
            // Generate a new salt
            byte[] salt = new byte[32]; 
            randomNumberGenerator.GetBytes(salt);

            // Create the hashed password
            var hash = new Rfc2898DeriveBytes(password, salt, iterationCount);
            var hashedPassword = hash.GetBytes(32); 

            // Combine salt and hashed password into a single string
            var result = new byte[salt.Length + hashedPassword.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hashedPassword, 0, result, salt.Length, hashedPassword.Length);

            // Convert to Base64 string
            return Convert.ToBase64String(result);
        }

        // Verify the password with the stored hash
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Convert the stored hash from Base64 string to byte array
            var storedHashBytes = Convert.FromBase64String(storedHash);

            // Get the salt from the stored hash
            var salt = new byte[32]; // 256 bits
            Buffer.BlockCopy(storedHashBytes, 0, salt, 0, salt.Length);

            // Hash the incoming password using the same salt
            var hash = new Rfc2898DeriveBytes(password, salt, iterationCount);
            var hashedPassword = hash.GetBytes(32); // 256 bits

            // Compare the newly hashed password with the stored hashed password
            for (int i = 0; i < hashedPassword.Length; i++)
            {
                if (storedHashBytes[i + salt.Length] != hashedPassword[i])
                {
                    return false; // Passwords do not match
                }
            }

            return true; // Passwords match
        }
    }
}