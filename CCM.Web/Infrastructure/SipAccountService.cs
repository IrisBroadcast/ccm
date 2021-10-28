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

using CCM.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CCM.Web.Infrastructure
{
    public class SipAccountService : BackgroundService
    {
        private readonly ILogger<SipAccountService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SipAccountService(ILogger<SipAccountService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"*********** Starting CCM SIP Account Background service ************");

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await Task.Delay(15000, stoppingToken);

            //    using (var scope = _serviceProvider.CreateScope())
            //    {
            //        var _registeredRepository = scope.ServiceProvider.GetRequiredService<ICachedRegisteredCodecRepository>();

            //        var useragents = _registeredRepository.GetRegisteredCodecsUpdateTimes().ToList();
            //        var expireTime = DateTime.UtcNow;

            //        foreach (var sip in useragents)
            //        {
            //            var expectedAfter = expireTime.AddSeconds(-(sip.Expires + 220));
            //            if (sip.Updated < expectedAfter)
            //            {
            //                _logger.LogDebug($"Cleanup service found expired registration: {sip.SIP} Expire:{sip.Expires}+20 -- Expected later than this:{expectedAfter} but was Updated:{sip.Updated} # Now:{expireTime}");
            //                //_registeredRepository.DeleteRegisteredSip(sip.SIP);
            //                // TODO: this does not trigger websocket info maybe?
            //            }
            //        }
            //    }
            //}

            _logger.LogInformation($"*********** Stopping CCM SIP Account Background service ************");
        }
    }
}
