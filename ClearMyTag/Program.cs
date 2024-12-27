using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;


namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        private readonly AutoTag _autoTag;

        public Program()
        {
            _autoTag = new AutoTag(Me);
            
            var blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(blocks);
            
            _autoTag.SetTag(blocks);
        }
        

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & (UpdateType.Terminal | UpdateType.Trigger | UpdateType.Script)) != 0
                && argument != string.Empty)
            {
                var blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocks(blocks);
                
                if (argument.Equals("SetTag"))
                {
                    _autoTag.SetTag(blocks);
                }
                else if (argument.Equals("RemoveTag"))
                {
                    _autoTag.RemoveTag(blocks);
                }
            }
        }
    }
}