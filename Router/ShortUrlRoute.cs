using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URL_shortener.Factory;
using URL_shortener.Models;

namespace URL_shortener.Router
{
    public class ShortUrlRoute : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            throw new NotImplementedException();
        }

        public Task RouteAsync(RouteContext context)
        {
            var SessionFactory = SessionFactoryBuilder.BuildSessionFactory(true, true);
            using (var Session = SessionFactory.OpenSession())
            {
                using (var transaction = Session.BeginTransaction())
                {
                    string url = context.HttpContext.Request.Path.Value.TrimStart('/').TrimEnd('/');
                    var FullUrl = Session.CreateCriteria<Url>().Add(Restrictions.Like("ShortUrl", url, MatchMode.Anywhere));
                    IList<Url> Urls =  FullUrl.List<Url>();
                   
                    foreach(var record in Urls)
                    {
                        if (url != "" && record.ShortUrl.Contains(url))
                        {
                            record.PassCount++;
                            Session.SaveOrUpdate(record);
                            context.Handler = async ctx =>
                            {
                                ctx.Response.Redirect(record.FullUrl, true);
                            };
                        }
                    }
                    transaction.Commit();
                }
            }
            return Task.CompletedTask;
        }
    }
}
