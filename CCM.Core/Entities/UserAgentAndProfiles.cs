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
using System.Collections.Generic;
using CCM.Core.Enums;

namespace CCM.Core.Entities
{
    public class UserAgentAndProfiles
    {
        public UserAgentAndProfiles(
            Guid id,
            string name,
            string identifier,
            MatchType matchType,
            string imagePath,
            string userInterfaceLink,
            bool activeX,
            int width,
            int height,
            string comment,
            string apiType,
            int connectionLines,
            int inputs,
            int outputs,
            int nrOfGpos,
            int inputMinDb,
            int inputMaxDb,
            int inputGainStep,
            string gpoNames,
            bool userInterfaceIsOpen,
            bool useScrollbars,
            List<Profile> profiles)
        {
            Id = id;
            Name = name;
            Identifier = identifier;
            MatchType = matchType;
            Image = imagePath;
            UserInterfaceLink = userInterfaceLink;
            Ax = activeX;
            Width = width;
            Height = height;
            Comment = comment;
            Api = apiType;
            Lines = connectionLines;
            Inputs = inputs;
            Outputs = outputs; // TODO: add this
            NrOfGpos = nrOfGpos;
            InputMinDb = inputMinDb;
            InputMaxDb = inputMaxDb;
            InputGainStep = inputGainStep;
            GpoNames = gpoNames;
            UserInterfaceIsOpen = userInterfaceIsOpen;
            UseScrollbars = useScrollbars;
            Profiles = profiles;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Identifier { get; }
        public MatchType MatchType { get; }
        public string Image { get; }
        public string UserInterfaceLink { get; }
        public bool Ax { get; }
        public int Width { get; }
        public int Height { get; }
        public string Comment { get; }
        public string Api { get; }
        public int Lines { get; }
        public int Inputs { get; }
        public int Outputs { get; } // New addition
        public int NrOfGpos { get; }
        public int InputMinDb { get; }
        public int InputMaxDb { get; }
        public int InputGainStep { get; }
        public string GpoNames { get; }
        public bool UserInterfaceIsOpen { get; }
        public bool UseScrollbars { get; }
        public List<Profile> Profiles { get; }
    }
}
