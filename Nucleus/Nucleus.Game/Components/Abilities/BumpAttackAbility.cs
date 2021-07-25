using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Data component which confers the ability to bump-attack
    /// </summary>
    [Serializable]
    public class BumpAttackAbility : Ability
    {
        private double _BaseDamage = 1;

        /// <summary>
        /// The base damage of bump attacks
        /// </summary>
        public double BaseDamage { get { return _BaseDamage; } set { _BaseDamage = value; } }

        private double _BaseKnockback = 1;

        /// <summary>
        /// The base knockback of bump attacks
        /// </summary>
        public double BaseKnockback { get { return _BaseKnockback; } set { _BaseDamage = value; } }


        public BumpAttackAbility() { }

        public BumpAttackAbility(double damage, double knockback)
        {
            _BaseDamage = damage;
            _BaseKnockback = knockback;
        }


        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            // Bump attack:
            MapData mD = context.Element?.GetData<MapData>();
            if (mD != null && mD.MapCell != null)
            {
                IList<MapCell> adjacent = context.Stage?.Map?.AdjacentCells(mD.MapCell.Index);
                //TODO: Diagonal?
                foreach (var cell in adjacent)
                {
                    Vector direction = cell.Position - mD.MapCell.Position;
                    Element target = cell.Contents.FirstWithDataComponent<Faction>();//context.Element.GetData<MapCellCollider>()?.Blocker(cell);
                    if (target != null)
                    {
                        if (context.Element?.GetData<Faction>()?.IsEnemy(target?.GetData<Faction>()) ?? false)
                        {
                            // Only allow bump attacks on elements of an opposing faction?
                            addTo.Actions.Add(new BumpAttackAction(target, cell, direction, BaseDamage, BaseKnockback));
                        }
                    }
                }
            }
        }
    }
}
