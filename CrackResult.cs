namespace PasswordBruteForceApp
{
    public class CrackResult
    {
        public string FoundPassword { get; set; }
        public double Seconds { get; set; }
        public long CheckedCount { get; set; }
        public int ThreadsUsed { get; set; }
        public bool Stopped { get; set; }
    }
}
