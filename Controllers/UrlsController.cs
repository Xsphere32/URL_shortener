using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URL_shortener.Factory;
using URL_shortener.Models;

namespace URL_shortener.Controllers
{
    public class UrlsController : Controller
    {
        private readonly ISessionFactory _SessionFactoryBuilder;
        public UrlsController()
        {
            _SessionFactoryBuilder = SessionFactoryBuilder.BuildSessionFactory(true,true);
        }
        public IActionResult Index()
        {
            try
            {
                using (var session = _SessionFactoryBuilder.OpenSession())
                {
                   IList<Url> urls = session.CreateCriteria<Url>().List<Url>();
                   return View(urls);
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
            
        }

        public IActionResult New()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Url url)
        {
            using (var session = _SessionFactoryBuilder.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    url.DateOfCreation = DateTime.Now;
                    url.PassCount = 0;
                    url.ShortUrl = "http://auto.bus/" + GenerateCode();
                    var ExistUrls = session.CreateCriteria<Url>().Add(Restrictions.Like("FullUrl", url.FullUrl, MatchMode.Anywhere)).List<Url>();
                    if (ExistUrls.Count == 0)
                    {
                        await session.SaveOrUpdateAsync(url);
                        await transaction.CommitAsync();
                        return RedirectToAction("Index", "Urls");
                    }
                    else
                    {
                        var ExistUrl = ExistUrls.FirstOrDefault();
                        return Content($"Такая запись уже есть в БД {ExistUrl.ShortUrl} ");
                    }
                    
                }
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var session = _SessionFactoryBuilder.OpenSession())
            {
                var Item = session.Query<Url>()
                                  .Where(x => x.ID == id)
                                  .FirstOrDefault();
                if (Item != null)
                {
                    return View(Item);
                }
                else
                {
                    return Content("Неопознанная ошибка");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Url url)
        {
            using (var session = _SessionFactoryBuilder.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.SaveOrUpdateAsync(url);
                    await transaction.CommitAsync();
                }
            }
            return RedirectToAction("Index", "Urls");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            using (var session = _SessionFactoryBuilder.OpenSession())
            {
                var Item = session.Query<Url>()
                                  .Where(x => x.ID == id)
                                  .FirstOrDefault();
                if (Item != null)
                {
                    return View(Item);
                }
                else
                {
                    return Content("Неопознанная ошибка");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Url url)
        {
            using (var session = _SessionFactoryBuilder.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.DeleteAsync(url);
                    await transaction.CommitAsync();
                }
            }
            return RedirectToAction("Index", "Urls");
        }

        private string GenerateCode()
        {
            Random random = new Random();
            string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            for (int i = 0; i <6; i++)
            {
                int CharNum = random.Next(1, 52);
                result += Alphabet[CharNum];
            }
            return result;
        }
    }
}