using System.Diagnostics;
using System.Threading;

namespace PasswordBruteForceApp
{
    public class BruteForceSingle
    {
        public CrackResult Start(string targetHash, CancellationToken token)
        {
            PasswordValidator validator = new PasswordValidator();
            BruteForceGenerator generator = new BruteForceGenerator();

            CrackResult result = new CrackResult();
            result.ThreadsUsed = 1;

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int length = 1; length <= 6; length++)
            {
                foreach (string password in generator.Generate(length))
                {
                    if (token.IsCancellationRequested)
                    {
                        result.Stopped = true;
                        stopwatch.Stop();
                        result.Seconds = stopwatch.Elapsed.TotalSeconds;
                        return result;
                    }

                    result.CheckedCount++;

                    if (validator.IsCorrect(password, targetHash))
                    {
                        result.FoundPassword = password;
                        stopwatch.Stop();
                        result.Seconds = stopwatch.Elapsed.TotalSeconds;
                        return result;
                    }
                }
            }

            stopwatch.Stop();
            result.Seconds = stopwatch.Elapsed.TotalSeconds;
            return result;
        }
    }
}
