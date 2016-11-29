using System.Collections.Generic;

namespace CouchbaseHealth.model
{

    public class CouchbaseBucketStat
    {

        public List<Node> nodes { get; set; }
     
    }
    public class Node
    {
        public Systemstats systemStats { get; set; }
     
    }

    public class Systemstats
    {
        public float cpu_utilization_rate { get; set; }
        public long swap_total { get; set; }
        public long swap_used { get; set; }
        public long mem_total { get; set; }
        public long mem_free { get; set; }
    }

}
