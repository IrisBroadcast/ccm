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

using System.Collections.Generic;
using CCM.Core.Cache;
using CCM.Core.Entities.Specific;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.CacheTests
{
    [TestFixture]
    public class ProfileCacheTests
    {
        [Test]
        public void GetFromCache()
        {
            IAppCache cache = new CachingService();

            var profiles = cache.GetOrAddAllProfileNamesAndSdp(
                () => new List<ProfileNameAndSdp>()
                {
                    new ProfileNameAndSdp() {Name = "Profile 1", Sdp = "Sdp A"},
                    new ProfileNameAndSdp() {Name = "Profile 2", Sdp = "Sdp B"},
                    new ProfileNameAndSdp() {Name = "Profile 3", Sdp = "Sdp C"}
                });

            Assert.IsNotNull(profiles);
            Assert.AreEqual(3, profiles.Count);
            Assert.AreEqual("Profile 3", profiles[2].Name);

            var sameProfiles = cache.GetOrAddAllProfileNamesAndSdp(null);
            Assert.IsNotNull(sameProfiles);
            Assert.AreEqual(3, sameProfiles.Count);
            Assert.AreEqual("Profile 3", profiles[2].Name);
        }

        [Test]
        public void DeleteFromCache()
        {
            IAppCache cache = new CachingService();

            var profiles = cache.GetOrAddAllProfileNamesAndSdp(
                () => new List<ProfileNameAndSdp>()
                {
                    new ProfileNameAndSdp() {Name = "Profile 1", Sdp = "Sdp A"},
                    new ProfileNameAndSdp() {Name = "Profile 2", Sdp = "Sdp B"},
                    new ProfileNameAndSdp() {Name = "Profile 3", Sdp = "Sdp C"}
                });

            Assert.IsNotNull(profiles);
            Assert.AreEqual(3, profiles.Count);
            Assert.AreEqual("Profile 3", profiles[2].Name);

            cache.ClearProfiles();

            var sameProfiles = cache.GetOrAddAllProfileNamesAndSdp(null);
            Assert.IsNull(sameProfiles);
        }

    }
}
