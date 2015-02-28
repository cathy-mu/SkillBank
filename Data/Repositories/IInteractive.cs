using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Data.Entity;

using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.DataSource.Data
{
    public interface IInteractiveRepository
    {
        void UpdateLike(Byte saveType, Byte favoriteType, int memberId, int relatedId, Boolean isLike);
        int UpdateComment(Byte saveType, int memberId, int paraId, String commentText);
    }

    public class InteractiveRepository : Entities, IInteractiveRepository
    {
        public InteractiveRepository()
        {
        }

        public void UpdateLike(Byte saveType, Byte favoriteType, int memberId, int relatedId, Boolean isLike)
        {
            Favorite_Save_p(saveType, favoriteType, memberId, relatedId, isLike, false);
        }

        public int UpdateComment(Byte saveType, int memberId, int paraId, String commentText)
        {
            return Comment_Save_p(saveType, memberId, paraId, commentText);
        }


        private void Favorite_Save_p(Byte saveType, Byte favoriteType, int memberId, int relatedId, Boolean isLike, Boolean isFavorite)
        {
            var saveTypePara = new ObjectParameter("saveType", saveType);
            var favoritePara = new ObjectParameter("favoriteType", favoriteType);
            var paraIdPara = new ObjectParameter("paraId", relatedId);
            var memberIdPara = new ObjectParameter("memberId", memberId);
            var isLikePara = new ObjectParameter("isLike", isLike);
            var isFavoritePara = new ObjectParameter("isFavorite", isFavorite);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Favorite_Save_p", saveTypePara, favoritePara, paraIdPara, memberIdPara, isLikePara, isFavoritePara);
        }


        private int Comment_Save_p(Byte saveType, int memberId, int paraId, String commentText)
        {
            int id = 0;
            var saveTypePara = new ObjectParameter("SaveType", saveType);
            var paraIdPara = new ObjectParameter("paraId", paraId);
            var memberIdPara = new ObjectParameter("MemberId", memberId);
            var commentTextPara = new ObjectParameter("CommentText", commentText);
            var idPara = new ObjectParameter("Id", id);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Comment_Save_p", saveTypePara, paraIdPara, memberIdPara, commentTextPara, idPara);
            return (int)idPara.Value;
        }


    }
}


