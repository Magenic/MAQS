using Magenic.MaqsFramework.BaseDatabaseTest;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace $safeprojectname$
{
    /// <summary>
    /// Simple database test class
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseDatabaseTest
    {
		/// <summary>
        /// Sample test
        /// </summary>
        [Test]
        public void SampleTest()
        {
            // TODO: Add test code
            // DataTable table = this.DatabaseWrapper.QueryAndGetDataTable("SELECT * FROM information_schema.tables");
            // Assert.AreEqual(table.Rows.Count, 10, "Expected 10 tables");
        }
    }
}