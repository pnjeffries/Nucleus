using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nucleus.WPF
{
    /// <summary>
    /// Interaction logic for Bool6DFieldControl.xaml
    /// </summary>
    public partial class Bool6DFieldControl : FieldControl, INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Event raised when a property of this object is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise a PropertyChanged event for the specified property name
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Boolean X component
        /// </summary>
        public bool X
        {
            get
            {
                if (Value != null && Value is Bool6D)
                    return ((Bool6D)Value).X;
                else return false;
            }
            set
            {
                if (Value != null && Value is Bool6D)
                    Value = ((Bool6D)Value).WithX(value);
                else
                    Value = new Bool6D(value, false, false, false, false, false);
                NotifyPropertyChanged("X");
            }
        }

        /// <summary>
        /// Boolean Y component
        /// </summary>
        public bool Y
        {
            get
            {
                if (Value != null && Value is Bool6D)
                    return ((Bool6D)Value).Y;
                else return false;
            }
            set
            {
                if (Value != null && Value is Bool6D)
                    Value = ((Bool6D)Value).WithY(value);
                else
                    Value = new Bool6D(false, value, false, false, false, false);
                NotifyPropertyChanged("Y");
            }
        }

        /// <summary>
        /// Boolean Z component
        /// </summary>
        public bool Z
        {
            get
            {
                if (Value != null && Value is Bool6D)
                    return ((Bool6D)Value).Z;
                else return false;
            }
            set
            {
                if (Value != null && Value is Bool6D)
                    Value = ((Bool6D)Value).WithZ(value);
                else
                    Value = new Bool6D(false, false, value, false, false, false);
                NotifyPropertyChanged("Z");
            }
        }

        /// <summary>
        /// Boolean XX component
        /// </summary>
        public bool XX
        {
            get
            {
                if (Value != null && Value is Bool6D)
                    return ((Bool6D)Value).XX;
                else return false;
            }
            set
            {
                if (Value != null && Value is Bool6D)
                    Value = ((Bool6D)Value).WithXX(value);
                else
                    Value = new Bool6D(false, false, false, value, false, false);
                NotifyPropertyChanged("XX");
            }
        }

        /// <summary>
        /// Boolean YY component
        /// </summary>
        public bool YY
        {
            get
            {
                if (Value != null && Value is Bool6D)
                    return ((Bool6D)Value).YY;
                else return false;
            }
            set
            {
                if (Value != null && Value is Bool6D)
                    Value = ((Bool6D)Value).WithYY(value);
                else
                    Value = new Bool6D(false, false, false, false, value, false);
            }
        }

        /// <summary>
        /// Boolean ZZ component
        /// </summary>
        public bool ZZ
        {
            get
            {
                if (Value != null && Value is Bool6D)
                    return ((Bool6D)Value).ZZ;
                else return false;
            }
            set
            {
                if (Value != null && Value is Bool6D)
                    Value = ((Bool6D)Value).WithZZ(value);
                else
                    Value = new Bool6D(false, false, false, false, false, value);
            }
        }

        #endregion

        #region Constructors

        public Bool6DFieldControl()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;
        }

        #endregion

        #region Methods

        protected override void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnValueChanged(e);
            NotifyPropertyChanged("X");
            NotifyPropertyChanged("Y");
            NotifyPropertyChanged("Z");
            NotifyPropertyChanged("XX");
            NotifyPropertyChanged("YY");
            NotifyPropertyChanged("ZZ");
        }

        #endregion
    }
}
