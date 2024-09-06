using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dwatch.checker.Models.Options
{
    public record WatcherOptions
    {
        public string HealthcheckUrl { get; set; } = "http://dwatch-server:8080";
        public int CheckDelay { get; set; } = 15;
    }
}
