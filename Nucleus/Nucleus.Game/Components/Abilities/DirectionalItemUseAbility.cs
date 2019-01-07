using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    public class DirectionalItemUseAbility : TemporaryAbility
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the ActionFactory property
        /// </summary>
        private ActionFactory _ActionFactory;

        /// <summary>
        /// The ActionFactory used to generate actions
        /// </summary>
        public ActionFactory ActionFactory
        {
            get { return _ActionFactory; }
            set { _ActionFactory = value; }
        }

        #endregion

        #region Constructor

        public DirectionalItemUseAbility(ActionFactory actionFactory)
        {
            ActionFactory = actionFactory;
        }

        #endregion

        #region Methods

        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            if (ActionFactory != null) ActionFactory.GenerateActions(context, addTo);
            //TODO: Always allow cancelling?
        }

        #endregion
    }
}
