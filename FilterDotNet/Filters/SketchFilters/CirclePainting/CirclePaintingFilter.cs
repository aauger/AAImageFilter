﻿using FilterDotNet.Extensions;
using FilterDotNet.Interfaces;
using FilterDotNet.Utils;
using FilterDotNet.Exceptions;

namespace FilterDotNet.Filters
{
    public class CirclePaintingFilter : IFilter, IConfigurableFilter
    {
        /* DI */
        private readonly IPluginConfigurator<(int, int, int)> _pluginConfigurator;
        private readonly IEngine _engine;

        /* Internals */
        private bool _ready = false;
        private int _maxDiff = 0, _minRad = 0, _maxRad = 0;

        /* Properties */
        public string Name => "Circle Painting";

        public CirclePaintingFilter(IPluginConfigurator<(int, int, int)> pluginConfigurator, IEngine engine)
        {
            this._pluginConfigurator = pluginConfigurator;
            this._engine = engine;
        }

        public IImage Apply(IImage input)
        {
            if (!this._ready)
                throw new NotReadyException();

            IImage output = this._engine.CreateImage(input.Width, input.Height);

            List<Circle> circles = new();
            for (int x = 0; x < input.Width; x += _minRad * 2)
            {
                for (int y = 0; y < input.Height; y += _minRad * 2)
                {
                    Circle c = new();
                    c.X = x;
                    c.Y = y;
                    c.Radius = _minRad;
                    c.Color = input.GetPixel(x, y);
                    circles.Add(c);
                }
            }

            Parallel.ForEach(circles, (c) =>
            {
                for (bool f = true; f && c.Radius < _maxRad; c.Radius++)
                {
                    int originX = c.X;
                    int originY = c.Y;

                    for (int deg = 0; deg < 360; deg += 10)
                    {
                        int rx = (int)(c.Radius * Math.Cos(Utilities.DegToRad(deg)) + originX);
                        int ry = (int)(c.Radius * Math.Sin(Utilities.DegToRad(deg)) + originY);

                        if (rx >= 0 && rx < input.Width
                            && ry >= 0 && ry < input.Height
                            && input.GetPixel(rx, ry).Difference(c.Color!) >= _maxDiff)
                        {
                            f = false;
                            break;
                        }
                    }
                }
            });

            foreach (Circle c in circles/*.Where(c => c.Radius > _minRad)*/.OrderBy(c => c.Radius))
            {
                Parallel.For(-c.Radius, c.Radius, (int x) =>
                {
                    int height = (int)Math.Sqrt(c.Radius * c.Radius - x * x);

                    Parallel.For(-height, height, (int y) =>
                    {
                        if (!output.OutOfBounds(c.X + x, c.Y + y))
                            output.SetPixel(c.X + x, c.Y + y, c.Color!);
                    });
                });
            }

            return output;
        }

        public IFilter Initialize()
        {
            (this._maxDiff, this._minRad, this._maxRad) = this._pluginConfigurator.GetPluginConfiguration();
            this._ready = true;
            return this;
        }
    }
}
