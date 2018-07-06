using Magenic.Maqs.BaseDatabaseTest;
using NUnit.Framework;
using System.Data;
using System.Linq;

namespace $safeprojectname$
{
    /// <summary>
    /// $safeprojectname$ test class with NUnit
    /// </summary>
    [TestFixture]
    public class $safeitemname$ : BaseDatabaseTest
    {
        /// <summary>
        /// Get record using stored procedure
        /// </summary>
        //[Test]  Disabled because this step will fail as the template does not include access to a test database
        public void GetRecordTestNUnit()
        {
            var result = this.DatabaseWrapper.Query("getStateAbbrevMatch", new { StateAbbreviation = "MN" }, commandType: CommandType.StoredProcedure);
            Assert.AreEqual(1, result.Count(), "Expected 1 state abbreviation to be returned.");
        }
    }
}