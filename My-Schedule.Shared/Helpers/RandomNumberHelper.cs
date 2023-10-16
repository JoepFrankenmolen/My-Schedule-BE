using System.Security.Cryptography;

namespace My_Schedule.Shared.Helpers
{
    public static class RandomNumberHelper
    {
        public static int GenerateRandomNumbers(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be a positive integer.", nameof(length));
            }

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                int secureInt = 0;
                for (int i = 0; i < length; i++)
                {
                    secureInt = secureInt * 256 + randomBytes[i];
                }

                // Ensure the generated number is positive
                secureInt = Math.Abs(secureInt);

                // Trim the number to the desired length
                secureInt %= (int)Math.Pow(10, length);

                return secureInt;
            }
        }
    }
}