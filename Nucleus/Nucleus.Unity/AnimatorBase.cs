using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Base class for Unity script components that apply a Nucleus tweening
    /// animation.
    /// </summary>
    /// <typeparam name="TAnimation"></typeparam>
    public abstract class AnimatorBase<TAnimation> : MonoBehaviour
        where TAnimation : Nucleus.Rendering.Animation
    {
        /// <summary>
        /// A factor applied to the speed of animations
        /// </summary>
        public double SpeedFactor = 1.0;

        private TAnimation _Animation = null;

        /// <summary>
        /// Get or set the current animation
        /// </summary>
        public TAnimation Animation
        {
            get { return _Animation; }
            set { _Animation = value; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Animation != null)
            {
                Animation.Advance(Time.deltaTime * SpeedFactor);
                Animation.Apply();

                if (Animation.Finished) Animation = null;
            }
        }
    }
}
