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

using CCM.Core.Helpers;
using CCM.DiscoveryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CCM.DiscoveryApi.Controllers
{
    /// <summary>
    /// Some basic information about the Discovery Api
    /// </summary>
    public class HomeController : Controller
    {
        protected ILogger<HomeController> _logger;
        private readonly ApplicationSettingsDiscovery _configuration;
        private readonly ApplicationBuildInformation _buildInformation;

        public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettingsDiscovery> configuration, IOptions<ApplicationBuildInformation> buildInformation)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _buildInformation = buildInformation.Value;
        }

        [HttpGet]
        [Route("~/", Name = "default")]
        public IActionResult Index()
        {
            var version = _buildInformation.Version;
            var releaseDate = _buildInformation.ReleaseDate;
            var html = @"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no'>
                    <meta name='description' content=''>
                    <title>IRIS Discovery</title>
                    <style type='text/css'>
                        body, html {
                            margin: 0;
                            padding: 0;
                            height: 100%;
                            background-color: #72c7bf !important;
                            font-family: Arial, Helvetica, sans-serif !important;
                            font-family: -apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Oxygen-Sans,Ubuntu,Cantarell,'Helvetica Neue',sans-serif !important;
                            -webkit-font-smoothing: subpixel-antialiased;
                            -ms-text-size-adjust: 100%;
                            -webkit-text-size-adjust: 100%;
                            line-height: 1;
                            color: #2d8b6c;
                        }
                        .container {
                            padding: 4vh 1vw 2vh 1vw;
                            margin: 0 auto;
                            max-width: 600px;
                            width: 95vw;
                        }
                        h1 {
                            border-top: 2vh solid #FFEB3B;
                            padding-top: 7vh
                            font-size: 5rem;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>IRIS<br/>Discovery</h1>
                        <h3>Version: " + version + @"<br/>Build date: " + releaseDate + @"</h3>
                    </div>
                </body>
                </html>";

            return new ContentResult()
            {
                Content = html,
                ContentType = "text/html"
            };
        }
    }
}
