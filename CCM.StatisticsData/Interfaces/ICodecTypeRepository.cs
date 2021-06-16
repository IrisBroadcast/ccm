﻿using CCM.StatisticsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Interfaces
{
    public interface ICodecTypeRepository
    {
        public List<CodecTypeEntity> GetAll();
    }
}