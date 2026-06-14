using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordBruteForceApp
{
    public class BruteForceMulti
    {
        public CrackResult Start(string targetHash, CancellationTokenSource cts)
        {
            PasswordValidator validator = new PasswordValidator();
            BruteForceGenerator generator = new BruteForceGenerator();

            CrackResult result = new CrackResult();

            int threadCount = Environment.ProcessorCount - 1;

            if (threadCount < 1)
            {
                threadCount = 1;
            }

            result.ThreadsUsed = threadCount;

            long checkedCounter = 0;

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = threadCount;
            options.CancellationToken = cts.Token;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                for (int length = 1; length <= 6; length++)
                {
                    int total = generator.GetCountForLength(length);

                    Parallel.For(0, total, options, (i, state) =>
                    {
                        if (cts.IsCancellationRequested)
                        {
                            state.Stop();
                            return;
                        }

                        string password = generator.GetPasswordByNumber(i, length);

                        Interlocked.Increment(ref checkedCounter);

                        if (validator.IsCorrect(password, targetHash))
                        {
                            result.FoundPassword = password;
                            cts.Cancel();
                            state.Stop();
                        }
                    });

                    if (cts.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normalu, kai paspaudžiamas Stop arba slaptažodis randamas
            }
            catch
            {
                result.Stopped = true;
            }

            stopwatch.Stop();
            result.Seconds = stopwatch.Elapsed.TotalSeconds;
            result.CheckedCount = checkedCounter;

            if (result.FoundPassword == null && cts.IsCancellationRequested)
            {
                result.Stopped = true;
            }

            return result;
        }
    }
}
