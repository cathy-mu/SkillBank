using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Collections.Generic;


namespace SkillBankAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private List<string> list ;//= new List<string> { "Item1", "Item2", "Item3", "Item4", "Item5" };

        public ValuesController()
        {
            list = new List<string> { "Item1", "Item2", "Item3", "Item4", "Item5" };
        }
        
        // GET api/values
        public IEnumerable<string> Get()
        {
            return list; 
        }

        // GET api/values/5
        public string GetItem(int id)
        {
            return list.Find(i => i.ToString().Contains(id.ToString()));
        }

        // POST api/values
        public List<string> Post(string value)
        {
            list.Add(value);
            return list;
        }

        // PUT api/values/5
        public void Put(int id, string value)
        {

        }

        // DELETE api/values/5
        public List<string> DeleteItem(int id)
        {
            list.Remove(list.Find((i => i.ToString().Contains(id.ToString()))));
            return list;
        }
    }
}