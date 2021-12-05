﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAImageFilter.Interfaces;

namespace AAImageFilter.Filters
{
    public class Threshold : IFilter, IConfigurableFilter
    {
        private IPluginConfigurator<int> _pluginConfigurator;
        private int _threshold;

        private bool _ready = false;

        public Threshold(IPluginConfigurator<int> pluginConfigurator)
        { 
            this._pluginConfigurator = pluginConfigurator;
        }

        public Image Apply(Image input)
        {
            if (!_ready)
                throw new Exception("Not ready");

            Bitmap b = (Bitmap)input;

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color here = b.GetPixel(x, y);
                    int avg = (here.R + here.G + here.B) / 3;
                    Color nColor = avg > _threshold ? Color.White : Color.Black;

                    b.SetPixel(x, y, nColor);
                }
            }

            return b;
        }

        public string GetFilterName()
        {
            return "Threshold";
        }

        public IFilter Initialize()
        {
            _threshold = _pluginConfigurator.GetPluginConfiguration();
            _ready = true;
            return this;
        }
    }
}
