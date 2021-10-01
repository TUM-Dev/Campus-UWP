using System;
using System.Diagnostics;
using System.Windows.Input;

namespace UI_Context.Classes
{
    /// <summary>
    /// Source: https://github.com/CommunityToolkit/WindowsCommunityToolkit/blob/v7.1.0/Microsoft.Toolkit.Uwp.SampleApp/Common/DelegateCommand.cs
    /// </summary>
    public class DelegateCommand<T>: ICommand
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private readonly Action<T> commandExecuteAction;
        private readonly Func<T, bool> commandCanExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecute = null)
        {
            commandExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            commandCanExecute = canExecute ?? (e => true);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// The parameter used by the command.
        /// </param>
        /// <returns>
        /// Returns a value indicating whether this command can be executed.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            try
            {
                return commandCanExecute(ConvertParameterValue(parameter));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// The parameter used by the command.
        /// </param>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            try
            {
                commandExecuteAction(ConvertParameterValue(parameter));
            }
            catch
            {
                Debugger.Break();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region --Misc Methods (Private)--
        private static T ConvertParameterValue(object parameter)
        {
            parameter = parameter is T ? parameter : Convert.ChangeType(parameter, typeof(T));
            return (T)parameter;
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
