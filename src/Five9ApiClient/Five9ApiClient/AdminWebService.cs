using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Quintsys.Five9ApiClient
{
    public interface IAdminWebService
    {
        Task<bool> CreateList(string listName);
    }

    public class AdminWebService : IAdminWebService
    {
        private static HttpClient Client = new HttpClient();
        private readonly string _username;
        private readonly string _password;
        private const string BaseUrl = "https://api.five9.com/wsadmin/AdminWebService";

        public AdminWebService(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("password");

            _username = username;
            _password = password;

            var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _username, _password)));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
        }

        public async Task<bool> CreateList(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName))
                throw new ArgumentNullException("listName");

            var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: BaseUrl);
            string content = CreateListContent(listName);
            request.Content = new StringContent(content: content, encoding: Encoding.UTF8, mediaType: "text/xml");

            using (var response = await Client.SendAsync(request))
            {
                return response.IsSuccessStatusCode;
            }
        }

        private static string CreateListContent(string listName)
        {
            const string envelopeFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                          "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ser=\"http://service.admin.ws.five9.com/\">" +
                                          "<soapenv:Header/>" +
                                          "<soapenv:Body>" +
                                          "<ser:createList>" +
                                          "<listName>{0}</listName>" +
                                          "</ser:createList>" +
                                          "</soapenv:Body>" +
                                          "</soapenv:Envelope>";
            return string.Format(envelopeFormat, listName);
        }
    }
}