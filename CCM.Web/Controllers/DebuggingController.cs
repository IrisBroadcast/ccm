/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Web.Mvc;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Authentication;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CCM.Web.Controllers
{
    [CcmAuthorize(Roles = "Admin, Remote")]
    [RoutePrefix("debug")]
    public class DebuggingController : Controller
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly ICcmUserRepository _ccmUserRepository;
        private readonly ISipAccountRepository _sipAccountRepository;

        public DebuggingController(ICcmUserRepository ccmUserRepository, ISipAccountRepository sipAccountRepository)
        {
            _ccmUserRepository = ccmUserRepository;
            _sipAccountRepository = sipAccountRepository;
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("getccmusers")]
        public ActionResult GetCcmUsers()
        {
            List<CcmUser> users = _ccmUserRepository.GetAll();
            return View(users);
        }

        [Route("getsipaccounts")]
        public ActionResult GetSipAccounts()
        {
            var accounts = _sipAccountRepository.GetAll();
            return View(accounts);
        }

        [Route("logtest")]
        public ActionResult LogTest()
        {
            EnableLoggingTarget(out var target, out var loggingRule);

            log.Debug("debug message");
            log.Warn("Warn message");
            log.Error(new ApplicationException("testing"), "Jag tror nåt gick fel");

            DisableLoggingTarget(loggingRule);

            ViewBag.Title = "Log Test";
            return View("ShowLog", target.Logs);
        }

        private void EnableLoggingTarget(out MemoryTarget target, out LoggingRule loggingRule)
        {
            target = new MemoryTarget
            {
                Layout = "${longdate} ${level:uppercase=true:padding=-7} ${message} ${exception:format=tostring}"
            };

            loggingRule = new LoggingRule();
            loggingRule.Targets.Add(target);
            loggingRule.EnableLoggingForLevels(LogLevel.Trace, LogLevel.Fatal);
            LogManager.Configuration.LoggingRules.Add(loggingRule);
            LogManager.ReconfigExistingLoggers();
        }

        private static void DisableLoggingTarget(LoggingRule loggingRule)
        {
            LogManager.Configuration.LoggingRules.Remove(loggingRule);
            LogManager.ReconfigExistingLoggers();
        }

        [Route("whoami")]
        public ActionResult WhoAmI()
        {
            return View();
        }

        [Route("cacheinfo")]
        public ActionResult CacheInfo()
        {
            return View();
        }

        [Route("getcacheinfo")]
        public ActionResult GetCacheInfo()
        {
            var memoryCache = MemoryCache.Default;

            string name = memoryCache.Name;
            long count = memoryCache.GetCount();
            int hashCode = memoryCache.GetHashCode();

            var model = new CacheViewModel()
            {
                Name = name,
                Count = count,
                HashCode = hashCode,
                CachedItems = new List<CachedItem>()
            };

            Debug.WriteLine($"Cache name: {name}");
            Debug.WriteLine($"Antal cachade objekt: {count}");
            Debug.WriteLine($"Cache hash code: {hashCode}");
            Debug.WriteLine("");

            Debug.WriteLine("Cachens innehåll");

            var cacheEnumberable = (IEnumerable)memoryCache;
            foreach (DictionaryEntry item in cacheEnumberable)
            {
                IList cachedList = item.Value as IList;

                var cachedItem = new CachedItem
                {
                    CacheKey = item.Key.ToString(),
                    CachedObject = item.Value,
                    CachedType = item.Value.GetType(),
                    ListCount = cachedList?.Count
                };
                model.CachedItems.Add(cachedItem);

                Debug.WriteLine($"Cachenyckel: {cachedItem.CacheKey}");
                Debug.WriteLine($"Cachad type: {cachedItem.CachedType}");

                if (cachedList != null)
                {
                    Debug.WriteLine("Cachat objekt är en lista");
                    Debug.WriteLine($"Antal objekt i cachad lista: {cachedItem.ListCount}");

                    foreach (var listItem in cachedList)
                    {
                        Debug.WriteLine($"\t{listItem}");
                    }
                }
                Debug.WriteLine("");
            }

            return View(model);
        }
    }

    public class CachedItem
    {
        public string CacheKey { get; set; }
        public Type CachedType { get; set; }
        public int? ListCount { get; set; }
        public object CachedObject { get; set; }
    }

    public class CacheViewModel
    {
        public string Name { get; set; }
        public long Count { get; set; }
        public int HashCode { get; set; }
        public IList<CachedItem> CachedItems { get; set; }
    }

}
