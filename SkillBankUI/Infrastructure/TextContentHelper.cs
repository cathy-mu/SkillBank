using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Web.Context;
using System.Configuration;

namespace SkillBank.Site.Web
{
    public static class TextContentHelper
    {
        /// <summary>
        /// Convert level to level name
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static String GetLevelName(Int16 level)
        {
            String levelName = "";
            switch (level)
            {
                case 1:
                    levelName = ResourceHelper.GetTransText(119);//"Beginner Level";
                    break;
                case 2:
                    levelName = ResourceHelper.GetTransText(120);//"Intermediate Level";
                    break;
                case 3:
                    levelName = ResourceHelper.GetTransText(121);//"Advance Level";
                    break;
            }
            return levelName;
        }

        public static String GetSkillLevelName(Byte level)
        {
            String levelName = "";
            int caseNum = (level<0?0:(level-1))/20;
            switch (caseNum)
            {
                case 0:
                    levelName = ResourceHelper.GetTransText(255); 
                    break;
                case 1:
                    levelName = ResourceHelper.GetTransText(256); 
                    break;
                case 2:
                    levelName = ResourceHelper.GetTransText(257); 
                    break;
                case 3:
                    levelName = ResourceHelper.GetTransText(258);
                    break;
                default:
                    levelName = ResourceHelper.GetTransText(259);
                    break;
            }
            return levelName;
        }

        public static String GetDateString(DateTime datetime,bool hasTime=false)
        {
            if (hasTime)
            {
                return datetime.ToString(" MM/dd  -  hh:mm");//Model.Contact.CreatedDate.ToString("hh:mm MMM.dd.yyyy")
            }
            else
            {
                return datetime.ToString("yyyy.M.d");
            }
        }


        public static String[] SplitBlurbTextWithParameter(int blurbId)
        {
            var text = ResourceHelper.GetTransText(blurbId);
            if (text.IndexOf("{0}") < 0)
            {
                text += "{0}";
            }

            return text.Replace("{0}", ";").Split(';');
        }

        public static String ReplaeceBlurbParameterWithText(int blurbId, String text, String text2 = "")
        {
            var result = ResourceHelper.GetTransText(blurbId);
            if (String.IsNullOrEmpty(text2))
            {
                return result.Replace("{0}", text);
            }
            else
            {
                return result.Replace("{0}", text).Replace("{1}", text2);
            }
        }

        public static String ReplaeceBlurbParaWithHighLightText(int blurbId, String text, String text2 = "", String textClassName = "orange")
        {
            var result = ResourceHelper.GetTransText(blurbId).Replace("{0}", String.Format("<span class=\"{0}\">{1}</span>", textClassName, text));
            
            if (!String.IsNullOrEmpty(text2))
            {
                result = result.Replace("{1}", String.Format("<span class=\"{0}\">{1}</span>", textClassName, text2));
            }
            return result;
        }

        public static String ReplaeceBlurbParameterWithSubText(int blurbId, String text, int length, int blankBlurbId = 0)
        {
            if (String.IsNullOrEmpty(text) && blankBlurbId>0)
            {
                text = ResourceHelper.GetTransText(blankBlurbId);
            }
            
            if (text.Length > length)
            {
                text = text.Substring(0, length) + " ...";
            }
            var result = ResourceHelper.GetTransText(blurbId);
            return result.Replace("{0}", text).Replace("{1}", text);
        }

        public static String ReplaeceBlurbParameterWithNumber(int blurbId, int number)
        {
            var result = ResourceHelper.GetTransText(blurbId);

            return result.Replace("{0}", number.ToString());
        }

        public static String GetClassStepText(ClassEditItem classInfo, Boolean hasCity)
        {
            int step = (!hasCity || (classInfo.Category_Id.Equals(0))) ? 1 : 0;
            step += (String.IsNullOrEmpty(classInfo.Title) || String.IsNullOrEmpty(classInfo.Summary) || String.IsNullOrEmpty(classInfo.WhyU) || String.IsNullOrEmpty(classInfo.Period) || String.IsNullOrEmpty(classInfo.Location)) ? 1 : 0;
            step += String.IsNullOrEmpty(classInfo.Cover) ? 1 : 0;
            if (step == 0)
            {
                return ResourceHelper.GetTransText(486);
            }
            else
            {
                var text = ResourceHelper.GetTransText(450);// {0} step left
                return text.Replace("{0}", step.ToString());
            }
        }

        public static String GetClassStepText(ClassInfo classInfo, Boolean isMemberInfoComplete)
        {
            int step = isMemberInfoComplete?0:1;
            step += (String.IsNullOrEmpty(classInfo.Title) || String.IsNullOrEmpty(classInfo.Summary)) ? 1 : 0;
            step += String.IsNullOrEmpty(classInfo.Cover) ? 1 : 0;
            if (step == 0)
            {
                return ResourceHelper.GetTransText(486);
            }
            else
            {
                var text = ResourceHelper.GetTransText(450);// {0} step left
                return text.Replace("{0}", step.ToString());
            }
        }

        public static String GetClassOwnerDistinceTag(int myCity, int classCityId, Decimal? posX, Decimal? posY)
        {
            StringBuilder result = new StringBuilder();

            if (classCityId.Equals(myCity) && posX.HasValue && posY.HasValue && posX.Value != 0 && posY.Value != 0)
            {
                result.Append(ResourceHelper.GetTransText(222).Replace("{1}", (classCityId.Equals(0) ? "" : TagHelper.GetCityName(classCityId))).Replace("{0}", String.Format("<label class=\"classlist-distince\" data-pos=\"{0},{1}\"></label>", posX.Value, posY.Value)));
            }
            else
            {
                result.Append(classCityId.Equals(0) ? "" : TagHelper.GetCityName(classCityId));
            }

            return result.ToString();
        }

        public static String GetJoinUsDateText(DateTime sinceDate)
        {
            return ResourceHelper.GetTransText(594).Replace("{0}", sinceDate.ToString("yyyy. M"));
        }

        public static String GetSocialAccoutName(Byte socialType)
        {
            string sicalAccoutName = "";
            switch (socialType)
            {
                case 0:
                    sicalAccoutName = ResourceHelper.GetTransText(595);
                    break;
                case 3:
                    sicalAccoutName = ResourceHelper.GetTransText(596);
                    break;
                default:
                    sicalAccoutName = ResourceHelper.GetTransText(225);
                    break;
            }
            return sicalAccoutName;
        }
               

        public static String GetSocialName(Byte socialType)
        {
            string socialName = "";
            switch ((Enums.SocialTpye)socialType)
            {
                case Enums.SocialTpye.Sina:
                    socialName = ResourceHelper.GetTransText(574);
                    break;
                //case Enums.SocialTpye.Mobile:
                //    socialName = "";
                //    break;
                case Enums.SocialTpye.QQ:
                    socialName = ResourceHelper.GetTransText(576);
                    break;
                default:
                    break;
            }
            return socialName;
        }

        public static String GetTextContent(String content, int emptyTextBlurbId = 0)
        {
            if (String.IsNullOrEmpty(content))
            {
                if (emptyTextBlurbId.Equals(0))
                {
                    return "";
                }
                else
                {
                    return ResourceHelper.GetTransText(emptyTextBlurbId); 
                }
            }
            else
            {
                //replace space and enter
                return content.Replace(Char.ConvertFromUtf32(32), " ").Replace(Char.ConvertFromUtf32(10), "<br />"); //Char.ConvertFromUtf32(13)
            }
        }
                

    }
}