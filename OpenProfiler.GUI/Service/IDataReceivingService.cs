﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.Service
{
    public interface IDataReceivingService
    {
        void StartCollecting();
        void StopCollecting();
    }
}
