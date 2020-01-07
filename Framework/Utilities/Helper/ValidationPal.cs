using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// Holds information about fields that are required to be in a config based on other fields
    /// </summary>
    public class ValidationPal
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not required fields are present
        /// </summary>
        public bool AreFieldsPresent { get; set; }

        /// <summary>
        /// Gets or sets the fields which are missing
        /// </summary>
        public string MissingField { get; set; }

    }
}
