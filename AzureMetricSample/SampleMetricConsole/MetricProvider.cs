using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SampleMetricConsole
{
    class MetricProvider:IMetricProvider
    {
        public string TenantId { get; private set; }
        public string ClientId { get; private set; }
        public string UserName { get; private set; }
        public string SubscriptionId { get; private set; }
        private string Password;
        private string jToken;
        private string ListMetrcis;
        private string apiVersion;
        
        public  MetricProvider()
        {
            this.TenantId = ConfigurationManager.AppSettings["tenantId"];
            this.ClientId = ConfigurationManager.AppSettings["clientId"];
            this.UserName = ConfigurationManager.AppSettings["userName"];
            this.SubscriptionId = ConfigurationManager.AppSettings["SubscriptionId"];
            this.Password = ConfigurationManager.AppSettings["password"];
            this.ListMetrcis = "https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/{2}/virtualMachines/{3}/metricDefinitions?api-version={4}&$filter={5}";
                             ///https://management.azure.com/subscriptions/bcb59800-e2f5-42cf-bc58-26248a25bb5a/resourceGroups/metricsampleRG/providers/Microsoft.Compute/virtualMachines/metricsample?api-version=2015-05-01-preview
            this.apiVersion = "2014-04-01";

        }
        public string GetAuthorizationHeaderSilent()
        {
            AuthenticationResult result = null;
            var context = new AuthenticationContext("https://login.windows.net/" + TenantId);
            // Directly specify the username and password. 
            var credential = 
                new Microsoft.IdentityModel.Clients.ActiveDirectory.UserCredential(
                    this.UserName,
                    this.Password);
            
            result = context.AcquireToken(
                "https://management.core.windows.net/",
                this.ClientId,
                credential);
            
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }
            jToken=result.AccessToken;
            return jToken;
        }

        public  async Task<string>  LoadMetricDefinitions(string ResourceGroup, string Provider, string VmName,string Filter)
        {
            string url=String.Format(ListMetrcis, SubscriptionId, ResourceGroup, Provider, VmName, apiVersion, Filter);
            string MetricListResponse = null;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jToken);
            string stringResponse = null;
            try
            {
                 MetricListResponse = await
                    client.GetStringAsync(
                        String.Format(ListMetrcis, SubscriptionId, ResourceGroup, Provider, VmName, apiVersion, Filter));
                 stringResponse = MetricListResponse.ToString();


                
               




            }
            catch (Exception Err)
            {

                Console.WriteLine(Err.Message);
            }

            return stringResponse;
        }

        public TableMetricData GetMetricStorageData(string jsonMetricDefinition,string MetricName)
        {
            TableMetricData myData = null;

            try
            {
                var jsonData = Newtonsoft.Json.Linq.JObject.Parse(jsonMetricDefinition);
                foreach (var allList in jsonData)
                 {
                     var list = allList.Value;
   
                     foreach (var xxx in list)
                     {
                         if (xxx["name"]["value"].ToString() == MetricName)
                         {
                             myData = new TableMetricData();
                             foreach (var metricAvailabilities in xxx["metricAvailabilities"])
                             {
                                 myData.tableEndpoint=metricAvailabilities["location"]["tableEndpoint"].ToString();
                                 foreach (var tableInfo in metricAvailabilities["location"]["tableInfo"])
                                 {
                                     myData.TableName = tableInfo["tableName"].ToString();
                                     myData.sasToken=tableInfo["sasToken"].ToString();
                                 }
                                 myData.partitionKey = metricAvailabilities["location"]["partitionKey"].ToString();
                             }
                             break;
                         }

                     }

                 }
            }
            catch (Exception Err)
            {

                Console.WriteLine(Err.Message);
            }
            return myData;
        }

        public async Task<string> ReadMetricValues(TableMetricData queryData)
        {
            string values = null;
            //3. Table Data
            try
            {
                 string readMetric = string.Format(
                    "{3}{0}{1}&api-version=2014-02-14&$filter=(PartitionKey eq '{2}')",
                    queryData.TableName,
                    queryData.sasToken,
                    queryData.partitionKey,
                    queryData.tableEndpoint
                    );

                var client = new HttpClient();
                //json header
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var metricValues = await
                    client.GetStringAsync(readMetric);
                values = metricValues.ToString();
            }
            catch (Exception X)
            {

                Console.WriteLine(X.Message);
            }
            return values;
        }
    }

    class TableMetricData
    {
        public string TableName { get; set; }
        public string sasToken { get; set; }
        public string partitionKey { get; set; }
        public string tableEndpoint { get; set; }

    }

}
