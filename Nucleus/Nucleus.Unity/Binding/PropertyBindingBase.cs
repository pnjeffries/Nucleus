using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Base class used to implement property binding in Unity
    /// </summary>
    public class PropertyBindingBase : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// The binding manager
        /// </summary>
        private DataBinding _Binding = new DataBinding();

        /// <summary>
        /// Get the DataBinding manager
        /// </summary>
        public DataBinding Binding
        {
            get { return _Binding; }
        }

        /// <summary>
        /// The data context of the binding - the object that
        /// the source data is drawn from.
        /// </summary>
        public object DataContext
        {
            get { return _Binding.DataContext; }
            set { _Binding.DataContext = value; }
        }

        /// <summary>
        /// The path of the property to be bound to
        /// </summary>
        public string Path = null;

        /// <summary>
        /// The format of the binding for text binding
        /// </summary>
        public string StringFormat = "{0:.}";

        #endregion

        #region Methods

        // Use this for initialization
        public void Start()
        {
            DataContext = null; //TODO
            _Binding.Path = Path;
            _Binding.StringFormat = StringFormat;
            RefreshUI();

            /*var field = GetComponent<UnityEngine.UI.InputField>();
            if (field != null)
            {
                // TODO: Register listener to set bound value
                field.onValueChanged.AddListener(delegate { ValueChanged(); });
            }*/
        }

        public void ValueChanged()
        {
            //var field = GetComponent<UnityEngine.UI.InputField>();
            //_Binding.SetBoundValue(field.text);
        }

        // Update is called once per frame
        void Update()
        {
            if (_Binding.BindingRefreshRequired)
            {
                _Binding.RefreshBinding();
            }
            if (_Binding.UIRefreshRequired)
            {
                //Prevent updating if the field is being edited:
                /*var field = GetComponent<InputField>();
                if (field == null || !field.isFocused)
                {
                    _Binding.UIRefreshRequired = false;
                    RefreshUI();
                }*/
            }
        }

        /// <summary>
        /// Refresh the UI with the bound value
        /// </summary>
        public virtual void RefreshUI()
        {
            double value;
            string textValue = _Binding.GetBoundValueString();

            /*
            // Will currently work for input fields and text:
            var field = GetComponent<InputField>();
            if (field != null)
            {
                if (double.TryParse(_Binding.GetBoundValue().ToString(), out value)) // output is a number
                {
                    field.text = string.Format(StringFormat, value.ToString());
                }
                else
                {
                    field.text = string.Format(StringFormat, textValue);
                }

            }
            else
            {
                // Assuming it's text...
                Text text = GetComponent<Text>();
                if (text != null)
                {
                    //text.text = string.Format(StringFormat, textValue);

                    if (double.TryParse(_Binding.GetBoundValue().ToString(), out value)) // output is a number
                    {
                        text.text = string.Format(StringFormat, value.ToString("N2"));
                    }
                    else
                    {
                        text.text = string.Format(StringFormat, textValue);
                    }
                }
            }
            */
        }

        #endregion
    }
}
