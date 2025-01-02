using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : IProgram
    {
        LidarScanner _scanner;
        
        Lidar _lidar;
        
        public void Init()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            
            var cameras = _myGridBlocks.Select(x => x as IMyCameraBlock).Where(x => x != null).ToList();
            
            _lidar = new Lidar(_logger,cameras);
            _scanner = new LidarScanner(_logger, _lidar, 15000, 2);
            
            _scanner.OnLock += OnLock;
            
            _commandExecutor.Add("TryStartMissile", (command) =>
            {
                var pbs = new List<IMyProgrammableBlock>(); 
                GridTerminalSystem.GetBlocksOfType(pbs);
                
                pbs = pbs.Select(x=>x).Where(x=>!x.CustomName.Contains(_myTag.MyTagString)).ToList();
                
                _logger.LogLine($"Try to execute {pbs.First().CustomName} pb");
                if (pbs.First().TryRun(command[0]))
                {
                    _logger.LogLine("Missile started.");
                }
                else
                {
                    _logger.LogLine("Missile failed to start.");
                }
                
            });
            
            _commandExecutor.Add("StartLocking", () => _scanner.StartLocking());
            _commandExecutor.Add("StopLocking", () => _scanner.StopLocking());

        }

        public void Execute()
        {
            _scanner.TryLock();
        }

        public void OnLock(MyDetectedEntityInfo scanResult)
        {
            _commandExecutor.TryExecute("TryStartMissile",
                $"Start {scanResult.Position.X},{scanResult.Position.Y},{scanResult.Position.Z}");
        }
    }
}