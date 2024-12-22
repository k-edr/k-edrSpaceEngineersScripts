using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : IProgram
    {
        private IMyTimerBlock _beginTimer;
        
        public void Init()
        {
            _beginTimer = GridTerminalSystem.GetBlockWithName($"Begin timer {_myTag.MyTagString}") as IMyTimerBlock;
            
            _commandExecutor.Add("WhenBegin", () =>
            {
                _logger.LogLine("Start products production.");
                _beginTimer.Trigger();
            });
            
            _commandExecutor.Add("WhenFinish", () =>
            {
                var factoryPbs = _myGridBlocks
                    .Select(x=> x as IMyProgrammableBlock)
                    .Where(x => x != null)
                    .ToList();
                
                var allPBs = new List<IMyProgrammableBlock>();
                GridTerminalSystem.GetBlocksOfType(allPBs);
                
                var newGridPb = allPBs.Except(factoryPbs, new PBEqualsComparer()).FirstOrDefault();
                if (newGridPb != null)
                {
                    _logger.LogLine("Try run: " + newGridPb.TryRun("SetTag"));
                }
                
                _logger.LogLine("Finish products production.");
            });
        }

        public void Execute()
        {
            
        }
    }
}

