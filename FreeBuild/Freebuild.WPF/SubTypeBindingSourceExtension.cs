using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Freebuild.WPF
{
    public class SubTypeBindingSourceExtension : MarkupExtension
    {
        #region Properties

        /// <summary>
        /// Private backing field for the Type property
        /// </summary>
        private Type _Type;

        /// <summary>
        /// The type to get subtypes for
        /// </summary>
        public Type Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SubTypeBindingSourceExtension() { }

        /// <summary>
        /// Initialises a new SubTypeBindingSourceExtension with the specified type
        /// </summary>
        /// <param name="type"></param>
        public SubTypeBindingSourceExtension(Type type)
        {
            Type = type;
        }

        #endregion

        #region Methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_Type == null) throw new InvalidOperationException("The Type is not specified.");

            return _Type.GetSubTypes(false);
        }

        #endregion
    }
}
