using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        private readonly ILoggable _logger;

        private readonly CommandExecutor _commandExecutor;
        
        private List<IMyTerminalBlock> _myGridBlocks;
        
        private readonly AutoTag _myTag;
        
        public Program()
        {
            _logger = new SurfaceLogger(Me.GetSurface(0));
            _commandExecutor = new CommandExecutor(_logger);
            
            _commandExecutor.Add("ClearLogger", () => ((SurfaceLogger)_logger).Clear());
            
            _myTag = new AutoTag(Me);
            
            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);
            _commandExecutor.Add("RemoveTag", () => _myTag.RemoveTag(blocks));
            _commandExecutor.Add("SetTag", () =>
            {
                var blocks2 = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocks(blocks2);
                _myTag.SetTag(blocks2);
                _myGridBlocks = blocks2.Where(x=>x.CustomName.Contains(_myTag.MyTagString)).ToList();
                _logger.LogLine($"Blocks count: {_myGridBlocks.Count}");
                _logger.LogLine($"The tag is: {_myTag.MyTagString}");
            });
            
            _commandExecutor.TryExecute("SetTag");
            
            Init();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & (UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script)) != 0
                && argument != string.Empty)
            {
                var commands = argument.Split(';');
                foreach (var command in commands)
                {
                    var commandName = command.Split(' ')[0];
                    var commandParams = string.Join(" ", command.Split(' ').Skip(1));

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