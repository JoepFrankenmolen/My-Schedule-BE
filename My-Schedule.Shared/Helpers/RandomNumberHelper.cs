using System.Security.Cryptography;

namespace My_Schedule.Shared.Helpers
{
    public static class RandomNumberHelper
    {
        public static int GenerateRandomNumbers(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be more than 0.");
            }

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                int randomInt = 0;
                for (int i = 0; i < length; i++)
                {
                    randomInt = randomInt * 10 + randomBytes[i] % 10;
                }

                // Ensure that the generated number has the specified length
                int minLength = (int)Math.Pow(10, length - 1);
                return randomInt < minLength ? randomInt + minLength : randomInt;
            }
        }
    }
}