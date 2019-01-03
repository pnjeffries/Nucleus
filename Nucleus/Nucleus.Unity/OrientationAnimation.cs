using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;
using Nucleus.Maths;
using Nucleus.Rendering;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Animation to rotate a Unity gameobject about the vertical axis
    /// </summary>
    public class OrientationAnimation : Animation<GameObject, Angle>
    {
        public OrientationAnimation()
        {
        }

        public OrientationAnimation(GameObject target, Angle startState, Angle endState, double duration, Interpolation tweening) : base(target, startState, endState, duration, false)
        {
            Tweening = tweening;
            CustomInterpolationFunction =
                delegate (Angle v1, Angle v2, double t, Interpolation i)
                {
                    return i.Interpolate(v1, v2, t);
                };
        }

        public override void Apply()
        {
            Target.transform.rotation = CurrentState.ToUnityQuaternion();
        }
    }
}
