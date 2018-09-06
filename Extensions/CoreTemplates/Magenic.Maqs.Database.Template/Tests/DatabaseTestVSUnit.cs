using Magenic.Maqs.BaseDatabaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;

namespace Tests
{
    /// <summary>
    /// Tests test class with VSUnit
    /// </summary>
    [TestClass]
    public class DatabaseTestVSUnit : BaseDatabaseTest
    {
        /// <summary>
        /// Get record using stored procedure
        /// </summary>
        //[TestMethod]  Disabled because this step will fail as the template does not include access to a test database
        public void GetRecordTest()
        {
            var result = this.DatabaseDriver.Query("getStateAbbrevMatch", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);
            Assert.AreEqual(1, result.Count(), "Expected 1 state abbreviation to be returned.");
        }
    }
}