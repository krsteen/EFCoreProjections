namespace EFCorePerformance.Cmd.Stats
{
    public class RunStats
    {
        public RunStats(int serviceIndex, int testIndex, string serviceName, string methodName, int elapsedMs, int sizeBytes)
        {
            ServiceIndex = serviceIndex;
            TestIndex = testIndex;
            ServiceName = serviceName;
            MethodName = methodName;
            ElapsedMs = elapsedMs;
            SizeBytes = sizeBytes;
        }

        public int ServiceIndex { get; set; }

        public int TestIndex { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public int ElapsedMs { get; set; }

        public int SizeBytes { get; set; }
    }
}
