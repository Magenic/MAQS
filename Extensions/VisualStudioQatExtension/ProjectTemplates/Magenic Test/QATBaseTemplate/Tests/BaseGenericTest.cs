using Magenic.Maqs.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $safeprojectname$
{
    /// <summary>
    /// $safeitemname$ test class
    /// </summary>
    [TestClass]
    public class $safeitemname$ : BaseTest
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