namespace PasswordBruteForceApp
{
    public class PasswordValidator
    {
        private HashHelper hashHelper = new HashHelper();

        public bool IsCorrect(string password, string targetHash)
        {
            string hash = hashHelper.HashPassword(password);
            return hash == targetHash;
        }
    }
}
