using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class JwtConfiguration
    {
        public string GetSection { get; set; } = "JwtSettings";

        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }

        public int expires { get; set; }

        public string SecretKey { get; set; }

    }
}
