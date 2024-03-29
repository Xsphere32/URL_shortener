﻿using Microsoft.AspNetCore.Mvc;
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
        
       
        [HttpGet]
        public async Task<IActionResult> CreatedUrl(string FullUrl)
        {
            bool isContains = false;
            if (!FullUrl.StartsWith("http://") || !FullUrl.Contains("https://"))
            {
                 isContains = true;
            }
            if (!isContains)
            {
                FullUrl = "http://" + FullUrl;
            }
            Url url = new Url();
            using (var session = _SessionFactoryBuilder.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var ExistUrl = session.Query<Url>()
                                           .Where(o => o.FullUrl == FullUrl)
                                           .SingleOrDefault();
                    if (ExistUrl != null)
                    {
                        return PartialView(ExistUrl);
                    }
                    else
                    {
                        url.FullUrl = FullUrl;
                        url.DateOfCreation = DateTime.Now;
                        url.PassCount = 0;
                        url.ShortUrl = "http://auto.bus/" + GenerateCode();
                        await session.SaveOrUpdateAsync(url);
                        await transaction.CommitAsync();
                        return PartialView(url);
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