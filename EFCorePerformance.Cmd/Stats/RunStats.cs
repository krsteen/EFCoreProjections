namespace EFCorePerformance.Cmd.Stats
{
    public class RunStats
    {
        public RunStats(int serviceIndex, int testIndex, string testName, string methodName, int elapsedMs, int itemCount, int sizeBytes)
        {
            ServiceIndex = serviceIndex;
            TestIndex = testIndex;
            TestName = testName;
            MethodName = methodName;
            ElapsedMs = elapsedMs;
            ItemCount = itemCount;
            SizeBytes = sizeBytes;
        }

        public int ServiceIndex { get; set; }

        public int TestIndex { get; set; }

        public string TestName { get; set; }

        public string MethodName { get; set; }

        public int ElapsedMs { get; set; }

        public int ItemCount { get; set; }

        public int SizeBytes { get; set; }
    }
}
