using Magenic.Maqs.BaseDatabaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tests
{
    /// <summary>
    /// Sample test class
    /// </summary>
    [TestClass]
    public class $safeitemname$ : BaseDatabaseTest
    {
        /// <summary>
        /// Sample test
        /// </summary>
        // [TestMethod] - Disabled because this step will fail as the template does not include access to a test database
        public void SampleTest()
        {
            var table = this.DatabaseDriver.Query("SELECT * FROM information_schema.tables");
            Assert.AreEqual(table.Count(), 10, "Expected 10 tables");
        }
    }
}
