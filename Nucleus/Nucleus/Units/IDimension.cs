using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Units
{
    public interface IDimension
    {
    }

    public static class Dimension
    {
        public abstract class Length : IDimension { }
    }
}
