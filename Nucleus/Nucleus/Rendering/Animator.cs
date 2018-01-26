using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Manager class which applies animations to objects
    /// </summary>
    public class Animator
    {
        #region Properties

        /// <summary>
        /// Private backing field for Animations property
        /// </summary>
        private IList<Animation> _Animations = new List<Animation>();

        /// <summary>
        /// The current set of active animations
        /// </summary>
        public IList<Animation> Animations
        {
            get { return _Animations; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an animation to the purview of this animator
        /// </summary>
        /// <param name="animation"></param>
        public void AddAnimation(Animation animation)
        {
            Animations.Add(animation);
        }

        /// <summary>
        /// Advance and apply all animations
        /// </summary>
        /// <param name="dt"></param>
        public void Animate(double dt)
        {
            for (int i = Animations.Count - 1; i >= 0; i--)
            {
                Animation anim = Animations[i];
                anim.Advance(dt);
                anim.Apply();
                if (anim.Finished) Animations.RemoveAt(i);
            }
        }

        /// <summary>
        /// Finish all animations by advancing them to their end state
        /// </summary>
        public void FinishAll()
        {
            for (int i = Animations.Count - 1; i >= 0; i--)
            {
                Animation anim = Animations[i];
                anim.Finish();
                anim.Apply();
                Animations.RemoveAt(i);
            }
        }

        #endregion
    }
}
