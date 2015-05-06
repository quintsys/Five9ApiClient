using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Quintsys.Five9ApiClient
{
    public class AdminWebService
    {
        private readonly string _username;
        private readonly string _password;
        private const string BaseUrl = "https://api.five9.com/wsadmin/AdminWebService";

        public AdminWebService(string username, string password)
        {
            if (username == null) 
                throw new ArgumentNullException("username");
            if (password == null) 
                throw new ArgumentNullException("password");

            _username = username;
            _password = password;
        }

        public async Task<bool> CreateList(string listName)
        {
            if (listName == null) 
                throw new ArgumentNullException("listName");

            const string envelopeFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                          "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ser=\"http://service.admin.ws.five9.com/\">" +
                                          "<soapenv:Header/>" +
                                          "<soapenv:Body>" +
                                          "<ser:createList>" +
                                          "<listName>{0}</listName>" +
                                          "</ser:createList>" +
                                          "</soapenv:Body>" +
                                          "</soapenv:Envelope>";
            var data = string.Format(envelopeFormat, listName);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_username, _password);
                using (HttpResponseMessage response = await httpClient.PostAsync(requestUri: BaseUrl, content: new StringContent(data)))
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}