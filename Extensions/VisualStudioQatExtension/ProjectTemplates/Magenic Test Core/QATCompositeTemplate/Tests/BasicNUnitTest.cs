using Magenic.Maqs.BaseTest;
using NUnit.Framework;

namespace $safeprojectname$
{
    /// <summary>
    /// $safeitemname$ test class with NUnit
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
            this.TestObject.Log.LogMessage("Start Test");
            Assert.IsTrue(true, "True is Not True");
        }
    }
}