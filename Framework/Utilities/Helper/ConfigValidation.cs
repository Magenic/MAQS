using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// Elements of config files which need to be validated on load
    /// </summary>
    public class ConfigValidation
    {
        /// <summary>
        /// Gets or sets the list of required fields for a config
        /// </summary>
        public List<string> RequiredFields { get; set; }
    }
}
