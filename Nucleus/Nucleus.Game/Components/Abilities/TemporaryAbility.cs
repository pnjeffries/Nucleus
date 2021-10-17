using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class for temporary abilities that are removed 
    /// </summary>
    [Serializable]
    public abstract class TemporaryAbility : Ability, IStartOfTurn, IEndOfTurn
    {
        private bool _DeleteAtEnd = false;

        protected override bool IsDisabled(TurnContext context)
        {
            return false; //TODO: Make disableable by status debuffs?
        }

        public void StartOfTurn(TurnContext context)
        {
            //_DeleteAtEnd = true; //TEMP
        }

        public void EndOfTurn(TurnContext context)
        {
            if (_DeleteAtEnd) Delete();
        }

        public override void PopulateActions(TurnContext context)
        {
            if (!_DeleteAtEnd)
            {
                base.PopulateActions(context);
                _DeleteAtEnd = true;
            }
            else Delete();
        }
    }
}
