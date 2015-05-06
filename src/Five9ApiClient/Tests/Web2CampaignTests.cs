using NUnit.Framework;

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
            _web2Campaign.F9RetUrl = "http://ogburnonline.com/five9/1/1/"; // => ToDo: create this url
            _web2Campaign.OptionalParameters = "&number2=3050001111";
            bool success = await _web2Campaign.AddToList();
            Assert.IsTrue(success);
        }
    }
}