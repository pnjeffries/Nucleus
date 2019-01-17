using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An abstract base class for documents which act as a save file
    /// for a game state
    /// </summary>
    [Serializable]
    public abstract class GameSaveFile : Document
    {
        #region Methods

        /// <summary>
        /// Retrieve the game state (if any) stored in this save file
        /// </summary>
        /// <returns></returns>
        public abstract GameState RetrieveState();

        #endregion

        #region Static Methods

        /// <summary>
        /// Load a ModelDocument from a file stored in binary format
        /// </summary>
        /// <param name="filePath">The path of the file to be loaded.</param>
        /// <returns>The loaded document, if a document could indeed be loaded.
        /// Else, null.</returns>
        public static GameSaveFile Load(FilePath filePath)
        {
            return Load<GameSaveFile>(filePath);
        }

        #endregion
    }

    /// <summary>
    /// An abstract base class for game save files which store a
    /// particular type of game state
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    [Serializable]
    public abstract class GameSaveFile<TState> : GameSaveFile
        where TState : GameState
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the State property
        /// </summary>
        private TState _State;

        /// <summary>
        /// The state which is stored in this file
        /// </summary>
        public TState State
        {
            get { return _State; }
            set { _State = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve the game state (if any) stored in this save file
        /// </summary>
        /// <returns></returns>
        public override GameState RetrieveState()
        {
            return State;
        }

        #endregion
    }

}
