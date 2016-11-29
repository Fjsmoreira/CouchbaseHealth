using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Linq;
using Nimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouchbaseHealth
{
    public class DocumentCheck : ICheck
    {

        private readonly DocumentCheckSettings _settings;
        public string ShortName => $"Couchbase Total Documents in a bucket";
        public DocumentCheck(DocumentCheckSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            _settings = settings;

            InitializeCluster();

        }

        private void InitializeCluster()
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri> {new Uri(_settings.Server)},
            });
        }

        public Task<ICheckResult> RunAsync()
        {

            var bucket = string.IsNullOrWhiteSpace(_settings.BucketPassword)
                ? ClusterHelper.GetBucket(_settings.Bucket)
                : ClusterHelper.GetBucket(_settings.Bucket, _settings.BucketPassword);


            var context = new BucketContext(bucket);

            var totalDocumentsInBucket = (from document in context.Query<dynamic>() select document).Count();

            return TotalDocumentCheck(totalDocumentsInBucket);
        }

        private Task<ICheckResult> TotalDocumentCheck(int totalDocumentsInBucket)
        {
            if (totalDocumentsInBucket > _settings.DocumentLimitToError)
            {
                return
                    Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Error, $"Document Size Exceeded {_settings.DocumentLimitToError}. Current document size: {totalDocumentsInBucket}"));
            }

            if (totalDocumentsInBucket > _settings.DocumentLimitToWarning)
            {
                return Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Warning, $"Document Size Exceeded {_settings.DocumentLimitToError}. Current document size: {totalDocumentsInBucket}"));
            }

            return Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Okay,""));
        }

    }
}
