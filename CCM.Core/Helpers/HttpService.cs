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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace CCM.Core.Helpers
{
    // TODO: Remove me if im not in use
    //public class HttpService
    //{
    //    protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

    //    public static async Task<T> GetAsync<T>(Uri url) where T : class
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            var response = await client.GetAsync(url);

    //            if (!response.IsSuccessStatusCode)
    //            {
    //                Log.Warn("Request to {0} failed.", url);
    //                return null;
    //            }
    //            string jsonString = await response.Content.ReadAsStringAsync();
    //            return JsonConvert.DeserializeObject<T>(jsonString);
    //        }
    //    }

    //    public static async Task<HttpResponseMessage> PostJsonAsync(Uri url, object data = null)
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            try
    //            {
    //                HttpContent content = null;
    //                if (data != null)
    //                {
    //                    var s = JsonConvert.SerializeObject(data);
    //                    content = new StringContent(s, Encoding.UTF8, "application/json");
    //                }
    //                return await client.PostAsync(url, content);
    //            }
    //            catch (Exception)
    //            {
    //                return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
    //            }
    //        }
    //    }

    //}
}
