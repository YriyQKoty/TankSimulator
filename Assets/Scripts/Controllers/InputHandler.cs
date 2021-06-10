using System.Collections.Generic;
using System.Linq;
using Commands;
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
        [SerializeField] private TankUnit tank;

        /// <summary>
        /// Collection of movement commands
        /// </summary>
        private ICollection<ICommand> moveCommands = new List<ICommand>();
        /// <summary>
        /// Collection of rotation commands
        /// </summary>
        private ICollection<ICommand> rotationCommands = new List<ICommand>();

        private void Start()
        {
            InitCommands();
        }

        /// <summary>
        /// Inits command collections
        /// </summary>
        private void InitCommands()
        {
            moveCommands.Add(new ForwardMove(tank));
            moveCommands.Add(new BackwardMove(tank));
            moveCommands.Add(new StopCommand(tank));
        
            rotationCommands.Add(new RotateCommand(tank));
            rotationCommands.Add(new TurretRotation(tank));
        }
    
        void FixedUpdate()
        {
            moveCommands.FirstOrDefault(cmd => cmd.CanExecute())?.Execute();

            foreach (var command in rotationCommands.Where(cmd => cmd.CanExecute()))
            {
                command.Execute(); 
            }
        }
    }
}



