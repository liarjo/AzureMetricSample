using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMetricConsole
{
    class MetricProviderFactory
    {
        public static IMetricProvider GetProvider(int MetricType)
        {
            return new MetricProvider();
        }
    }
}
