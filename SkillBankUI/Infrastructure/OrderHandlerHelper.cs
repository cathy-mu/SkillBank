using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillBank.Site.Web
{
    public static class OrderHandlerHelper
    {
        /// <summary>
        /// Should generate the key for handler member's order
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="handleKey">Exist key before process</param>
        /// <returns>empty string if not need handler order</returns>
        public static String GetHandleMemberOrderKey(int memberId, String handleKey)
        {
            if (memberId > 0)
            {
                String newKey = String.Format("{0}_{1}", DateTime.Now.ToString("yy-MM-dd"), memberId);

                if (String.IsNullOrEmpty(handleKey) || !handleKey.Equals(newKey))
                {
                    return newKey;
                }
            }
            return "";
        }
    }
}