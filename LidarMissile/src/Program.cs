
using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRageMath;


namespace IngameScript
{
    
    partial class Program : IProgram
    {
        private Lidar _lidar;
        
        private LidarScanner _scanner;
        
        private LockedEntityHandler _lockedEntity;
        
        private DirectFlyier _flyieer;
        
        public void Init()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            
            Echo(_myTag.MyTagString);
            var cameras = _myGridBlocks.Select(x=>x as IMyCameraBlock).Where(x=>x!=null).ToList();
            _lidar = new Lidar(_logger,cameras);
            _scanner = new LidarScanner(_logger, _lidar, 5000, 2);
            
            _scanner.OnLock += OnLock;

            var gyros = _myGridBlocks.Select(x => x as IMyGyro).Where(x => x != null).ToList();
            var shipControler = (IMyShipController)_myGridBlocks.FirstOrDefault(x => x is IMyShipController);
            _flyieer = new DirectFlyier(_logger, gyros, shipControler);

            
            _commandExecutor.Add("Start", (coords) =>
            {
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
                
                var arr = coords[0].Split(',');
                float x = float.Parse(arr[0]),
                      y = float.Parse(arr[1]),
                      z = float.Parse(arr[2]);
                
                Vector3D vct = new Vector3D(x,y,z);

                var result = _lidar.Scan(vct);
                
                if(result.IsEmpty()) _logger.LogLine("No target found.");
                else OnLock(result);
            });

        }

        public void Execute()
        {
            if(_lockedEntity == null) return;

            IfLost();
            
            _lockedEntity.HoldLock();
            
            _flyieer.NavigateTo(_lockedEntity.LastDetectedEntity.Position);
        }

        public void OnLock(MyDetectedEntityInfo info)
        {
            _lockedEntity = new LockedEntityHandler(_logger, _lidar, info);
            _lockedEntity.HoldLock();

            var merges = _myGridBlocks
                .Select(x => x as IMyShipMergeBlock)
                .Where(x => x != null)
                .Where(x => x.CustomName.Contains(_myTag.MyTagString));

            foreach (var merge in merges)
            {
                merge.Enabled = false;
            }

            var thrusters = _myGridBlocks.Select(x => x as IMyThrust).Where(x => x != null);

            foreach (var thrust in thrusters)
            {
                thrust.Enabled = true;
                thrust.ThrustOverride = thrust.MaxThrust;
            }
        }

        public void IfLost()
        {
            if (!_lockedEntity.IsLocked)
            {
                foreach (var warhead in _myGridBlocks.Select(x=>x as IMyWarhead).Where(x=> x!=null))
                {
                    warhead.IsArmed = true;
                    warhead.StartCountdown();
                }
            }
        }
    }
}