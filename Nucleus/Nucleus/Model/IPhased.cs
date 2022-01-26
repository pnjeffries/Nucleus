using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleus.Model
{
    /// <summary>
    /// Interface for objects which can be assigned to a particular development phase or set of phases.
    /// </summary>
    public interface IPhased
    {
        #region Properties

        /// <summary>
        /// The development phases which this geometry should be included in.
        /// A value of -1 in the phase list indicates the list is unset.
        /// </summary>
        IList<int> Phases { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Add this geometry to a given development phase by tagging it with the phase number.
        /// </summary>
        /// <param name="phase"></param>
        /// <returns>True if the list of phases for this block was changed</returns>
        bool AddToPhase(int phase);

        /// <summary>
        /// Remove this geometry from a given development phase by deleting the phase number tag on this geometry.
        /// </summary>
        /// <param name="phase"></param>
        /// <returns>True if the list of phases for this block was changed</returns>
        bool RemoveFromPhase(int phase);

        /// <summary>
        /// Set this geometry to be included in a given set of phases.
        /// </summary>
        /// <param name="phases"></param>
        /// <returns>True if the list of phases for this block was changed</returns>
        bool SetPhases(params int[] phases);

        #endregion
    }
}
