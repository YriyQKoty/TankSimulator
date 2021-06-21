using Controllers;
using UnityEngine;

namespace Commands.AudioCommands
{
    public class StopEngineSound : ICommand
    {
        private TankController _tankController;

        public StopEngineSound(TankController tank)
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
            _tankController.EngineSource.Stop();
            
            if (!_tankController.EngineSource.isPlaying)
            {
                SoundManager.Instance.PlayClip(_tankController.EngineSource,SoundManager.Instance.EngineEnd);
            }
        }

        public bool CanExecute()
        {
            return (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D);
        }
    }
}