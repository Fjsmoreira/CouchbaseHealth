using Nimator;

namespace CouchbaseHealth
{
    public  class DocumentCheckSettings : ICheckSettings
    {

        public DocumentCheckSettings()
        {
            Server = "127.0.0.1";
            DocumentLimitToError = 100000;
            DocumentLimitToError = (int) (DocumentLimitToError * 0.75);
            Bucket = "Beer-Sample";
            BucketPassword = "couchbase";
        }

        public string Server { get; set; }
        public string Bucket { get; set; }

        public string BucketPassword { get; set; }
        public int DocumentLimitToWarning { get; set; }
        public int DocumentLimitToError { get; set; }
        public ICheck ToCheck()
        {
            
            return new DocumentCheck(this);
        }
    }
}
