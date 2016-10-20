using FreeBuild.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{
    /// <summary>
    /// Class that contains data on the context of a conversion to or from a robot data type
    /// </summary>
    public class RobotConversionContext : ConversionContext
    {
        /// <summary>
        /// The current conversion options set
        /// </summary>
        public RobotConversionOptions Options { get; set; } = new RobotConversionOptions();
    }
}
