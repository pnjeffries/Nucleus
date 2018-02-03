using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// An animation which animates a property specified by a path string.
    /// Uses reflection and therefore may be less performant than other animation types.
    /// </summary>
    public class PropertyAnimation : Animation<object, object>
    {
        #region Properties

        /// <summary>
        /// Private backing field for TargetPath property
        /// </summary>
        private string _TargetPath;

        /// <summary>
        /// The path of the property to be animated on the
        /// target object.
        /// </summary>
        public string TargetPath
        {
            get { return _TargetPath; }
            set { _TargetPath = value; }
        }

        #endregion

        #region Constructors

        public PropertyAnimation() : base() { }

        /// <summary>
        /// Initialise a new PropertyAnimation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetPath"></param>
        /// <param name="toValue"></param>
        /// <param name="duration"></param>
        public PropertyAnimation(object target, string targetPath, object toValue, double duration) : base()
        {
            Target = target;
            object fromValue = null;
            if (Target != null) fromValue = Target.GetFromPath(targetPath);

            Initialise(fromValue, toValue, duration);
        }

        #endregion

        public override void Apply()
        {
            if (Target != null)
            {
                object currentState = CurrentState;
                Target.SetByPath(TargetPath, currentState);
            }
        }
    }
}
