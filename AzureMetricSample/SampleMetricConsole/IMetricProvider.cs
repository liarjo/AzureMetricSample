using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMetricConsole
{
    interface IMetricProvider
    {
        string GetAuthorizationHeaderSilent();
        Task<string> LoadMetricDefinitions(string ResourceGroup, string Provider, string VmName, string Filter);
        TableMetricData GetMetricStorageData(string jsonMetricDefinition, string MetricName);
        Task<string> ReadMetricValues(TableMetricData queryData);
        
    }
}
