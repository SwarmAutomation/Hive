﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveSuite.Core
{
    public interface ISettings
    {
        void Load(string filePath);
        void Save(string filePath);

    }
}
