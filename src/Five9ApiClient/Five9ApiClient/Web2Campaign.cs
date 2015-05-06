using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Quintsys.Five9ApiClient
{
    public interface IWeb2Campaign
    {
        bool F9RetResults { get; set; }
        string F9RetUrl { get; set; }
        string OptionalParameters { get; set; }
        Task<bool> AddToList(string f9List, string number1);
    }

    public class Web2Campaign : IWeb2Campaign
    {
        private const string BaseUrl = "https://api.five9.com/web2campaign/";
        private readonly string _f9Domain;

        public Web2Campaign(string f9Domain)
        {
            if (f9Domain == null)
                throw new ArgumentNullException("f9Domain");

            _f9Domain = f9Domain;
            F9RetResults = true;
        }

        public bool F9RetResults { get; set; }
        public string F9RetUrl { get; set; }
        public string OptionalParameters { get; set; }

        public async Task<bool> AddToList(string f9List, string number1)
        {
            if (f9List == null) 
                throw new ArgumentNullException("f9List");
            if (number1 == null) 
                throw new ArgumentNullException("number1");

            using (HttpClient httpClient = new HttpClient(new HttpClientHandler {AllowAutoRedirect = true}))
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri: AddToListUrl(f9List, number1)))
                {
                    if (string.IsNullOrWhiteSpace(F9RetUrl))
                        return response.IsSuccessStatusCode;

                    string responseUri = response.RequestMessage.RequestUri.ToString();
                    if (!responseUri.Contains(F9RetUrl))
                    {
                        // there was no redirect!
                        ReportError(string.Format("{0}?F9errCode={1}&F9errDesc={2}", F9RetUrl, response.StatusCode, response.ReasonPhrase));
                    }

                    return response.IsSuccessStatusCode;
                }
            }
        }

        private string AddToListUrl(string f9List, string number1)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(BaseUrl).Append("AddToList");
            stringBuilder.AppendFormat("?F9domain={0}", _f9Domain);
            stringBuilder.AppendFormat("&F9list={0}", f9List);
            stringBuilder.AppendFormat("&number1={0}", number1);

            stringBuilder.AppendFormat("&F9retURL={0}", F9RetUrl);
            stringBuilder.AppendFormat("&F9retResults={0}", F9RetResults);

            if (!string.IsNullOrWhiteSpace(OptionalParameters))
            {
                stringBuilder.AppendFormat("&{0}", OptionalParameters);
            }
            return stringBuilder.ToString();
        }

        private static async void ReportError(string url)
        {
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler {AllowAutoRedirect = false}))
            {
                await httpClient.GetAsync(requestUri: url);
            }
        }
    }
}