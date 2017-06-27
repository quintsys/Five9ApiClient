using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Quintsys.Five9ApiClient
{
    public interface IWeb2Campaign
    {
        Task<bool> AddToList(string f9List, string number1, string optionalParameters, string f9RetUrl, bool f9RetResults = true);
    }

    public class Web2Campaign : IWeb2Campaign
    {
        private const string BaseUrl = "https://api.five9.com/web2campaign/";
        private readonly string _f9Domain;
        private static HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });

        public Web2Campaign(string f9Domain)
        {
            if (f9Domain == null)
                throw new ArgumentNullException("f9Domain");

            _f9Domain = f9Domain;
        }

        public async Task<bool> AddToList(string f9List, string number1, string optionalParameters, string f9RetUrl, bool f9RetResults = true)
        {
            if (f9List == null)
                throw new ArgumentNullException("f9List");
            if (number1 == null)
                throw new ArgumentNullException("number1");

            var requestUri = AddToListUrl(f9List, number1, optionalParameters, f9RetUrl, f9RetResults);
            using (HttpResponseMessage response = await Client.GetAsync(requestUri: requestUri))
            {
                if (string.IsNullOrWhiteSpace(f9RetUrl))
                    return response.IsSuccessStatusCode;

                string responseUri = response.RequestMessage.RequestUri.ToString();
                if (responseUri.Contains(BaseUrl))
                {
                    // there was no redirect!
                    var errorReportUrl = string.Format("{0}?F9errCode={1}&F9errDesc={2}", f9RetUrl, (int)response.StatusCode, response.ReasonPhrase);
                    await Client.GetAsync(requestUri: errorReportUrl);
                }

                return response.IsSuccessStatusCode;
            }
        }

        private string AddToListUrl(string f9List, string number1, string optionalParameters, string f9RetUrl, bool f9RetResults)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(BaseUrl).Append("AddToList");
            stringBuilder.AppendFormat("?F9domain={0}", _f9Domain);
            stringBuilder.AppendFormat("&F9list={0}", f9List);
            stringBuilder.AppendFormat("&number1={0}", number1);

            stringBuilder.AppendFormat("&F9retURL={0}", f9RetUrl);
            stringBuilder.AppendFormat("&F9retResults={0}", f9RetResults);

            if (!string.IsNullOrWhiteSpace(optionalParameters))
            {
                stringBuilder.AppendFormat("&{0}", optionalParameters);
            }
            return stringBuilder.ToString();
        }
    }
}