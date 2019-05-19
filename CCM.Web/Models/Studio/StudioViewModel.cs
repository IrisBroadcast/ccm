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
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Studio
{
    public class StudioViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Studio_Error_Message_Name_Too_Long")]
        [Display(ResourceType = typeof(Resources), Name = "Studio_Studio_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Audio_Codec_Sip_Address")]
        public string CodecSipAddress { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Camera_Host_Or_Ip_Address")]
        public string CameraAddress { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Camera_Is_In_Use")]
        public bool CameraActive { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Camera_User_Name")]
        public string CameraUsername { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Camera_Password")]
        public string CameraPassword { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Url_To_Camera_Video_Feed")]
        public string CameraVideoUrl { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Camera_Still_Image")]
        public string CameraImageUrl { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Camera_Audio_Url")]
        public string CameraPlayAudioUrl { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Names_Of_Audio_Clips")]
        public string AudioClipNames { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Information_Text")]
        public string InfoText { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Url_To_Page_With_Manual")]
        public string MoreInfoUrl { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Number_Of_Inputs")]
        public int NrOfAudioInputs { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Inputs_Names")]
        public string AudioInputNames { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Studio_Preselected_Input_Level")]
        public string AudioInputDefaultGain { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Nr_Of_Gpos")]
        public int NrOfGpos { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Gpo_Names")]
        public string GpoNames { get; set; }

        [Required]
        [Range(0, 60)]
        [Display(ResourceType = typeof(Resources), Name = "Studio_Number_Of_Miutes_Before_Page_Gets_Inactive")]
        public int InactivityTimeout { get; set; }
    }
}
