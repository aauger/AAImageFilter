﻿using FilterDotNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDotNet.Drawing
{
    public class Graphics
    {
        private IImage _instance;

        public static Graphics FromIImage(IImage input) => new Graphics(input);
        private Graphics(IImage input)
        {
            this._instance = input;
        }

        public void PlotLine(int x1, int y1, int x2, int y2)
        {
            throw new NotImplementedException();
        }
    }
}
