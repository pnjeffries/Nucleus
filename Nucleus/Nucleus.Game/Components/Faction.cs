using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A player or AI-controlled faction
    /// </summary>
    [Serializable]
    public class Faction : Named, IElementDataComponent
    {
        #region Constructors

        /// <summary>
        /// Initialise a blank faction
        /// </summary>
        public Faction() { }

        /// <summary>
        /// Initialise a faction with the specified name
        /// </summary>
        /// <param name="name"></param>
        public Faction(string name)
        {
            Name = name;
        }

        #endregion

        #region Method

        /// <summary>
        /// Is this faction an enemy of the specified other faction?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsEnemy(Faction other)
        {
            // Currently, all factions are all enemies of each other:
            return other != null && other != this;
        }

        /// <summary>
        /// Is this faction an ally of the specified other faction?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAlly(Faction other)
        {
            // Currently, all factions are only allies of themselves:
            return other == this;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Does elementB belong to a faction which is an enemy of the faction of elementA?
        /// </summary>
        /// <param name="elementA"></param>
        /// <param name="elementB"></param>
        /// <returns></returns>
        public static bool AreEnemies(Element elementA, Element elementB)
        {
            Faction factionA = elementA?.GetData<Faction>();
            Faction factionB = elementB?.GetData<Faction>();
            return factionA != null && factionA.IsEnemy(factionB);
        }

        #endregion

    }
}
