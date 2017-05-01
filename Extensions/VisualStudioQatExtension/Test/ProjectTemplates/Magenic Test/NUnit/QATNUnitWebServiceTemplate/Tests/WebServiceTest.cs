using Magenic.MaqsFramework.BaseWebServiceTest;
using NUnit.Framework;
using WebServiceModel;

namespace $safeprojectname$
{
    /// <summary>
    /// Simple web service test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseWebServiceTest
    {
        /// <summary>
        /// Get single product as XML
        /// </summary>
        [Test]
        public void GetXmlDeserialized()
        {
            ProductXml result = this.WebServiceWrapper.Get<ProductXml>("/api/XML_JSON/GetProduct/1", "application/xml", false);

            Assert.AreEqual(result.Id, 1, "Expected to get product 1");
        }

        /// <summary>
        /// Get single product as Json
        /// </summary>
        [Test]
        public void GetJsonDeserialized()
        {
            ProductJson result = this.WebServiceWrapper.Get<ProductJson>("/api/XML_JSON/GetProduct/1", "application/json", false);

            Assert.AreEqual(result.Id, 1, "Expected to get product 1");
        }
    }
}
