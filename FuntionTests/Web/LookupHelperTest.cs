using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.Web;
using SkillBank.Site;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class LookupHelperTests
    {
        

        //[TestMethod]
        //public void Should_GetCity_ByKey()
        //{
        //    String searchKey = "sha";
        //    Dictionary<int, CityInfo> cities = new Dictionary<int, CityInfo>();
        //    cities.Add(1,new CityInfo() { CityId = 1, CityKey = "shanghai", CityName = "上海" });
        //    cities.Add(2,new CityInfo() { CityId = 2, CityKey = "Beijing", CityName = "北京" });
        //    cities.Add(3,new CityInfo() { CityId = 3, CityKey = "zhenzhen", CityName = "深圳" });
           
        //    var cityList = LookupHelper.GetCity4Picker(cities);
        //    var result = cityList.Where(c => c.Text.Contains(searchKey)).ToList();
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.Count);
        //}

        
    }
}
