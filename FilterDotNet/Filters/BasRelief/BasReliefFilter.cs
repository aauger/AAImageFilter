﻿using FilterDotNet.Exceptions;
using FilterDotNet.Interfaces;
using FilterDotNet.Utils;

namespace FilterDotNet.Filters
{
    public class BasReliefFilter : IFilter, IConfigurableFilter
    {
        /* DI */
        private readonly IPluginConfigurator<int> _pluginConfigurator;
        private readonly IEngine _engine;

        /* Internals */
        private int _basReliefHeight;
        private bool _ready = false;

        /* Properties */
        public string Name => "Bas Relief";

        public BasReliefFilter(IPluginConfigurator<int> pluginConfigurator, IEngine engine)
        {
            this._pluginConfigurator = pluginConfigurator;
            this._engine = engine;
        }

        public IImage Apply(IImage input)
        {
            if (!this._ready)
                throw new NotReadyException();

            IImage output = this._engine.CreateImage(input.Width, input.Height);

            Parallel.For(0, input.Width - this._basReliefHeight, (int x) => 
            {
                Parallel.For(0, input.Height - this._basReliefHeight, (int y) => 
                {
                    IColor fs = input.GetPixel(x, y);
                    IColor sn = input.GetPixel(x + this._basReliefHeight, y + this._basReliefHeight);
                    int red = this._engine.Clamp(fs.R + (this._engine.MaxValue / 2) - sn.R);
                    int green = this._engine.Clamp(fs.G + (this._engine.MaxValue / 2) - sn.G);
                    int blue = this._engine.Clamp(fs.B + (this._engine.MaxValue / 2) - sn.B);

                    output.SetPixel(x, y, this._engine.CreateColor(red, green, blue, this._engine.MaxValue));
                });
            });

            return output;
        }

        public IFilter Initialize()
        {
            this._basReliefHeight = this._pluginConfigurator.GetPluginConfiguration();
            this._ready = true;
            return this;
        }
    }
}
