using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dwatch.checker.Models.Options
{
    public record DeviceOptions
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? IP { get; set; }
    }
}
