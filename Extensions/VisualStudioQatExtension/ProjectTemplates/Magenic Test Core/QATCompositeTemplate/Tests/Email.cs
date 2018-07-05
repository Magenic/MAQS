using Magenic.Maqs.BaseEmailTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// Sample test class
    /// </summary>
    [TestClass]
    public class Email : BaseEmailTest
    {
        /// <summary>
        /// Sample test
        /// </summary>
        // [TestMethod] - Disabled because this step will fail as the template does not include access to a test email account
        public void SampleTest()
        {
            Assert.IsTrue(this.EmailWrapper.CanAccessEmailAccount(), "Could not access account");
        }
    }
}
