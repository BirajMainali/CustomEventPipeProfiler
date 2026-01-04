namespace CustomEventPipeProfiler.Extensions
{
    public static class DictionaryPayloadExtensions
    {
        public static bool IsCpuUsage(this IDictionary<string, object> payload)
            => string.Equals(payload["Name"].ToString(), "cpu-usage", StringComparison.OrdinalIgnoreCase);

        public static double GetMean(this IDictionary<string, object> payload)
            => Convert.ToDouble(payload["Mean"]);
    }
}
