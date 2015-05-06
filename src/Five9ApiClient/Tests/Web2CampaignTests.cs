using NUnit.Framework;
using Quintsys.EnviromentConfigurationManager;

namespace Quintsys.Five9ApiClient.Tests
{
    [TestFixture]
    public class Web2CampaignTests
    {
        private Web2Campaign _web2Campaign;


        [SetUp]
        public void Setup()
        {
            _web2Campaign = new Web2Campaign("Kinetic Revenue", "TEST", "9540001111");
        }

        [Test]
        public async void AddToList_Returns_True_When_Ran_With_Proper_Parameters()
        {
            _web2Campaign.F9RetResults = true;
            _web2Campaign.F9RetUrl = ReturnUrl();
            _web2Campaign.OptionalParameters = "&number2=3050001111";

            bool success = await _web2Campaign.AddToList();

            Assert.IsTrue(success);
        }

        private static string ReturnUrl()
        {
            var config = new EnviromentConfigManager();
            string f9RetUrl = config.Get("FIVE9_RETURN_URL");
            return string.Format("{0}1/1/", f9RetUrl);
        }
    }
}