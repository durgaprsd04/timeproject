using System;
using System.Net.Http;
using System.Threading.Tasks;
using Timeproject.Interface;
using Timeproject.Models;
using Newtonsoft.Json;
using log4net;
namespace Timeproject
{
    public class TimeAPIHelper : ITimeAPIHelper
    {
        private HttpClient client = new HttpClient();
        private readonly string uri;
        private ILog logger;
        public string Uri => uri;

        public TimeAPIHelper(string uri, ILog logger)
        {
            this.uri = uri;
            this.logger = logger;
        }
        public override string ToString()
        {
            return uri.ToString();
        }
        public async Task<string> GetUTCTimeFromAPI()
        {
            var result = string.Empty;
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var time = await response.Content.ReadAsStringAsync();
                    var obj =  JsonConvert.DeserializeObject<WorldClockModel>(time);
                    result = obj.utc_datetime;
                }
            }
            catch(Exception e)
            {
                logger.ErrorFormat("Error occcured while fetch data from API {0} with message {1}", uri, e.Message);
            }    
            return result;
        }
        
    }
}