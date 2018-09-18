using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CCM.Core.CodecControl.Interfaces;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Interfaces.Repositories.Specialized;
using CCM.Web.Models.StudioMonitor;

namespace CCM.Web.Controllers
{
    // Studio monitoring
    public class StudioMonitorController : Controller
    {
        private readonly IStudioRepository _studioRepository;
        private readonly ICodecManager _codecManager;
        private readonly ICodecInformationRepository _codecInformationRepository;

        public StudioMonitorController(IStudioRepository studioRepository, ICodecManager codecManager, ICodecInformationRepository codecInformationRepository)
        {
            _studioRepository = studioRepository;
            _codecManager = codecManager;
            _codecInformationRepository = codecInformationRepository;
        }

        public ActionResult Index(Guid id)
        {
            StudioMonitorViewModel vm = CreateViewModel(id);
            return View(vm);
        }

        private StudioMonitorViewModel CreateViewModel(Guid id)
        {
            var studio = _studioRepository.GetById(id);
            var vm = Mapper.Map<StudioMonitorViewModel>(studio);

            // URL is only present if the camera is active
            if (studio.CameraActive)
            {
                var baseUrl = new UriBuilder
                {
                    Scheme = "http",
                    Host = studio.CameraAddress,
                    UserName = studio.CameraUsername,
                    Password = !string.IsNullOrEmpty(studio.CameraUsername) ? studio.CameraPassword : string.Empty // Lösenord utan användarnamn ger ogiltig url.
                }.Uri;

                vm.CameraVideoUrl = new Uri(baseUrl, studio.CameraVideoUrl).ToString();
                vm.CameraImageUrl = new Uri(baseUrl, studio.CameraImageUrl).ToString();
                vm.CameraPlayAudioUrl = new Uri(baseUrl, studio.CameraPlayAudioUrl).ToString();
            }
            else
            {
                vm.CameraVideoUrl = vm.CameraImageUrl = string.Empty;
            }

            // TODO: Lägg till egenskaper från studions kodare + ev kodarstatus
            // TODO: T.ex. kan max antal ljudingångar och GPO:er kollas här.

            return vm;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            IList<Studio> studios;
            try
            {
                studios = _studioRepository.GetAll().OrderBy(s => s.Name).ToList();
            }
            catch (Exception)
            {
                studios = new List<Studio>();
            } 
            return View(studios);
        }

        public ActionResult HangUpStudio(Guid id)
        {
            var model = new HangUpStudioViewModel { StudioId = id };
            return View(model);
        }

        public ActionResult DoHangUp(HangUpStudioViewModel model)
        {
            StudioMonitorViewModel vm = CreateViewModel(model.StudioId);

            if (model.IHaveChecked && model.IHaveBooked)
            {
                var codec = _codecInformationRepository.GetCodecInformationBySipAddress(vm.CodecSipAddress);
                _codecManager.HangUpAsync(codec);
            }

            return RedirectToAction("Index", new { Id = vm.StudioId } );
        }
     
    }
}