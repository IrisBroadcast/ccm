using System;
using AutoMapper;
using coreentities = CCM.Core.Entities;
using dataentities = CCM.Data.Entities;
using CCM.Web.Infrastructure;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Data.AutoMapperTests
{
    [TestFixture]
    public class MapperTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AutoMapperWebConfiguration.Configure();
        }

        [Test]
        public void should_map_studioentity_to_studio()
        {
            var dbStudio = new dataentities.StudioEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Studio 54",
                CodecSipAddress = "test@acip.example.com",
                CameraAddress = "kameras45.example.com",
                CameraActive =  true,
                CameraUsername = "user",
                CameraPassword = "pwd",
                CameraVideoUrl = "/camera",
                AudioClipNames = "Dagge, Lars-Åke",
                NrOfGpos = 2,
                GpoNames = "Grön, Röd",
                NrOfAudioInputs = 3,
                AudioInputNames = "Mik1, Mik2, Mik3",
                InfoText = "En text",
                MoreInfoUrl = "example.com/moreinfo",
                CreatedBy = "anders",
                CreatedOn = DateTime.Parse("2017-02-09 13:01:00"),
                UpdatedBy = "putte",
                UpdatedOn = DateTime.Parse("2017-02-09 13:02:00")
            };

            var studio = Mapper.Map<coreentities.Studio>(dbStudio);

            Assert.AreEqual("Studio 54", studio.Name);
            Assert.AreEqual("test@acip.example.com", studio.CodecSipAddress);
            Assert.AreEqual("kameras45.example.com", studio.CameraAddress);
            Assert.AreEqual(true, studio.CameraActive);
            Assert.AreEqual("user", studio.CameraUsername);
            Assert.AreEqual("Dagge, Lars-Åke", studio.AudioClipNames);
            Assert.AreEqual(2, studio.NrOfGpos);
            Assert.AreEqual("Grön, Röd", studio.GpoNames);
            Assert.AreEqual(3, studio.NrOfAudioInputs);
            Assert.AreEqual("Mik1, Mik2, Mik3", studio.AudioInputNames);
            Assert.AreEqual("En text", studio.InfoText);
            Assert.AreEqual("example.com/moreinfo", studio.MoreInfoUrl);
            Assert.AreEqual("anders", studio.CreatedBy);
            Assert.AreEqual("putte", studio.UpdatedBy);
            Assert.AreEqual(dbStudio.CreatedOn, studio.CreatedOn);
            Assert.AreEqual(dbStudio.UpdatedOn, studio.UpdatedOn);
        }

        [Test]
        public void should_map_null_object_without_exception()
        {
            var studio = Mapper.Map<coreentities.Studio>(null);
            Assert.IsNull(studio);
        }


        [Test]
        public void should_map_groupentity_to_group()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var namn = "Grupp1";
            var text = "descrition for this group is optional";
            var user1 = "nisse";
            var user2 = "klara";
            var date1 = DateTime.Parse("2017-02-09 13:01:00");
            var date2 = DateTime.Parse("2017-02-09 13:02:00");

            var entity = new dataentities.ProfileGroupEntity()
            {
                Id = guid1,
                Name = namn,
                Description = text,
                CreatedBy = user1,
                CreatedOn = date1,
                UpdatedBy = user2,
                UpdatedOn = date2,
            };

            var mappedEntity = Mapper.Map<coreentities.ProfileGroup>(entity);

            Assert.AreEqual(namn, mappedEntity.Name);
            Assert.AreEqual(text, mappedEntity.Description);
            Assert.AreEqual(user1, mappedEntity.CreatedBy);
            Assert.AreEqual(date1, mappedEntity.CreatedOn);
            Assert.AreEqual(user2, mappedEntity.UpdatedBy);
            Assert.AreEqual(date2, mappedEntity.UpdatedOn);

            Assert.AreEqual(0, mappedEntity.Profiles.Count);

            //var mappedCollectionEntity = mappedEntity.Profiles.First();
            //Assert.AreEqual(guid2, mappedCollectionEntity.Id);


        }
    }
}
