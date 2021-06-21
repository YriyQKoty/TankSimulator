using Controllers;
using UnityEngine;

namespace Commands.AudioCommands
{
    public class StopTrackSound : ICommand
    {
        private TankController _tankController;

        public StopTrackSound(TankController tank)
        {
            if (tank == null)
            {
                Debug.LogError("Tank is null!");
                return;
            }

            _tankController = tank;
        }

        public void Execute()
        {
            _tankController.TrackSource.Stop();
        }

        public bool CanExecute()
        {
            return Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D);
        }
    }
}