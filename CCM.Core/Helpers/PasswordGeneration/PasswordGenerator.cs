/*
 * Copyright (c) 2019 Sveriges Radio AB, Stockholm, Sweden
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
using System.Collections.Generic;
using System.Linq;

namespace CCM.Core.Helpers.PasswordGeneration
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";

        private readonly PasswordGeneratorConfiguration configuration;
        private readonly Random random;
        private readonly string specialCharacters;
        private readonly string allCharacters;

        public PasswordGenerator(IPasswordGeneratorConfigurationProvider configurationProvider)
        {
            this.configuration = configurationProvider.GetConfiguration();
            this.random = new Random();
            this.specialCharacters = string.Concat(this.configuration.AllowedSpecialCharacters);
            this.allCharacters = string.Concat(LowerCaseLetters, UpperCaseLetters, Digits, this.specialCharacters);
        }

        public string GeneratePassword()
        {
            var generatedCharacters = new char?[this.configuration.Length];
            var generatorCriterias = new List<Criteria>
            {
                new Criteria(this.configuration.NumberOfDigits, Digits),
                new Criteria(this.configuration.NumberOfLowerCaseLetters, LowerCaseLetters),
                new Criteria(this.configuration.NumberOfUpperCaseLetters, UpperCaseLetters),
                new Criteria(this.configuration.NumberOfSpecialCharacters, this.specialCharacters),
            };

            // Generate characters according to criterias.
            foreach (var criteria in generatorCriterias)
            {
                for (int i = 0; i < criteria.Count; i++)
                {
                    // Get the free positions in the result string.
                    var freePositions = GetPositionsOfNull(generatedCharacters).ToArray();

                    // Get random position of the free positions.
                    var randomPosition = this.GetRandomItem(freePositions);

                    // Fill the random position with a random character.
                    generatedCharacters[randomPosition] = this.GetRandomCharacter(criteria.Characters);
                }
            }

            // Fill remaining positions in result string with any of the allowed characters.
            var remainingFreePositions = GetPositionsOfNull(generatedCharacters).ToList();
            foreach (var position in remainingFreePositions)
            {
                generatedCharacters[position] = this.GetRandomCharacter(this.allCharacters);
            }

            return string.Concat(generatedCharacters);
        }

        private static IEnumerable<int> GetPositionsOfNull<T>(IEnumerable<T> input)
        {
            int i = 0;
            foreach (var item in input)
            {
                if (item == null)
                {
                    yield return i;
                }

                i++;
            }
        }

        private char GetRandomCharacter(string input)
        {
            var index = this.random.Next(input.Length);
            return input[index];
        }

        private T GetRandomItem<T>(T[] items)
        {
            var index = this.random.Next(items.Length);
            return items[index];
        }

        private class Criteria
        {
            public Criteria(int count, string characters)
            {
                Count = count;
                Characters = characters;
            }

            public int Count { get; }

            public string Characters { get; }
        }
    }
}
