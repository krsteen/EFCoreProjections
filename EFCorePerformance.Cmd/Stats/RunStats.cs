namespace EFCoreProjections.Cmd.Stats
{
    public class RunStats
    {
        public RunStats(string testName, string methodName, int elapsedMs, int itemCount, int sizeBytes)
        {          
            TestName = testName;
            MethodName = methodName;
            ElapsedMs = elapsedMs;
            ItemCount = itemCount;
            SizeBytes = sizeBytes;
        }
        
        public string TestName { get; set; }

        public string MethodName { get; set; }

        public int ElapsedMs { get; set; }

        public int ItemCount { get; set; }

        public int SizeBytes { get; set; }
    }
}
