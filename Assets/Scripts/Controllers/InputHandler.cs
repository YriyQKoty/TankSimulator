using System.Collections.Generic;
using System.Linq;
using Commands;
using Commands.AudioCommands;
using Commands.MovementCommands;
using Commands.RotationCommands;
using UnityEngine;

namespace Controllers
{
    public class InputHandler : MonoBehaviour
    {
        /// <summary>
        /// Tank instance
        /// </summary>
        [SerializeField] private TankController tankController;

        /// <summary>
        /// Collection of movement commands
        /// </summary>
        private ICollection<ICommand> moveCommands = new List<ICommand>();
        /// <summary>
        /// Collection of rotation commands
        /// </summary>
        private ICollection<ICommand> rotationCommands = new List<ICommand>();

        private ICollection<ICommand> audioCommands = new List<ICommand>();
        
        private ICommand shootCommand;

        private void Start()
        {
            InitCommands();
        }

        /// <summary>
        /// Inits command collections
        /// </summary>
        private void InitCommands()
        {
            moveCommands.Add(new MoveCommand(tankController, Direction.Forward));
            moveCommands.Add(new MoveCommand(tankController, Direction.Backward));
            moveCommands.Add(new StopCommand(tankController));
        
            rotationCommands.Add(new RotateCommand(tankController));
            rotationCommands.Add(new TurretRotateCommand(tankController));
            rotationCommands.Add(new GunRotateCommand(tankController));
            
            audioCommands.Add(new StopEngineSound(tankController));
            audioCommands.Add(new StopTrackSound(tankController));

            shootCommand = new ShootCommand(tankController);
        }
    
        void FixedUpdate()
        {
            moveCommands.FirstOrDefault(cmd => cmd.CanExecute())?.Execute();

            foreach (var command in rotationCommands.Where(cmd => cmd.CanExecute()))
            {
                command.Execute(); 
            }

            foreach (var command in audioCommands.Where(cmd => cmd.CanExecute()))
            {
                command.Execute();
            }
            
            if (shootCommand.CanExecute()) shootCommand.Execute();
        }
    }
}



