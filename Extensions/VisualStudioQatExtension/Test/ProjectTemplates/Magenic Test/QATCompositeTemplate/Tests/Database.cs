using Magenic.MaqsFramework.BaseDatabaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

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
            DataTable table = this.DatabaseWrapper.QueryAndGetDataTable("SELECT * FROM information_schema.tables");
            Assert.AreEqual(table.Rows.Count, 10, "Expected 10 tables");
        }
    }
}
