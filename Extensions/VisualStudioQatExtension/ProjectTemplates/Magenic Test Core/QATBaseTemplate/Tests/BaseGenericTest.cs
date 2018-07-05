using Magenic.Maqs.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $safeprojectname$
{
    /// <summary>
    /// BaseGenericTest test class
    /// </summary>
    [TestClass]
    public class BaseGenericTest : BaseTest
    {
        /// <summary>
        /// Sample test
        /// </summary>
        [TestMethod]
        public void SampleTest()
        {
            this.TestObject.Log.LogMessage("Start Test");
            Assert.IsTrue(true, "True is Not True");
        }
    }
}