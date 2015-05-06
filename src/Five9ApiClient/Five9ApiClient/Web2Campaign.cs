using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Quintsys.Five9ApiClient
{
    public class Web2Campaign
    {
        private readonly string _f9Domain;
        private readonly string _f9List;
        private readonly string _number1;
        private const string BaseUrl = "https://api.five9.com/web2campaign/";

        public Web2Campaign(string f9Domain, string f9List, string number1)
        {
            if (string.IsNullOrWhiteSpace(f9Domain))
                throw new ArgumentNullException("f9Domain");
            if (string.IsNullOrWhiteSpace(f9List))
                throw new ArgumentNullException("f9List");
            if (string.IsNullOrWhiteSpace(number1))
                throw new ArgumentNullException("number1");

            _f9Domain = f9Domain;
            _f9List = f9List;
            _number1 = number1;

            F9RetResults = true;
        }

        public bool F9RetResults { get; set; }
        public string F9RetUrl { get; set; }
        public string OptionalParameters { get; set; }

        public async Task<bool> AddToList()
        {
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler {AllowAutoRedirect = true}))
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri: AddToListUrl()))
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

        private string AddToListUrl()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(BaseUrl).Append("AddToList");
            stringBuilder.AppendFormat("?F9domain={0}", _f9Domain);
            stringBuilder.AppendFormat("&F9list={0}", _f9List);
            stringBuilder.AppendFormat("&number1={0}", _number1);

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