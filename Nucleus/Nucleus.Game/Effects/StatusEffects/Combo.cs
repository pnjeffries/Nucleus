using Nucleus.Base;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Status effect which describes the state of having scored multiple hits
    /// in quick succession
    /// </summary>
    [Serializable]
    public class Combo : StatusEffect, IFastDuplicatable, ICritChanceModifier
    {
        private int _Hits = 1;

        /// <summary>
        /// The number of hits in this combo
        /// </summary>
        public int Hits
        {
            get { return _Hits; }
            set 
            { 
                ChangeProperty(ref _Hits, value);
                NotifyPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Description of the combo
        /// </summary>
        public string Description
        {
            get
            {
                //if (Hits < 2) return "";
                return GetType().Name.ToUpper() + "×" + Hits + "!";
            }
        }

        public Combo() : base(2) { }

        public Combo(Combo other) : base(other.TimeRemaining)
        {
            _Hits = other.Hits;
        }


        public override bool Apply(IActionLog log, EffectContext context)
        {
            return false;
        }

        public override void Merge(IStatusEffect other)
        {
            TimeRemaining = Math.Max(TimeRemaining, other.TimeRemaining);
            if (other is Combo oC)
            {
                Hits += oC.Hits; 
            }
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new Combo(this);
        }

        public double ModifyCritChance(double critChance, IActionLog log, EffectContext context)
        {
            // + 10% per combo level
            return critChance + 0.1 * Hits;
        }
    }
}
