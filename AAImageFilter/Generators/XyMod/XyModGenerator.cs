﻿using FilterDotNet.Exceptions;
using FilterDotNet.Interfaces;

namespace FilterDotNet.Generators
{
    public class XyModGenerator : IGenerator, IConfigurableGenerator
    {
        /* DI */
        private readonly IGeneratorConfigurator<(int, int, int)> _generatorConfigurator;
        private readonly Func<int, int, IImage> _imageCreator;
        private readonly Func<int, int, int, int, IColor> _colorCreator;

        /* Internals */
        private bool _ready = false;
        private int _width = 0, _height = 0, _mod = 0;

        /* Properties */
        public string Name => "XY Mod Generator";

        public XyModGenerator(IGeneratorConfigurator<(int, int, int)> generatorConfigurator, Func<int, int, IImage> imageCreator, Func<int, int, int, int, IColor> colorCreator)
        {
            this._generatorConfigurator = generatorConfigurator;
            this._imageCreator = imageCreator;
            this._colorCreator = colorCreator;
        }

        public IImage Generate()
        {
            if (!this._ready)
                throw new NotReadyException();

            IColor black = _colorCreator(0, 0, 0, 255);
            IColor white = _colorCreator(255, 255, 255, 255);

            IImage image = this._imageCreator(this._width, this._height);

            Parallel.For(0, this._width, (int x) =>
            {
                Parallel.For(0, this._height, (int y) =>
                {
                    if ((x ^ y) % _mod != 0)
                    {
                        image.SetPixel(x, y, white);
                    }
                    else
                    {
                        image.SetPixel(x, y, black);
                    }
                });
            });

            return image;
        }

        public IGenerator Initialize()
        {
            (this._width, this._height, this._mod) = _generatorConfigurator.GetGeneratorConfiguration();
            this._ready = true;
            return this;
        }
    }
}
