using System.Security.Cryptography;
using System.Text;

namespace PasswordBruteForceApp
{
    public class HashHelper
    {
        private const string StaticSalt = "ManoPastovusSalt123";

        public string HashPassword(string password)
        {
            string text = password + StaticSalt;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }
    }
}
