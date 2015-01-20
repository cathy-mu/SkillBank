using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Web.Context;
using System.Configuration;

namespace SkillBank.Site.Web
{
    public static class PagingHelper
    {

        public static void GetPagingListIds(int pageNum, int pageListSize, int pageId, out int pageMinId, out int pageMaxId)
        {
            pageMinId = 1;
            pageMaxId = pageNum;

            int adjustNumber = 0;
            int pageListSizeBuffer = (pageListSize - 1) / 2;//just for even number
            //not show all pages
            if (pageNum > pageListSize)
            {
                pageMinId = pageId - pageListSizeBuffer;
                pageMaxId = pageId + pageListSizeBuffer;
                if (pageMinId <= 0)
                {
                    adjustNumber = 1 - pageMinId;//pageListSizeBuffer - pageId + 1;
                    pageMaxId = pageId + pageListSizeBuffer + adjustNumber;
                    pageMinId = 1;
                }
                else if (pageMaxId > pageNum)
                {
                    adjustNumber = pageMaxId - pageNum;//pageListSizeBuffer + pageId - pageNum;
                    pageMinId = pageId - pageListSizeBuffer - adjustNumber;
                    pageMaxId = pageNum;
                }
            }
        }
    }
}
