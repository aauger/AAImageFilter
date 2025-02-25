﻿using FilterDotNet.Exceptions;
using FilterDotNet.Interfaces;

namespace FilterDotNet.Filters
{
    public class DissolveFilter : IFilter, IConfigurableFilter
    {
        /* DI */
        private readonly IPluginConfigurator<(IImage, int)> _pluginConfigurator;
        private readonly IEngine _engine;

        /* Internals */
        private readonly int _RAND_VAL_MAX = 101;
        private readonly int _PIX_MAP_SIZE = 100;

        private bool _ready = false;
        private IImage? _other;
        private int? _factor;
        private bool[,]? _pixMap;
        
        /* Properties */
        public string Name => "Dissolve";

        public DissolveFilter(IPluginConfigurator<(IImage, int)> pluginConfigurator, IEngine engine)
        {
            this._pluginConfigurator = pluginConfigurator;
            this._engine = engine;
        }

        public IImage Apply(IImage input)
        {
            if (!this._ready)
                throw new NotReadyException();

            if (this._other is null 
                || this._factor is null
                || this._pixMap is null
                || !(input.Width == this._other.Width && input.Height == this._other.Height))
                throw new BadConfigurationException("input and configured image have different dimensions");

            IImage output = this._engine.CreateImage(input.Width, input.Height);

            Parallel.For(0, input.Width, (int x) => 
            {
                Parallel.For(0, input.Height, (int y) =>
                {
                    IColor color = (this._pixMap[x % this._PIX_MAP_SIZE, y % this._PIX_MAP_SIZE]) switch
                    {
                        true => input.GetPixel(x, y),
                        false => this._other.GetPixel(x, y),
                    };

                    output.SetPixel(x, y, color);
                });
            });

            return output;
        }

        public IFilter Initialize()
        {
            Random random = new Random();
            (this._other, this._factor) = _pluginConfigurator.GetPluginConfiguration();                
            this._pixMap = new bool[this._PIX_MAP_SIZE, this._PIX_MAP_SIZE];
            for (int x = 0; x < this._PIX_MAP_SIZE; x++)
            {
                for (int y = 0; y < this._PIX_MAP_SIZE; y++)
                {
                    this._pixMap[x, y] = random.Next(this._RAND_VAL_MAX) > this._factor;
                }
            }
            this._ready = true;
            return this;
        }
    }
}
