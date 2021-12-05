using AAImageFilter;
using AAImageFilter.Interfaces;
using AAImageFilter.Filters;

namespace NET6ImageFilter
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();         
            List<IFilter> filters = new(){
                new Invert(),
                new Threshold(new WinformIntConfigurator("Threshold: "))
            };
            Application.Run(new MainForm(filters));
        }
    }
}