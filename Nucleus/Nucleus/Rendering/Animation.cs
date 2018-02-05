using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Base class for Animations - tweened transitions between multiple
    /// states that happen over time.
    /// </summary>
    [Serializable]
    public abstract class Animation : Unique, IDuplicatable
    {
        #region Properties

        /// <summary>
        /// Private backing field for Time property
        /// </summary>
        private double _Time = 0;

        /// <summary>
        /// The current time of the animation
        /// </summary>
        public double Time
        {
            get { return _Time; }
            set
            {
                _Time = value;
                if (_Time > Duration)
                {
                    if (Looping) _Time -= Duration;
                    else _Time = Duration;
                }
            }
        }

        /// <summary>
        /// Has this animation finished playing?
        /// </summary>
        public bool Finished
        {
            get { return (!Looping && _Time == Duration); }
        }

        /// <summary>
        /// Private backing field for Looping property
        /// </summary>
        private bool _Looping = false;

        /// <summary>
        /// Get or set whether this animation will loop
        /// </summary>
        public bool Looping
        {
            get { return _Looping; }
            set { _Looping = value; }
        }

        /// <summary>
        /// Private backing field for Tweening property
        /// </summary>
        private Interpolation _Tweening = Interpolation.LINEAR;

        /// <summary>
        /// The tweening algorithm to be used by this animation
        /// </summary>
        public Interpolation Tweening
        {
            get { return _Tweening; }
            set { _Tweening = value; }
        }

        /// <summary>
        /// The duration of this animation
        /// </summary>
        public abstract double Duration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Progress the animation by the specified timestep
        /// </summary>
        /// <param name="dT">The timestep</param>
        public void Advance(double dT)
        {
            Time += dT;
        }

        /// <summary>
        /// End the animation.
        /// </summary>
        /// <returns></returns>
        public void Finish()
        {
            Time = Duration;
        }

        /// <summary>
        /// Restart the animation
        /// </summary>
        public void Reset()
        {
            Time = 0;
        }

        /// <summary>
        /// Apply this animation to the target object
        /// </summary>
        public abstract void Apply();

        #endregion
    }

    /// <summary>
    /// Generic base class for Animations - tweened transitions between multiple
    /// states that happen over time.
    /// </summary>
    /// <typeparam name="TTarget">The type of the target to which the animation is applied</typeparam>
    /// <typeparam name="TState">The type of the state value which will be modified throughout this animation</typeparam>
    public abstract class Animation<TTarget, TState> : Animation
    {
        #region Properties

        /// <summary>
        /// Private backing field for Target property
        /// </summary>
        private TTarget _Target;

        /// <summary>
        /// The object being animated
        /// </summary>
        public TTarget Target
        {
            get { return _Target; }
            set { _Target = value; }
        }

        /// <summary>
        /// Private backing field for States property
        /// </summary>
        private SortedList<double, TState> _States = new SortedList<double, TState>();

        /// <summary>
        /// Get the states of this animation, keyed by the time at which they occur
        /// </summary>
        public SortedList<double, TState> States
        {
            get { return _States; }
        }

        /// <summary>
        /// The state value at the start of this animation
        /// </summary>
        public TState InitialState
        {
            get
            {
                return _States.Values.FirstOrDefault();
            }
        }

        /// <summary>
        /// The (possibly interpolated) state at the current Time
        /// </summary>
        public TState CurrentState
        {
            get
            {
                return _States.InterpolatedValueAt(Time, Tweening);
            }
        }

        /// <summary>
        /// The duration of this animation
        /// </summary>
        public override double Duration
        {
            get
            {
                return _States.Keys.LastOrDefault();
            }
        }

        #endregion

        #region Constructors

        public Animation()
        {

        }

        public Animation(TTarget target, TState startState, TState endState, double duration, bool looping = false)
        {
            Target = target;
            Initialise(startState, endState, duration);
            Looping = looping;
        }

        /// <summary>
        /// Initialise the stored start and end states of this animation
        /// </summary>
        /// <param name="startState"></param>
        /// <param name="endState"></param>
        /// <param name="duration"></param>
        protected void Initialise(TState startState, TState endState, double duration)
        {
            States.Add(0, startState);
            States.Add(duration, endState);
        }

        #endregion
    }
}
