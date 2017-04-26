using Magenic.MaqsFramework.BaseTest;
using NUnit.Framework;

namespace $safeprojectname$
{
    /// <summary>
    /// $safeitemname$ test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseTest
    {
        /// <summary>
        /// Sample test
        /// </summary>
        [Test]
        public void SampleTest()
        {
            this.Log.LogMessage("Is true True?");
            Assert.True(true, "true Is Not True");
        }
    }
}