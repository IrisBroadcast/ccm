﻿/*
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CCM.Web.Properties;

namespace CCM.Web.Infrastructure.ValidationAttributes
{
    public class MustContainCharactersAttribute : ValidationAttribute
    {
        private readonly int _minimumCount;
        private readonly string _allowedCharacters;

        public MustContainCharactersAttribute(int minimumCount, string allowedCharacters)
        {
            _minimumCount = minimumCount;
            _allowedCharacters = allowedCharacters;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stringValue = value as string;
            if (stringValue == null)
            {
                return ValidationResult.Success;
            }

            var numberOfSpecialCharacters = stringValue.Count(_allowedCharacters.Contains);
            if (numberOfSpecialCharacters < _minimumCount)
            {
                var charactersString = string.Join(" ", _allowedCharacters as IEnumerable<char>);
                var errorMessage = string.Format(Resources.Password_Must_Contain_At_Least_X_Of_Following_Characters_Y, _minimumCount, charactersString);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
