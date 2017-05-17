using FreeBuild.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Undo
{
    /// <summary>
    /// An undo state which can be used to revert a property value change
    /// </summary>
    public class PropertyUndoState : UndoState
    {
        #region Properties

        public override bool IsValid
        {
            get
            {
                return Target != null;
            }
        }

        /// <summary>
        /// The object whose property state is being stored
        /// </summary>
        public object Target { get; private set; }

        /// <summary>
        /// The name of the property the state of which is being stored
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The value of the property to be stored
        /// </summary>
        public object PropertyValue { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new undo state
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public PropertyUndoState(object target, string propertyName, object propertyValue)
        {
            Target = target;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        /// <summary>
        /// Create a new undo state, automatically extracting the current value of the property
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        public PropertyUndoState(object target, string propertyName)
        {
            Target = target;
            PropertyName = propertyName;
            SetValue();
        }

        /// <summary>
        /// Create a new undo state based on an extended property changed event
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        public PropertyUndoState(object target, PropertyChangedEventArgsExtended args)
        {
            Target = target;
            PropertyName = args.PropertyName;
            PropertyValue = args.OldValue;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the PropertyValue property by extracting it via reflection based on the current
        /// Target and PropertyName
        /// </summary>
        private void SetValue()
        {
            if (Target != null)
            {
                Type targetType = Target.GetType();
                PropertyInfo pI = targetType.GetProperty(PropertyName);
                if (pI != null)
                {
                    PropertyValue = pI.GetValue(Target, null);
                }
            }
        }

        public override void Restore()
        {
            if (Target != null)
            {
                Type targetType = Target.GetType();
                PropertyInfo pI = targetType.GetProperty(PropertyName);
                if (pI != null)
                {
                    pI.SetValue(Target, PropertyValue, null);
                }
            }
        }

        public override UndoState GenerateRedo()
        {
            if (Target != null)
            {
                return new PropertyUndoState(Target, PropertyName);
            }
            return null;
        }

        #endregion
    }
}
