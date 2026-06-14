using System;

namespace PasswordBruteForceApp
{
    public class PasswordCreator
    {
        private string chars = "abc123";
        private Random random = new Random();

        public string CreatePassword()
        {
            // [4-6) reiškia 4 arba 5 simboliai
            int length = random.Next(4, 6);

            string password = "";

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                password += chars[index];
            }

            return password;
        }
    }
}
