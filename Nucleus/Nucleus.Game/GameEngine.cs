using Nucleus.Base;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A core engine for games and realtime simulations
    /// </summary>
    public class GameEngine : NotifyPropertyChangedBase
    {
        #region Fields

        private DateTime _LastUpdate = DateTime.UtcNow;

        #endregion

        #region Properties

        /// <summary>
        /// Private backing field for Instance
        /// </summary>
        private static GameEngine _Instance = new GameEngine();

        /// <summary>
        /// The instance of the game engine
        /// </summary>
        public static GameEngine Instance { get { return _Instance; } }

        /// <summary>
        /// The sub-manager which handles user input
        /// </summary>
        public InputManager Input { get; } = new InputManager();

        /// <summary>
        /// Private backing member variable for the State property
        /// </summary>
        private GameState _State = null;

        /// <summary>
        /// The current state
        /// </summary>
        public GameState State
        {
            get { return _State; }
            set
            {
                _State = value;
                NotifyPropertyChanged("State");
            }
        }

        /// <summary>
        /// The special effects supervisor
        /// </summary>
        public SFXSupervisor SFX { get; } = new SFXSupervisor();

        /// <summary>
        /// Private backing member variable for the Module property
        /// </summary>
        private GameModule _Module;

        /// <summary>
        /// The currently loaded module
        /// </summary>
        public GameModule Module
        {
            get { return _Module; }
            private set { ChangeProperty(ref _Module, value, "Module"); }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Perform engine initialisation.
        /// This should be done after a module has been loaded.
        /// </summary>
        public virtual void StartUp()
        {
            State = Module?.StartingState();

            _LastUpdate = DateTime.UtcNow;

            State?.StartUp();
        }

        /// <summary>
        /// Called every frame update
        /// </summary>
        public virtual void Update()
        {
            DateTime now = DateTime.UtcNow;

            var info = new UpdateInfo((now - _LastUpdate).TotalSeconds);

            State.Update(info);

            _LastUpdate = now;
        }

        /// <summary>
        /// Load a module into the engine
        /// </summary>
        /// <param name="module"></param>
        public void LoadModule(GameModule module)
        {
            Module = module;
        }

        

        #endregion
    }
}
