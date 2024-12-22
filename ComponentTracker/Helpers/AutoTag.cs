using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class AutoTag
    {
        private IMyProgrammableBlock _block;
        
        public long MyTag => _block.EntityId;

        public AutoTag(IMyProgrammableBlock block)
        {
            _block = block;
        }

        public void SetTag(List<IMyTerminalBlock> blocks)
        {
            string myTagString = $"[{MyTag}]";

            foreach (var block in blocks)
            {
                if (string.IsNullOrEmpty(block.CustomName) || !block.CustomName.Contains("["))
                {
                    if (!block.CustomName.EndsWith(myTagString))
                    {
                        block.CustomName += $" {myTagString}";
                    }
                }
            }
        }

        public void RemoveTag(List<IMyTerminalBlock> blocks)
        {
            foreach (var block in blocks)
            {
                int startIndex = block.CustomName.IndexOf('[');
                int endIndex = block.CustomName.IndexOf(']');

                if (startIndex != -1 && endIndex > startIndex)
                {
                    string tag = block.CustomName.Substring(startIndex, endIndex - startIndex + 1);
                    block.CustomName = block.CustomName.Replace(tag, "").Trim();
                }
            }
        }
    }
}