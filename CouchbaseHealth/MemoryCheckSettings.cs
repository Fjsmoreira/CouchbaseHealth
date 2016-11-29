using Nimator;

namespace CouchbaseHealth
{
    public class MemoryCheckSettings : ICheckSettings
    {
        public MemoryCheckSettings()
        {
            Server = "127.0.0.1";
            MemoryInUsePercentageToError = 75;
            MemoryInUsePercentageToWarning = (int)(MemoryInUsePercentageToError * 0.75);
            Bucket = "Beer-Sample";
            BucketPassword = "couchbase";
        }
        public string Server { get; set; }
        public string Bucket { get; set; }
        public string BucketPassword { get; set; }
        public int MemoryInUsePercentageToError { get; set; }
        public int MemoryInUsePercentageToWarning { get; set; }
        public ICheck ToCheck()
        {
            return new MemoryCheck(this);
        }
  
    }
}
