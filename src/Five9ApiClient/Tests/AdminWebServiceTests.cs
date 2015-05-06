using System;
using NUnit.Framework;
using Quintsys.EnviromentConfigurationManager;

namespace Quintsys.Five9ApiClient.Tests
{
    [TestFixture]
    public class AdminWebServiceTests
    {
        private AdminWebService _adminWebService;

        [SetUp]
        public void Setup()
        {
            var config = new EnviromentConfigManager();
            string username = config.Get("FIVE9_USERNAME");
            string password = config.Get("FIVE9_PASSWORD");

            _adminWebService = new AdminWebService(username, password);
        }

        [Test]
        public void CreateList_Throws_If_List_Name_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(async () => await _adminWebService.CreateList(null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: listName"));
        }

        [Test]
        public void CreateList_Throws_If_List_Name_Is_Empty()
        {
            var ex = Assert.Throws<ArgumentNullException>(async () => await _adminWebService.CreateList(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: listName"));
        }

        [Test]
        public async void CreateList_Returns_True_When_Ran_With_Proper_Parameters()
        {
            var randomName = string.Format("DELETE_THIS_{0}", Guid.NewGuid());
            bool success = await _adminWebService.CreateList(randomName);
            Assert.IsTrue(success);
        }

    }
}