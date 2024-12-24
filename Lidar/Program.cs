using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
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
    class LockedEntitiesSurfaceDisplay
    {
        private IMyTextSurface _surface;
        
        private List<LockedEntityHandler> _handlers;
        
        private Lidar _lidar;
        private LidarScanner _scanner;
        
        public LockedEntitiesSurfaceDisplay(IMyTextSurface surface, List<LockedEntityHandler> handlers, Lidar lidar, LidarScanner scanner)
        {
            _surface = surface;
            _handlers = handlers;
            _lidar = lidar;
            _scanner = scanner;
        }

        public void Update()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Available distance: {_lidar.MaxAvailableDistance()}");
 
            foreach (var handler in _handlers)
            {
                sb.AppendLine($"Entity: {handler.LastDetectedEntity.EntityId}");
                sb.AppendLine($"Position: {handler.LastDetectedEntity.Position}");
                sb.AppendLine($"Speed: {handler.LastDetectedEntity.Velocity.LengthSquared()}");
            }

            _surface.WriteText(sb);
        }
        
    }
    
    partial class Program : IProgram
    {
        private LidarScanner _lidarScanner;
        
        private Lidar _lidar;
        
        private List<LockedEntityHandler> _lockedEntityHandlers = new List<LockedEntityHandler>();
        
        private LockedEntitiesSurfaceDisplay _display;
        
        public void Init()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            
            var cameras = _myGridBlocks.Select(x=> x as IMyCameraBlock).Where(x=> x is IMyCameraBlock).ToList();
            
            _lidar = new Lidar(_logger, cameras);
            _lidarScanner = new LidarScanner(_logger, _lidar,15000,10);
            _lidarScanner.OnLock += OnLock;
            
            _commandExecutor.Add("StartScan", () => _lidarScanner.StartLocking());
            _commandExecutor.Add("StopScan", () => _lidarScanner.StopLocking());

            var display = _myGridBlocks.Select(x => x).FirstOrDefault(x => x.CustomName.Equals("Display " + _myTag.MyTagString)) as IMyTextSurface;
            _display = new LockedEntitiesSurfaceDisplay(display, _lockedEntityHandlers, _lidar, _lidarScanner);
        }

        public void Execute()
        {
            _lidarScanner.TryLock();
            
            _lockedEntityHandlers.RemoveAll(x => !x.IsLocked);
            foreach (var handler in _lockedEntityHandlers)
            {
                handler.HoldLock();
            }
            
            _display.Update();
        }

        private void OnLock(MyDetectedEntityInfo scanResult)
        {
            if(_lockedEntityHandlers.Any(x => x.LastDetectedEntity.EntityId == scanResult.EntityId)) return;
            var newHandler = new LockedEntityHandler(_logger, _lidar, scanResult);
            
            newHandler.HoldLock();

            if (newHandler.LastDetectedEntity.EntityId == scanResult.EntityId)
            {
                _lockedEntityHandlers.Add(newHandler);
                
                _logger.LogLine($"Successfully locked entity: {scanResult.EntityId}");
            }
            else
            {
                _logger.LogLine($"Failed to lock entity: {scanResult.EntityId}");
            }
        }
    }
}