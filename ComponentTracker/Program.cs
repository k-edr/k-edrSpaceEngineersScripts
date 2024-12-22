using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Linq;

namespace IngameScript
{
    partial class Program : IProgram
    {
        long _myTag;

        private List<IMyTerminalBlock> _myGridBlocks;
        
        public void Init()
        {
            _myTag = new AutoTag(Me).MyTag;
            
            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);
            _commandExecutor.Add("RemoveTag", () => new AutoTag(Me).RemoveTag(blocks));
            _commandExecutor.Add("SetTag", () =>
            {
                new AutoTag(Me).SetTag(blocks);
                _myGridBlocks = blocks.Where(x=>x.CustomName.Contains(_myTag.ToString())).ToList();
            });
            
            _commandExecutor.TryExecute("SetTag");
        }

        public void Execute()
        {
            
        }
    }
}