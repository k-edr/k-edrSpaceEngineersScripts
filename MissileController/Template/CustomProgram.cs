using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        ILoggable _logger;

        CommandExecutor _commandExecutor;

        public Program()
        {
            _logger = new SurfaceLogger(Me.GetSurface(0));
            _commandExecutor = new CommandExecutor(_logger);
            
            _commandExecutor.Add("ClearLogger", () => ((SurfaceLogger)_logger).Clear());

            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);
            _commandExecutor.Add("RemoveTag", () => new AutoTag(Me).RemoveTag(blocks));
            _commandExecutor.Add("SetTag", () => new AutoTag(Me).SetTag(blocks));
            
            Init();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & (UpdateType.Terminal | UpdateType.Trigger)) != 0
                && argument != string.Empty)
            {
                var commands = argument.Split(';');
                foreach (var command in commands)
                {
                    var commandName = argument.Split(' ')[0];
                    var commandParams = string.Join(" ", argument.Split(' ').Skip(1));

                    _commandExecutor.TryExecute(commandName, commandParams);
                }
            }
            else
            {
                Echo(DateTime.Now.ToString("hh:mm:ss"));
                Execute();
            }
        }
    }
}