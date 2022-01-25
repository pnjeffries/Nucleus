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
        #region constants

        /// <summary>
        /// The score at or below which another faction is considered an enemy
        /// </summary>
        public const double EnemyScore = -100;

        /// <summary>
        /// The score at or above which another faction is considered a friend
        /// </summary>
        public const double AllyScore = 100;

        #endregion

        private IDictionary<string, double> _Relationships = new Dictionary<string, double>();

        /// <summary>
        /// The relationship scores for other factions in relation to this one
        /// </summary>
        public IDictionary<string, double> Relationships
        {
            get { return _Relationships; }
        }

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

        /// <summary>
        /// Initialise a faction with a name and a list of enemy names
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enemyNames"></param>
        public Faction(string name, params string[] enemyNames) : this(name)
        {
            foreach (var enemyName in enemyNames) Relationships[enemyName] = EnemyScore*2;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Is this faction an enemy of the specified other faction?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsEnemy(Faction other)
        {
            // Currently, all factions are all enemies of each other:
            return other != null && other != this && GetRelationshipScore(other) <= EnemyScore;
        }

        /// <summary>
        /// Is this faction an ally of the specified other faction?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAlly(Faction other)
        {
            // Currently, all factions are only allies of themselves:
            return other == this || GetRelationshipScore(other) >= AllyScore;
        }

        /// <summary>
        /// Get the relationship score for another faction
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double GetRelationshipScore(Faction other)
        {
            if (other == null || !Relationships.ContainsKey(other.Name)) return 0;
            return Relationships[other.Name];
        }

        /// <summary>
        /// Set the relationship score for another faction
        /// </summary>
        /// <param name="other"></param>
        /// <param name="score"></param>
        public void SetRelationShipScore(Faction other, double score)
        {
            Relationships[other.Name] = score;
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
