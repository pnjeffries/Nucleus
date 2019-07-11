using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    [Serializable]
    public class AnimationBinding
    {
        /// <summary>
        /// Private backing member variable for the PropertyName property
        /// </summary>
        private string _PropertyName;

        /// <summary>
        /// The name of the property to be watched for changes
        /// </summary>
        public string PropertyName
        {
            get { return _PropertyName; }
            set { _PropertyName = value; }
        }

        /// <summary>
        /// Private backing member variable for the Animation property
        /// </summary>
        private Animation _Animation = null;

        /// <summary>
        /// The animation to be played to transition changes in the property value
        /// </summary>
        public Animation Animation
        {
            get { return _Animation; }
            set { _Animation = value; }
        }
    }
}
