﻿using FilterDotNet.Interfaces;
using FilterDotNet.Exceptions;
using FilterDotNet.Utils;
using FilterDotNet.Extensions;

namespace FilterDotNet.Filters
{
    public class DivideFilter : IFilter, IConfigurableFilter
    {
        /* DI */
        private readonly IPluginConfigurator<IImage> _pluginConfigurator;
        private readonly IEngine _engine;

        /* Internals */
        private bool _ready = false;
        private IImage _other;

        /* Properties */
        public string Name => "Divide";

        public DivideFilter(IPluginConfigurator<IImage> pluginConfigurator, IEngine engine)
        {
            this._pluginConfigurator = pluginConfigurator;
            this._engine = engine;
        }

        public IImage Apply(IImage input)
        {
            if (!this._ready)
                throw new NotReadyException();

            if (!(input.Width == this._other.Width && input.Height == this._other.Height))
                throw new BadConfigurationException("input and configured image have different dimensions");

            IImage output = this._engine.CreateImage(input.Width, input.Height);

            Parallel.For(0, input.Width, (int x) => 
            {
                Parallel.For(0, input.Height, (int y) => 
                {
                    IColor cc = input.GetPixel(x, y);
                    IColor oc = this._other.GetPixel(x, y);

                    int nr = DivClamp(oc.R, cc.R);
                    int ng = DivClamp(oc.G, cc.G);
                    int nb = DivClamp(oc.B, cc.B);

                    output.SetPixel(x, y, this._engine.CreateColor(nr, ng, nb, this._engine.MaxValue));
                });
            });

            return output;
        }

        private int DivClamp(int c0, int c1)
        {
            double d0 = this._engine.ScaleValueToFractional(c0);
            double d1 = this._engine.ScaleValueToFractional(c1);
            if (c1 == 0)
                return c1;

            double result = d0 / d1;
            
            int value = this._engine.ClampedScaledFromFractional(result);
            return value;
        }

        public IFilter Initialize()
        {
            this._other = this._pluginConfigurator.GetPluginConfiguration();
            this._ready = true;
            return this;
        }
    }
}
