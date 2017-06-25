using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Nucleus.WPF
{
    /// <summary>
    /// A markup extension to retrieve enum values.
    /// Based on http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
    /// Use example:
    /// ItemsSource="{Binding Source={fb:EnumBindingSource {x:Type [ !!ENUM TYPE!! ]}} }"
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Private backing field for EnumType property
        /// </summary>
        private Type _EnumType;

        /// <summary>
        /// The type of enum to extract the values for
        /// </summary>
        public Type EnumType
        {
            get { return _EnumType; }
            set
            {
                if (value != null)
                {
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum) throw new ArgumentException("Type must be an Enum.");
                }
                _EnumType = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EnumBindingSourceExtension() { }

        /// <summary>
        /// Initialises an EnumBindingSourceExtension with the specified enum type
        /// </summary>
        /// <param name="enumType"></param>
        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        #endregion

        #region Methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            

            Type actualType = Nullable.GetUnderlyingType(_EnumType) ?? _EnumType;
            Array values = Enum.GetValues(actualType);

            if (actualType == _EnumType) return values;
            else
            {
                Array tempArray = Array.CreateInstance(actualType, values.Length + 1);
                values.CopyTo(tempArray, 1);
                return tempArray;
            }
        }

        #endregion
    }
}
