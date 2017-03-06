using Magenic.MaqsFramework.BaseEmailTest;
using NUnit.Framework;

namespace $safeprojectname$
{
    /// <summary>
    /// Sample email test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseEmailTest
    {
        /// <summary>
        /// Sample test
        /// </summary>
        // [Test] - Disabled because this step will fail as the template does not include access to a test email account
        public void SampleTest()
        {
            Assert.IsTrue(this.EmailWrapper.CanAccessEmailAccount(), "Could not access account");
        }
    }
}
