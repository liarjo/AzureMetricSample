using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMetricConsole
{
    class Program
    {
        static void red(string txt)
        {
            ConsoleColor current = ConsoleColor.White; ;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(txt);
            Console.ForegroundColor = current;
        }

        static  void PrintMetricDefinition(string json)
        {
            var jsonData = Newtonsoft.Json.Linq.JObject.Parse(json);

            foreach (var allList in jsonData)
            {
                var list = allList.Value;

                foreach (var xxx in list)
                {
                    Console.WriteLine("> " + xxx["name"]["value"]);
                    //Console.WriteLine("> Unit " + xxx["unit"]);
                    //Console.WriteLine("> primaryAggregationType " + xxx["primaryAggregationType"]);
                    //foreach (var metricAvailabilities in xxx["metricAvailabilities"])
                    //{
                    //    Console.WriteLine("> timeGrain " + metricAvailabilities["timeGrain"]);
                    //    Console.WriteLine("> retention " + metricAvailabilities["retention"]);
                    //    Console.WriteLine("> location " + metricAvailabilities["location"]["tableEndpoint"]);
                    //    foreach (var tableInfo in metricAvailabilities["location"]["tableInfo"])
                    //    {
                    //        Console.WriteLine("> tableName " + tableInfo["tableName"]);
                    //        Console.WriteLine("> startTime " + tableInfo["startTime"]);
                    //        Console.WriteLine("> endTime " + tableInfo["endTime"]);
                    //        Console.WriteLine("> sasToken " + tableInfo["sasToken"]);
                    //        Console.WriteLine("> sasTokenExpirationTime " + tableInfo["sasTokenExpirationTime"]);
                    //    }
                    //    Console.WriteLine("> partitionKey " + metricAvailabilities["location"]["partitionKey"]);
                    //}
                    Console.WriteLine();
                }

            }
        }
        static void PrintValues(string json,string metricName)
        {
            var jsonData = Newtonsoft.Json.Linq.JObject.Parse(json);


            foreach (var allList in jsonData)
            {
                var list = allList.Value;

                foreach (var xxx in list)
                {
                    if (metricName == xxx["CounterName"].ToString())
                    {
                        Console.WriteLine("CounterName " + xxx["CounterName"].ToString());
                        Console.WriteLine("Maximum " + xxx["Maximum"].ToString());
                        Console.WriteLine("Minimum " + xxx["Minimum"].ToString());
                        Console.WriteLine("Total " + xxx["Total"].ToString());
                        Console.WriteLine("Average " + xxx["Average"].ToString());
                        Console.WriteLine("Timestamp " + xxx["Timestamp"].ToString());
                        Console.WriteLine();
                    }
                }
            }

        }
        static void Main(string[] args)
        {
            IMetricProvider myMetric = MetricProviderFactory.GetProvider(0);
            string counterName =  @"\Processor(_Total)\% Processor Time";
            //1. obtain the JWT token
            
            Console.ForegroundColor = ConsoleColor.Red;
            red("My Token is:");
            string myToken = myMetric.GetAuthorizationHeaderSilent();
            Console.WriteLine(myToken);
            Console.WriteLine("");
            
            //2. Get Metrcis List
            red("Get Netric List");
            Task<string> defTask=   myMetric.LoadMetricDefinitions(
                    "metricsampleRG",
                    "Microsoft.Compute",
                    "metricsample",
                   "");
            defTask.Wait();
            string jsonMetricDefinition = defTask.Result;
            PrintMetricDefinition(jsonMetricDefinition);

            //3. Get Metric values Table storage 
                red("Get Metrics values storage");
                TableMetricData xData = myMetric.GetMetricStorageData(jsonMetricDefinition, counterName);

            //4. Read Values
            red("Metric Values");
            Task<string> readTask = myMetric.ReadMetricValues(xData);
            readTask.Wait();

            PrintValues(readTask.Result, counterName);


            Console.ReadLine();
        }
    }
}
