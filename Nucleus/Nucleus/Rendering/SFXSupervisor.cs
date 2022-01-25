using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Supervisor class responsible for cacheing special effects triggers
    /// </summary>
    [Serializable]
    public class SFXSupervisor
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Triggers property
        /// </summary>
        private IList<SFXTrigger> _Triggers = new List<SFXTrigger>();

        /// <summary>
        /// The 'to do' list for SFX triggers
        /// </summary>
        public IList<SFXTrigger> Triggers
        {
            get { return _Triggers; }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear the list of SFX Triggers
        /// </summary>
        public void Clear()
        {
            _Triggers.Clear();
        }

        /// <summary>
        /// Trigger a special effect
        /// </summary>
        /// <param name="keyword">The keyword denoting the effect to trigger</param>
        /// <returns></returns>
        public SFXTrigger Trigger(string keyword)
        {
            var result = new SFXTrigger(keyword);
            _Triggers.Add(result);
            return result;
        }

        /// <summary>
        /// Trigger a special effect
        /// </summary>
        /// <param name="keyword">The keyword denoting the effect to trigger</param>
        /// <param name="position">The (starting) position of the effect</param>
        /// <returns></returns>
        public SFXTrigger Trigger(string keyword, Vector position)
        {
            var result = new SFXTrigger(keyword, position);
            _Triggers.Add(result);
            return result;
        }

        /// <summary>
        /// Trigger a special effect
        /// </summary>
        /// <param name="keyword">The keyword denoting the effect to trigger</param>
        /// <param name="position">The (starting) position of the effect</param>
        /// <param name="direction">The direction or path of the effect</param>
        /// <returns></returns>
        public SFXTrigger Trigger(string keyword, Vector position, Vector direction)
        {
            var result = new SFXTrigger(keyword, position, direction);
            _Triggers.Add(result);
            return result;
        }

        /// <summary>
        /// Trigger a special effect
        /// </summary>
        /// <param name="keyword">The keyword denoting the effect to trigger</param>
        /// <param name="context">The data context to be used to bind effect properties to</param>
        /// <returns></returns>
        public SFXTrigger Trigger(string keyword, object context)
        {
            var result = new SFXTrigger(keyword, context);
            _Triggers.Add(result);
            return result;
        }

        #endregion
    }
}
