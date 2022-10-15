using HL.Loyalty.Models;
using HL.Loyalty.RulesEngine.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HL.Loyalty.RulesEngine.Tests.Providers
{
    [TestClass()]
    public class TierConfigurationProviderTest
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTiers_Null_connString()
        {
            var provider2 = new TierConfigurationProvider(null);
        }

        //[TestMethod()]
        //[TestCategory("IntegrationTest")]
        //public void GetTiers_Valid_connString()
        //{
        //    var provider2 = new TierConfigurationProvider("data source=crm-usw-dev-sql.database.windows.net;initial catalog=Loyalty;user id=sqlguy;password=Herb1234;MultipleActiveResultSets=True;");
        //    var products = provider2.Load(PointsCategoryType.Product, "US");
        //    Assert.IsTrue(products.Count > 1);
        //}

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTiers_Null_Country_Exception()
        {
            var provider2 = new TierConfigurationProvider("connString");
            var products = provider2.Load(PointsCategoryType.Product, null);
        }
    }
}
