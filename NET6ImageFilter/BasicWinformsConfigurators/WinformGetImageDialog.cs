﻿using AAImageFilter.Interfaces;
using NET6ImageFilter.ImageProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET6ImageFilter.BasicWinformsConfigurators
{
    public class WinformGetImageDialog : IPluginConfigurator<IImage>
    {
        public IImage GetPluginConfiguration()
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            { 
                var image = Image.FromFile(dialog.FileName);
                if (image != null)
                    return new GDIDrawingImage((Bitmap)image);
            }
            return new GDIDrawingImage(1, 1);
        }
    }
}