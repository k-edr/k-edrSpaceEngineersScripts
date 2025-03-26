using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        private ILoggable _logger;

        private CommandExecutor _commandExecutor;

        private List<IMyTerminalBlock> _myGridBlocks;

        private AutoTag _myTag;

        public Program()
        {
            InitLogger();
            InitCommandExecutor();
            InitAutoTag();

            _commandExecutor.Add("ClearLogger", () => ((SurfaceLogger)_logger).Clear());
            _commandExecutor.TryExecute("SetTag");

            Init();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & (UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script)) != 0
                && argument != string.Empty)
            {
                _logger.LogLine($"Got command\\s:" + argument);
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


        private void InitCommandExecutor()
        {
            _commandExecutor = new CommandExecutor(_logger);
        }

        private void InitLogger()
        {
            _logger = new SurfaceLogger(Me.GetSurface(0));
            Me.GetSurface(0).ContentType = ContentType.TEXT_AND_IMAGE;

            (_logger as SurfaceLogger).Clear();
        }

        private void InitAutoTag()
        {
            _myTag = new AutoTag(_logger, Me);

            _logger.LogLine($"The tag is: {_myTag.MyTagString}");

            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);
            _commandExecutor.Add("RemoveAllTags", () => _myTag.RemoveAllTags(blocks));
            _commandExecutor.Add("SetTag", () =>
            {
                var blocks2 = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocks(blocks2);
                _myTag.SetTag(blocks2);
                _myGridBlocks = blocks2.Where(x => x.CustomName.Contains(_myTag.MyTagString)).ToList();
                _logger.LogLine($"Blocks count: {_myGridBlocks.Count}");
                _logger.LogLine($"The tag is: {_myTag.MyTagString}");
            });
        }
    }
}
//TODO: add the MyGridBlocksOfType<>(), MyGridBlocksWithName(string), MyGridBlocksContainsName(string), FirstBlockOfType<>(),FirstBlockWithName().
//_myGridBlocks.Select(x => x as IMyShipMergeBlock).Where(x => x != null);