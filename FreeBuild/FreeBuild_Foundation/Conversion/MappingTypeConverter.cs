using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Converter which provides a customisable property mapping from one type to another
    /// </summary>
    [Serializable]
    public class MappingTypeConverter : NotifyPropertyChangedBase, ITypeConverter
    {

        #region Properties

        /// <summary>
        /// Private backing member variable for TypeA property
        /// </summary>
        private Type _TypeA;

        /// <summary>
        /// The first type to map to/from
        /// </summary>
        public Type TypeA
        {
            get { return _TypeA; }
            set
            {
                _TypeA = value;
                NotifyPropertyChanged("TypeA");
            }
        }

        /// <summary>
        /// Private backing member variable for TypeB property
        /// </summary>
        private Type _TypeB;

        /// <summary>
        /// The second type to map to/from
        /// </summary>
        public Type TypeB
        {
            get { return _TypeB; }
            set
            {
                _TypeB = value;
                NotifyPropertyChanged("TypeB");
            }
        }



        /// <summary>
        /// The collection of property mappings that describe how the properties of one type should be converted into another
        /// </summary>
        public ObservableCollection<PropertyMapping> PropertyMap { get; } = new ObservableCollection<PropertyMapping>();

        #endregion

        #region Methods

        public object Convert(object fromObject)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
