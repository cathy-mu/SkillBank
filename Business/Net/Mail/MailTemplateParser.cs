//#region

//using System.Collections.Generic;
//using System.Globalization;
//using System.Text.RegularExpressions;
//using EFSchools.Englishtown.ContentConfigurationET;
//using EFSchools.Englishtown.ContentManagementET;

//#endregion

//namespace EFSchools.EnglishFirst.SalesPages.Net.Mail
//{
//    /// <summary>
//    ///   MailTemplateParser
//    /// </summary>
//    internal class MailTemplateParser : StringParser
//    {
//        /// <summary>
//        /// </summary>
//        /// <param name = "context"></param>
//        /// <param name = "cultureInfo"></param>
//        public MailTemplateParser(IETContext context, CultureInfo cultureInfo)
//            : this(context, cultureInfo, null)
//        {
//        }

//        /// <summary>
//        /// </summary>
//        /// <param name = "context"></param>
//        /// <param name = "cultureInfo"></param>
//        /// <param name = "userValues"></param>
//        public MailTemplateParser(IETContext context, CultureInfo cultureInfo, Dictionary<string, string> userValues)
//            : base(context, cultureInfo)
//        {
//            if (userValues == null)
//            {
//                userValues = new Dictionary<string, string>();
//            }
//            UserValues = userValues;
//            ContextDelegate = GetEmailContext;
//        }

//        /// <summary>
//        /// </summary>
//        public Dictionary<string, string> UserValues { get; protected set; }

//        private string GetEmailContext(Match match)
//        {
//            string value;
//            string var = match.Groups["var"].Value.Trim().ToLower();

//            if (!UserValues.TryGetValue(var, out value))
//            {
//                value = string.Empty;
//            }
//            return value ?? string.Empty;
//        }
//    }
//}