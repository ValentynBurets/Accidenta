using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.Options
{
    public class SerilogSettings
    {
        public string MinimumLevel { get; set; } = "Information";
        public string? OutputTemplate { get; set; }
        public string? FilePath { get; set; }
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = true;
    }
}
