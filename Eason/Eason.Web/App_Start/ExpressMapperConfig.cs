using Eason.Core.Users;
using Eason.EntityFramework.Entities.Authorization;
using Eason.EntityFramework.Entities.News;
using Eason.Web.Models;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.Web.App_Start
{
    public class ExpressMapperConfig
    {
        public static void RegisterMapper()
        {
            Mapper.Register<UserDto, User>();
            Mapper.Register<User, UserDto>();
            Mapper.Register<RegisterModel, UserDto>()
                .Member(m => m.creationTime, n => DateTime.Now)
                .Member(m => m.isActive, n => 0)
                .Member(m => m.isDelete, n => 0)
                  .Member(m => m.imgUrl, n => n.imgUrl)
                .Member(m => m.lastLoginTime, n => DateTime.Now)
               ;
            Mapper.Register<Article, BannerModel>()
                .Member(m => m.Id, n => n.id)
                 .Member(m => m.PicUrl, n => n.imageUrl)
                 .Member(m => m.Title, n => n.title)
                 .Member(m => m.OutLink, n => n.outLink)
                 .Member(m => m.Time, n => n.creationTime.ToString("yyyy-MM-dd hh:mm:ss"))
                 .Member(m => m.TitleClass, n => string.Empty);
            Mapper.Register<Article, ArticleListModel>()
               .Member(m => m.Id, n => n.id)
               .Member(m => m.CategoryCode, n => n.categoryId)
                .Member(m => m.mTitle, n => n.mTitle)
                 .Member(m => m.AuthorCode, n => n.creatorId)
                .Member(m => m.PicUrl, n => n.imageUrl)
                .Member(m => m.Title, n => n.title)
                .Member(m => m.Summary, n => n.desc)
                .Member(m => m.Author, n => n.creatorName)
                .Member(m => m.OutLink, n => n.outLink)
                .Member(m => m.Time, n => n.creationTime.ToString("yyyy-MM-dd hh:mm:ss"))
                .Member(m => m.TitleClass, n => string.Empty);
            Mapper.Register<Article, ArticleItemModel>()
              .Member(m => m.Id, n => n.id)
              .Member(m => m.CategoryCode, n => n.categoryId)
                 .Member(m => m.AuthorCode, n => n.creatorId)
               .Member(m => m.Title, n => n.title)
               .Member(m => m.Summary, n => n.desc)
               .Member(m => m.Author, n => n.creatorName)
                .Member(m => m.ReadNum, n => n.readNum)
                 .Member(m => m.VideoUrl, n => n.videoUrl)
               .Member(m => m.Time, n => n.creationTime.ToString("yyyy-MM-dd hh:mm:ss"))
               .Member(m => m.Content, n => n.contents);
            Mapper.Register<ArticleComment, CommentListModel>()

             .Member(m => m.cother, n => string.Empty)

              .Member(m => m.cname, n => n.creatorName)

              .Member(m => m.ctime, n => n.creationTime.ToString("yyyy-MM-dd hh:mm:ss"))
              .Member(m => m.ccont, n => n.contents)
               .Member(m => m.curl, n => n.imgUrl);

        }
    }
}