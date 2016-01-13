using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Utility
{
    public static class DataTagHelper
    {

        public static Boolean GetIsLike(String likes, int itemId)
        {
            String tempId = itemId.ToString();
            try
            {
                if (!String.IsNullOrEmpty(likes) && likes.Contains(Constants.Setting.DBDataDelimiter))
                {
                    String[] likeList = likes.Substring(0, likes.Length - 1).Split(Constants.Setting.DBDataDelimiter);
                    if (likeList.Contains(tempId))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static Boolean GetIsLike(String[] likes, int itemId)
        {
            if (likes!=null && likes.Length > 0)
            {
                String tempId = itemId.ToString();
                if (likes.Contains(tempId))
                {
                    return true;
                }
            }
            return false;
        }
        
        public static String[] GetLikeList(String likes)
        {
            if (!String.IsNullOrEmpty(likes) && likes.Contains(Constants.Setting.DBDataDelimiter))
            {
                String[] likeList = likes.Substring(0, likes.Length - 1).Split(Constants.Setting.DBDataDelimiter);
                return (likeList.Length > 0) ? likeList : null;
            }
            return null;
        }


    }
}
