using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using SkillBank.Site.Services;
using SkillBank.Site.Web;


namespace SkillBankWeb.Controllers.API
{
    public class CityController : ApiController
    {
        public readonly IContentService _contentService;


        public CityController( IContentService contentService)
        {
            _contentService = contentService;
        }

        // POST api/city
        public int Post(String cityName)
        {
            var cityDic = _contentService.GetCities("cn");
            int cityId = LookupHelper.GetCityIdByName(cityDic, cityName);
            return cityId;
        }

        //// PUT api/city/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}
    }

}
