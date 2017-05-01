using Magenic.MaqsFramework.BaseDatabaseTest;
using NUnit.Framework;
using System.Data;

namespace $safeprojectname$
{
    /// <summary>
    /// Sample database test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseDatabaseTest
    {
        /// <summary>
        /// Sample test
        /// </summary>
        // [Test] - Disabled because this step will fail as the template does not include access to a test database
        public void SampleTest()
        {
            DataTable table = this.DatabaseWrapper.QueryAndGetDataTable("SELECT * FROM information_schema.tables");
            Assert.AreEqual(table.Rows.Count, 10, "Expected 10 tables");
        }
    }
}
