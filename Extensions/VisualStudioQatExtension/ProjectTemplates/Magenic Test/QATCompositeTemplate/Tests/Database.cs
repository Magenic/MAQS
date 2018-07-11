using Magenic.Maqs.BaseDatabaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace $safeprojectname$
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
            var tables = this.DatabaseWrapper.Query("SELECT * FROM information_schema.tables").ToList();
            Assert.AreEqual(10, tables.Count, "Expected 10 tables");
        }
    }
}
