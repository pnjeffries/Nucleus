using Nucleus.Geometry;
using Nucleus.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A game state which features modal interface elements (i.e. message boxes,
    /// inventory screens, etc.)
    /// </summary>
    [Serializable]
    public abstract class ModalState : GameState
    {
        private ModalWindowCollection _Modals = new ModalWindowCollection();

        /// <summary>
        /// The collection of modal windows currently open within this state.
        /// Do not modify this collection directly, use OpenModal and CloseModal
        /// functions instead.
        /// </summary>
        public ModalWindowCollection Modals
        {
            get { return _Modals; }
        }

        /// <summary>
        /// Get the currently active modal window which has input
        /// focus.
        /// </summary>
        public ModalWindow ActiveModal
        {
            get { return _Modals?.LastOrDefault(); }
        }

        /// <summary>
        /// Is any modal open?
        /// </summary>
        public bool IsModalOpen
        {
            get { return _Modals.Count > 0; }
        }


        /// <summary>
        /// Open a new modal window.  This will become the new active modal.
        /// </summary>
        /// <param name="modal"></param>
        public void OpenModal(ModalWindow modal)
        {
            Modals.Add(modal);
            NotifyPropertyChanged(nameof(ActiveModal));
        }

        /// <summary>
        /// Close the specified modal window, or if no window
        /// is specified, the currently active one.
        /// Focus will pass to the next topmost modal.
        /// </summary>
        /// <param name="modal"></param>
        public void CloseModal(ModalWindow modal = null)
        {
            if (modal == null) modal = ActiveModal;
            if (Modals.Contains(modal)) Modals.Remove(modal);
            NotifyPropertyChanged(nameof(ActiveModal));
        }

        /// <summary>
        /// Close all currently open modals
        /// </summary>
        public void CloseAllModals()
        {
            Modals.Clear();
            NotifyPropertyChanged(nameof(ActiveModal));
        }

        public override void InputRelease(InputFunction input, Vector direction)
        {
            base.InputRelease(input, direction);
            CloseModal();
        }
    }
}
