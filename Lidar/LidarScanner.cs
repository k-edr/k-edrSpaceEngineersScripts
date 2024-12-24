using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    class LidarScanner
    {
        private ILoggable _logger;

        private Lidar _lidar;
        
        private float _scanDistance;
        
        private float _scansPerCycle;
        
        private bool _beginLocking;
        
        public event Action<MyDetectedEntityInfo> OnLock;

        public LidarScanner(ILoggable logger, Lidar lidar, float scanDistance = 10000, float scansPerCycle = 10)
        {
            _logger = logger;
            _lidar = lidar;
            _scanDistance = scanDistance;
            _scansPerCycle = scansPerCycle;
            
            OnLock += WhenLocked;
        }

        public void StartLocking()
        {
            _beginLocking = true;
        }
        
        public void StopLocking()
        {
            _beginLocking = false;
        }

        public void TryLock()
        {
            if (_beginLocking)
            {
                Lock();
            };
        }

        private void WhenLocked(MyDetectedEntityInfo scanResult)
        {
            StopLocking();
        }

        private void Lock()
        {
            int count = 0;
            while (count++<_scansPerCycle)
            {
                MyDetectedEntityInfo scanResult = _lidar.Scan(_scanDistance);

                if (!scanResult.IsEmpty())
                {
                    _logger.LogLine($"Was locked: {scanResult.Type} with id: {scanResult.EntityId}");
                    
                    OnLock?.Invoke(scanResult);
                    
                    return;
                }
            }
        }
    }
}