using UnityEngine;

namespace Commands
{
    /// <summary>
    /// Command pattern interface
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes command
        /// </summary>
        void Execute();

        /// <summary>
        /// Checks if command can be executed
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }
}