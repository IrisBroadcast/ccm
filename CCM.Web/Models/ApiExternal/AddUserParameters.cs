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

using CCM.Web.Infrastructure.PasswordGeneration;
using System.ComponentModel.DataAnnotations;
using CCM.Web.Infrastructure.ValidationAttributes;
using CCM.Web.Properties;

namespace CCM.Web.Models.ApiExternal
{
    public class UserModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string DisplayName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Comment { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.Password_Required))]
        [MinLength(PasswordComplexityConfiguration.MinLength, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.Password_Must_Be_At_Least_X_Characters_Long))]
        [MaxLength(PasswordComplexityConfiguration.MaxLength, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.Password_Can_Not_Be_Longer_Than_X_Characters))]
        [MustContainCharacters(PasswordComplexityConfiguration.MinNumberOfSpecial, PasswordComplexityConfiguration.AllowedSpecialCharacters)]
        [MustContainDigits(PasswordComplexityConfiguration.MinNumberOfDigits)]
        [MustContainLowerCaseLetters(PasswordComplexityConfiguration.MinNumberOfLower)]
        [MustContainUpperCaseLetters(PasswordComplexityConfiguration.MinNumberOfUpper)]
        public string NewPassword { get; set; }
    }

    public class AddUserModel : UserModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.Password_Required))]
        [MinLength(PasswordComplexityConfiguration.MinLength, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.Password_Must_Be_At_Least_X_Characters_Long))]
        [MaxLength(PasswordComplexityConfiguration.MaxLength, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.Password_Can_Not_Be_Longer_Than_X_Characters))]
        [MustContainCharacters(PasswordComplexityConfiguration.MinNumberOfSpecial, PasswordComplexityConfiguration.AllowedSpecialCharacters)]
        [MustContainDigits(PasswordComplexityConfiguration.MinNumberOfDigits)]
        [MustContainLowerCaseLetters(PasswordComplexityConfiguration.MinNumberOfLower)]
        [MustContainUpperCaseLetters(PasswordComplexityConfiguration.MinNumberOfUpper)]
        public string Password { get; set; }
    }
}
