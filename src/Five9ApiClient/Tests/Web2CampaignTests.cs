using NUnit.Framework;
using Quintsys.EnviromentConfigurationManager;

namespace Quintsys.Five9ApiClient.Tests
{
    [TestFixture]
    public class Web2CampaignTests
    {
        [SetUp]
        public void Setup()
        {
            var config = new EnviromentConfigManager();
            _domain = config.Get("FIVE9_DOMAIN");
            _f9ReturnUrl = config.Get("FIVE9_RETURN_URL");

            _web2Campaign = new Web2Campaign(_domain);
        }

        private IWeb2Campaign _web2Campaign;
        private string _domain;
        private string _f9ReturnUrl;

        [Test]
        public async void AddToList_Reports_On_Server_And_Network_Errors()
        {
            const int leadId = 1;
            const int batchId = 1;

            _web2Campaign = new Web2Campaign("invalid domain");

            var success = await _web2Campaign.AddToList(
                f9List: "this list does not exist", number1: "9540001111",
                optionalParameters: "number2=3050001111",
                f9RetUrl: string.Format("{0}{1}/{2}/", _f9ReturnUrl, leadId, batchId));

            Assert.IsTrue(success);
        }

        [Test]
        public async void AddToList_Returns_True_When_Ran_With_Invalid_Parameters()
        {
            const int leadId = 1;
            const int batchId = 1;

            _web2Campaign = new Web2Campaign("invalid domain");

            var success = await _web2Campaign.AddToList(f9List: "this list does not exist",
                number1: "9540001111",
                optionalParameters: "number2=3050001111",
                f9RetUrl: string.Format("{0}{1}/{2}/", _f9ReturnUrl, leadId, batchId));

            Assert.IsTrue(success);
        }

        [Test]
        public async void AddToList_Returns_True_When_Ran_With_Proper_Parameters()
        {
            const int leadId = 1;
            const int batchId = 1;

            var success = await _web2Campaign.AddToList(f9List: "TEST",
                number1: "9540001111",
                optionalParameters: "number2=3050001111",
                f9RetUrl: string.Format("{0}{1}/{2}/", _f9ReturnUrl, leadId, batchId));

            Assert.IsTrue(success);
        }
    }
}