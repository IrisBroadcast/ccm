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
using CCM.Core.SipEvent;
using CCM.Core.SipEvent.Messages;
using CCM.Core.SipEvent.Parser;
using CCM.Data.Repositories;
using Ninject;
using NUnit.Framework;

namespace CCM.Tests.KamailioTests
{
    [TestFixture, Explicit]
    public class RegistrationTests : SipMessageHandlerTestsBase
    {
        [SetUp]
        public void Setup()
        {
            _sipMessageManager = kernel.Get<KamailioMessageManager>();
            _sipRep = kernel.Get<RegisteredSipRepository>();
        }


        [Test]
        public void should_register_växjö_10()
        {
            var sipEvent = CreateSipRegisterEvent("192.0.2.82", "ProntoNet LC v6.8.1", "vaxjo-10@acip.example.com", "Växjö 10");
            var sipMessage = new SipEventParser().Parse(sipEvent);

            _sipMessageManager.HandleSipMessage(sipMessage);
            var sipRep = kernel.Get<RegisteredSipRepository>();
            var sip = sipRep.Single(rs => rs.SIP == "vaxjo-10@acip.example.com");

            Assert.IsNotNull(sip);
            Assert.AreEqual("192.0.2.82", sip.IP);
            Assert.AreEqual("RH Växjö", sip.Location.Name);
            Assert.AreEqual("ProntoNet", sip.UserAgent.Name);
            Assert.AreEqual("vaxjo-10@acip.example.com", sip.User.UserName);
        }
     


        [Test]
        public void should_register_new_codec()
        {
            // ASSIGN
            var userName = "patpet2@acip.example.com";

            // Delete any already registered codec
            DeleteRegisteredSip(userName);

            var ipAddress = GetRandomLocationIpAddress();
            var displayName = "Test";

            var sipEvent = CreateSipRegisterEvent(ipAddress, "ME-UMAC2-M/0.255", userName, displayName);
            var sipMessage = new SipEventParser().Parse(sipEvent);

            // ACT
            SipEventHandlerResult result = _sipMessageManager.HandleSipMessage(sipMessage);

            // ASSERT
            Assert.AreEqual(SipEventChangeStatus.CodecAdded, result.ChangeStatus);

            var sip = _sipRep.Single(rs => rs.SIP == userName);
            Assert.AreEqual(ipAddress, sip.IP);
            Assert.AreEqual(userName, sip.User.UserName);

            // Clean up
            DeleteRegisteredSip(sip.Id);
        }


        [Test]
        public void test_codec_registration()
        {
            var userName = "patpet2@acip.example.com";

            DeleteRegisteredSip(userName);

            // Add new
            var sipMessage = new SipRegistrationMessage()
            {
                Ip = GetRandomLocationIpAddress(),
                Port = 5060,
                UnixTimeStamp = GetUnixTimeStamp(DateTime.Now),
                Sip = new SipUri(userName),
                UserAgent = "ProntoNet LC v6.8.1",
                Username = userName,
                ToDisplayName = "Test",
                Expires = 60
            };

            // Assert
            var result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.CodecAdded, result.ChangeStatus);

            // Update only timestamp. Should return nothing changed.
            // Act
            sipMessage.UnixTimeStamp = GetUnixTimeStamp(DateTime.Now.AddSeconds(1));
            // Assert
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.NothingChanged, result.ChangeStatus);

            // Update location. Should return status updated.
            // Act
            sipMessage.Ip = GetRandomLocationIpAddress();
            // Assert
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.CodecUpdated, result.ChangeStatus);

            // Only timestamp
            sipMessage.UnixTimeStamp = GetUnixTimeStamp(DateTime.Now.AddSeconds(2));
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.NothingChanged, result.ChangeStatus);

            // Update display name. Should return status updated.
            // Act
            sipMessage.ToDisplayName = "New display name";
            // Assert
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.CodecUpdated, result.ChangeStatus);

            // Update user agent
            sipMessage.UserAgent = "Quantum/3.4.3";
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.CodecUpdated, result.ChangeStatus);

            // Only timestamp
            sipMessage.UnixTimeStamp = GetUnixTimeStamp(DateTime.Now.AddSeconds(3));
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(SipEventChangeStatus.NothingChanged, result.ChangeStatus);

        }
        
    }
}
