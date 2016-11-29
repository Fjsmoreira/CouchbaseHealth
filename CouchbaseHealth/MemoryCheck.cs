using CouchbaseHealth.model;
using CouchbaseHealth.utils;
using Nimator;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CouchbaseHealth
{
    public class MemoryCheck : ICheck
    {
        private readonly MemoryCheckSettings _settings;
        public string ShortName => "Couchbase Available Memory in Bucket";
        public MemoryCheck(MemoryCheckSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            this._settings = settings;
            
        }

        public Task<ICheckResult> RunAsync()
        {
            var couchbaseApi = new RestHelper(
                new HttpBasicAuthenticator(_settings.Bucket,_settings.BucketPassword), 
                new Uri(_settings.Server) );

            var request = new RestRequest($"/pools/default/buckets/{_settings.Bucket}/");
            var stats =  couchbaseApi.Execute<CouchbaseBucketStat>(request);


            return AvailableMemoryCheck(stats);
        }

        private Task<ICheckResult> AvailableMemoryCheck(CouchbaseBucketStat stats)
        {
            var memFree = stats?.nodes?.FirstOrDefault()?.systemStats?.mem_free;
            var totalMem = stats?.nodes?.FirstOrDefault()?.systemStats?.mem_total;
            if (memFree == null || totalMem == null)
            {
                return
                    Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Critical,
                        "There was something wrong returning the memory information"));
            }

            var percentUsedMem = (memFree * 100.00) / totalMem;

            if (percentUsedMem > _settings.MemoryInUsePercentageToError)
            {
                return Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Error,
                    $"Memory usage excedeed {_settings.MemoryInUsePercentageToError:0.00}%. Currently at {percentUsedMem:0.00}%"));
            }

            if (percentUsedMem > _settings.MemoryInUsePercentageToWarning)
            {
                return Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Warning,
                    $"Memory usage excedeed {_settings.MemoryInUsePercentageToWarning:0.00}%. Currently at {percentUsedMem:0.00}%"));
            }

            return Task.FromResult<ICheckResult>(new CheckResult(ShortName, NotificationLevel.Okay, ""));
        }
    }
}
