using Huevy.Lib.IO;
using System.Collections.Generic;

namespace Huetiful.Lib
{
    public class AssemblyConfig : AssemblyConfigRepository<AssemblyConfig>
    {
        public AssemblyConfig()
        {
            this.ApiKeys = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the api keys. The key is the MAC address of the bridge, and the value is the 'user name'.
        /// </summary>
        public IDictionary<string, string> ApiKeys { get; set; }
    }
}
