﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAImageFilter.Exceptions;
using AAImageFilter.Interfaces;
using AAImageFilter.Utils;

namespace AAImageFilter.Filters
{
    public class GlassFilter : IFilter, IConfigurableFilter
    {
        private readonly IPluginConfigurator<int> _pluginConfigurator;
        private int _glassDistance;
        private bool _ready = false;

        public GlassFilter(IPluginConfigurator<int> pluginConfigurator)
        { 
            this._pluginConfigurator = pluginConfigurator;
        }

        public Image Apply(Image input)
        {
            if (!_ready)
                throw new NotReadyException();

            Random rnd = new Random();
            Bitmap bmp = (Bitmap)input;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int x2 = MathUtils.Clamp(x + rnd.Next(-_glassDistance, _glassDistance), 0, bmp.Width - 1);
                    int y2 = MathUtils.Clamp(y + rnd.Next(-_glassDistance, _glassDistance), 0, bmp.Height - 1);
                    Color here = bmp.GetPixel(x, y);
                    Color there = bmp.GetPixel(x2, y2);

                    bmp.SetPixel(x, y, there);
                    bmp.SetPixel(x2, y2, here);
                }
            }

            return bmp;
        }

        public string GetFilterName()
        {
            return "Glass";
        }

        public IFilter Initialize()
        {
            _glassDistance = _pluginConfigurator.GetPluginConfiguration();
            _ready = true;
            return this;
        }
    }
}
