using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A collection of cell masks
    /// </summary>
    [Serializable]
    public class CellMaskCollection : ObservableCollection<CellMask>
    {
        /// <summary>
        /// Filter the specified bitField through the masks
        /// </summary>
        /// <typeparam name="TMapCell"></typeparam>
        /// <returns></returns>
        public void Apply<TMapCell>(ICellMap<bool> bitField, ICellMap<TMapCell> map)
            where TMapCell : IMapCell
        {
            foreach (var mask in this)
            {
                mask.Apply(bitField, map);
            }
        }
    }
}
