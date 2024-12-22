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
                var allPBs = new List<IMyProgrammableBlock>();
                List<IMyProgrammableBlock> factoryPbs = (List<IMyProgrammableBlock>)_myGridBlocks.Where(x => x is IMyProgrammableBlock);
                GridTerminalSystem.GetBlocksOfType(allPBs);
                
                var newGridPb = allPBs.Except(factoryPbs).FirstOrDefault();

                if (newGridPb != null)
                {
                    newGridPb.TryRun("SetTag");
                }
                
                _logger.LogLine("Finish products production.");
            });
        }

        public void Execute()
        {
            
        }
    }
}