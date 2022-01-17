﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDotNet.Interfaces
{
    public interface IEngine
    {
        Func<int, int, IImage> CreateImage { get; }
        Func<int, int, int, int, IColor> CreateColor { get; }
        Func<int, int> Clamp { get; }
        int MaxValue { get; }
        int MinValue { get; }
    }
}
