using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CCM.Core.Entities;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories
{
    public class StudioRepository : BaseRepository, IStudioRepository
    {
        public StudioRepository(IAppCache cache) : base(cache)
        {
        }

        public void Save(Studio studio)
        {
            using (var db = GetDbContext())
            {
                StudioEntity dbStudio;
                var timeStamp = DateTime.UtcNow;

                if (studio.Id != Guid.Empty)
                {
                    dbStudio = db.Studios.SingleOrDefault(g => g.Id == studio.Id);

                    if (dbStudio == null)
                    {
                        throw new Exception("Studio could not be found");
                    }
                }
                else
                {
                    dbStudio = new StudioEntity()
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = studio.CreatedBy,
                        CreatedOn = timeStamp,
                    };

                    studio.Id = dbStudio.Id;
                    studio.CreatedOn = dbStudio.CreatedOn;
                    db.Studios.Add(dbStudio);
                }


                dbStudio.Name = studio.Name;
                dbStudio.CodecSipAddress = studio.CodecSipAddress;
                dbStudio.CameraAddress = studio.CameraAddress;
                dbStudio.CameraActive = studio.CameraActive;
                dbStudio.CameraUsername = studio.CameraUsername;
                dbStudio.CameraPassword = studio.CameraPassword;
                dbStudio.CameraVideoUrl = studio.CameraVideoUrl;
                dbStudio.CameraImageUrl = studio.CameraImageUrl;
                dbStudio.CameraPlayAudioUrl = studio.CameraPlayAudioUrl;
                dbStudio.AudioClipNames = studio.AudioClipNames;
                dbStudio.InfoText = studio.InfoText;
                dbStudio.MoreInfoUrl = studio.MoreInfoUrl;
                dbStudio.NrOfAudioInputs = studio.NrOfAudioInputs;
                dbStudio.AudioInputNames = studio.AudioInputNames;
                dbStudio.AudioInputDefaultGain = studio.AudioInputDefaultGain;
                dbStudio.NrOfGpos = studio.NrOfGpos;
                dbStudio.GpoNames = studio.GpoNames;
                dbStudio.InactivityTimeout = studio.InactivityTimeout;
                dbStudio.UpdatedBy = studio.UpdatedBy;
                studio.UpdatedOn = dbStudio.UpdatedOn = timeStamp;

                db.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var db = GetDbContext())
            {
                var dbStudio = db.Studios.SingleOrDefault(g => g.Id == id);
                if (dbStudio != null)
                {
                    db.Studios.Remove(dbStudio);
                    db.SaveChanges();
                }
            }
        }

        public Studio GetById(Guid id)
        {
            using (var db = GetDbContext())
            {
                var dbStudio = db.Studios.SingleOrDefault(g => g.Id == id);
                return MapToStudio(dbStudio);
            }
        }

        public List<Studio> GetAll()
        {
            using (var db = GetDbContext())
            {
                var dbStudios = db.Studios.ToList();
                return dbStudios.Select(MapToStudio).OrderBy(g => g.Name).ToList();
            }
        }

        public List<Studio> FindStudios(string search)
        {
            using (var db = GetDbContext())
            {
                search = (search ?? string.Empty).ToLower();
                var dbStudios = db.Studios.Where(r => r.Name.ToLower().Contains(search)).ToList();
                return dbStudios.Select(MapToStudio).OrderBy(g => g.Name).ToList();
            }
        }
        
        private Studio MapToStudio(StudioEntity dbStudio)
        {
            return Mapper.Map<Studio>(dbStudio);
        }
        
    }
}