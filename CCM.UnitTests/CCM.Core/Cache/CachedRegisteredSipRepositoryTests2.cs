using System;
using System.Collections.Generic;
using System.Linq;
using CCM.Core.Cache;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using FakeItEasy;
using LazyCache;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core.Cache
{
    [TestFixture]
    public class CachedRegisteredSipRepositoryTests2
    {
        IRegisteredSipRepository _internalRegisteredSipRepository;
        private IAppCache _cache;

        private CachedRegisteredSipRepository _sut;

        [SetUp]
        public void SetUp()
        {
            _internalRegisteredSipRepository = A.Fake<IRegisteredSipRepository>();
            _cache = new CachingService();

            // Clear cache
            _cache.FullReload();

            _sut = new CachedRegisteredSipRepository(_cache, _internalRegisteredSipRepository);
        }

        [Test]
        public void should_load_list_from_internal_repository_on_first_access()
        {
            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void should_not_load_list_from_internal_repository_on_second_access()
        {
            _sut.GetCachedRegisteredSips();
            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void should_reload_list_from_internal_repository_when_codec_added()
        {
            var newRegisteredSip = new RegisteredSip() { SIP = "c" };
            A.CallTo(() => _internalRegisteredSipRepository.UpdateRegisteredSip(newRegisteredSip)).Returns(
                new KamailioMessageHandlerResult()
                {
                    ChangedObjectId = Guid.NewGuid(),
                    ChangeStatus = KamailioMessageChangeStatus.CodecAdded
                });

            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
            _sut.UpdateRegisteredSip(newRegisteredSip);
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void should_not_reload_list_from_internal_repository_when_codec_reregistered()
        {
            var registeredSip = new RegisteredSip() { SIP = "a@acip.example.com" };

            var internalList = new List<RegisteredSipDto>
            {
                new RegisteredSipDto {Id = Guid.NewGuid(), Sip = "a@acip.example.com"}
            };

            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).Returns(internalList);

            A.CallTo(() => _internalRegisteredSipRepository.UpdateRegisteredSip(registeredSip)).Returns(
                new KamailioMessageHandlerResult()
                {
                    ChangedObjectId = internalList[0].Id,
                    ChangeStatus = KamailioMessageChangeStatus.NothingChanged
                });

            // Act
            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
            _sut.UpdateRegisteredSip(registeredSip);
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void should_load_registeredSips_if_list_is_non_existing()
        {
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).Returns(new List<RegisteredSipDto>
            {
                new RegisteredSipDto {Id = Guid.NewGuid(), Sip = "a@acip.example.com"},
                new RegisteredSipDto {Id = Guid.NewGuid(), Sip = "b@acip.example.com"}
            });

            var regSipList = _sut.GetCachedRegisteredSips();

            Assert.IsNotNull(regSipList);
            Assert.AreEqual(2, regSipList.Count);
            Assert.AreEqual("a@acip.example.com", regSipList.First().Sip);

        }

        [Test]
        public void should_not_reload_registeredSips_if_list_already_loaded()
        {
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).Returns(new List<RegisteredSipDto>
            {
                new RegisteredSipDto {Id = Guid.NewGuid(), Sip = "a@acip.example.com"},
                new RegisteredSipDto {Id = Guid.NewGuid(), Sip = "b@acip.example.com"}
            });

            var regSipList = _sut.GetCachedRegisteredSips();
            Assert.IsNotNull(regSipList);
            Assert.AreEqual(2, regSipList.Count);
            Assert.AreEqual("a@acip.example.com", regSipList.First().Sip);
            
            var regSipList2 = _sut.GetCachedRegisteredSips();

            Assert.IsNotNull(regSipList2);
            Assert.AreEqual(2, regSipList2.Count);
            Assert.AreEqual("a@acip.example.com", regSipList2.First().Sip);

            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);

        }

        [Test]
        public void should_reload_registeredSips_when_codec_added()
        {
            var registeredSipDto = new RegisteredSipDto { Id = Guid.NewGuid(), Sip = "a@acip.example.com" };

            var internalList = new List<RegisteredSipDto>();

            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).Returns(internalList);

            A.CallTo(() => _internalRegisteredSipRepository.UpdateRegisteredSip(A<RegisteredSip>.Ignored)).Returns(
                new KamailioMessageHandlerResult()
                {
                    ChangedObjectId = Guid.NewGuid(),
                    ChangeStatus = KamailioMessageChangeStatus.CodecAdded
                });

            // Act
            var regSipList = _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
            Assert.IsNotNull(regSipList);
            Assert.AreEqual(0, regSipList.Count);

            internalList.Add(registeredSipDto);
            _sut.UpdateRegisteredSip(new RegisteredSip() { SIP = "a@acip.example.com" });
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);

            var regSipList2 = _sut.GetCachedRegisteredSips();
            Assert.IsNotNull(regSipList2);
            Assert.AreEqual(1, regSipList2.Count);
            Assert.AreEqual("a@acip.example.com", regSipList2[0].Sip);
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Twice);

        }

        [Test]
        public void should_reload_registeredSips_when_codec_removed()
        {
            // Assign
            var registeredSipDto = new RegisteredSipDto { Id = Guid.NewGuid(), Sip = "a@acip.example.com" };

            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).Returns(new List<RegisteredSipDto> { registeredSipDto });

            A.CallTo(() => _internalRegisteredSipRepository.UpdateRegisteredSip(A<RegisteredSip>.Ignored))
                .Returns( new KamailioMessageHandlerResult { ChangedObjectId = registeredSipDto.Id, ChangeStatus = KamailioMessageChangeStatus.CodecRemoved });

            // Act
            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);
            
            _sut.UpdateRegisteredSip(new RegisteredSip { SIP = "a@acip.example.com" });
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Once);

            _sut.GetCachedRegisteredSips();
            A.CallTo(() => _internalRegisteredSipRepository.GetCachedRegisteredSips()).MustHaveHappened(Repeated.Exactly.Twice);
        }
        
    }
}