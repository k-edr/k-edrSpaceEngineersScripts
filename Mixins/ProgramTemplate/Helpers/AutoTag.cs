using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class AutoTag
    {
        private IMyProgrammableBlock _block;
        
        private ILoggable _logger;
        
        public long MyTag => _tag;

        private long _tag;
        public string MyTagString => $"[{_tag}]";

        public AutoTag(ILoggable logger, IMyProgrammableBlock block)
        {
            _block = block;
            _logger = logger;
            
            var wasTaggedBeforeIni = _block.CustomName.Contains('[');
            var isTagSameToPbId = _block.CustomName.Contains($"[{_block.EntityId}]");

            if (wasTaggedBeforeIni)
            {
                int startIndex = block.CustomName.IndexOf('[') + 1;
                int endIndex = block.CustomName.IndexOf(']');

                if (startIndex > 0 && endIndex > startIndex)
                {
                    long.TryParse(block.CustomName.Substring(startIndex, endIndex - startIndex), out _tag);
                    _logger.LogLine($"Was found tag: {_tag}. Found tag was set.");
                }
            }
            else
            {
                _tag = _block.EntityId;
                
                _logger.LogLine($"Was not found tag. New tag was set as in Pb: {_tag}.");
            }
        }

        public void SetTag(List<IMyTerminalBlock> blocks)
        {
            foreach (var block in blocks)
            {
                if (string.IsNullOrEmpty(block.CustomName) || !block.CustomName.Contains("["))
                {
                    if (!block.CustomName.EndsWith(MyTagString))
                    {
                        block.CustomName += $" {MyTagString}";
                    }
                }
            }
        }

        public void RemoveAllTags(List<IMyTerminalBlock> blocks)
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