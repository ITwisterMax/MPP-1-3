using System;
using System.Windows.Input;

/// <summary>
///     Commands for assembly
/// </summary>
namespace AssemblyBrowserView.Commands
{
    class AssemblyCommands : ICommand
    {
        /// <summary>
        ///     Function which will be executed
        /// </summary>
        private Action<object> ExecuteFunction;

        /// <summary>
        ///     Check if function can be executed
        /// </summary>
        private Func<object, bool> CanExecuteFunction;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="executeFunction">Function which will be executed</param>
        /// <param name="canExecuteFunction">Check if function can be executed</param>
        public AssemblyCommands(Action<object> executeFunction, Func<object, bool> canExecuteFunction = null)
        {
            ExecuteFunction = executeFunction;
            CanExecuteFunction = canExecuteFunction;
        }

        /// <summary>
        ///     On execute event changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        ///     Check if function can be executed
        /// </summary>
        /// 
        /// <param name="param">Executed parameter</param>
        /// 
        /// <returns>Bool</returns>
        public bool CanExecute(object param)
        {
            return (CanExecuteFunction == null) || CanExecuteFunction(param);
        }

        /// <summary>
        ///     Execute current function
        /// </summary>
        /// 
        /// <param name="param">Executed parameter</param>
        public void Execute(object param)
        {
            ExecuteFunction(param);
        }
    }
}
