using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nucleus.WPF
{
    /// <summary>
    /// Command to invoke delegate methods on other objects
    /// </summary>
    public class InvokeMethodCommand : ICommand
    {
        #region Events

        /// <summary>
        /// Event raised when the value returned by the CanExecute delegate is changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Fields

        /// <summary>
        /// The action delegate to be invoked when this command is executed
        /// </summary>
        private readonly Action _Execute;

        /// <summary>
        /// A predicate delegate to be invoked to check whether this command can be executed
        /// </summary>
        public readonly Predicate<object> _CanExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new command
        /// </summary>
        /// <param name="execute">The method to be executed</param>
        /// <param name="canExecute">Optional.  A method which returns true or false depending on whether the method may be executed</param>
        public InvokeMethodCommand(Action execute, Predicate<object> canExecute = null)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        /// <summary>
        /// Create a new command from reflection information
        /// </summary>
        /// <param name="target"></param>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public InvokeMethodCommand(object target, MethodInfo execute, MethodInfo canExecute = null)
        {
            _Execute = (Action)Delegate.CreateDelegate(typeof(Action), target, execute);
            if (canExecute != null) _CanExecute = (Predicate<object>)Delegate.CreateDelegate(typeof(Predicate<object>), target, canExecute);
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _Execute();
        }

        #endregion
    }
}
