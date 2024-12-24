using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    class LockedEntityHandler
    {
        private ILoggable _logger;
        
        private Lidar _lidar;
        
        private MyDetectedEntityInfo _lastDetectedEntity;

        public MyDetectedEntityInfo LastDetectedEntity => _lastDetectedEntity;
        
        public bool IsLocked => !_lastDetectedEntity.IsEmpty();
        
        public LockedEntityHandler(ILoggable logger, Lidar lidar, MyDetectedEntityInfo lastDetectedEntity)
        {
            _logger = logger;
            _lidar = lidar;
            _lastDetectedEntity = lastDetectedEntity;
        }
        
        public void HoldLock()
        {
            if (!IsLocked) return;
            
            var newScan = _lidar.Scan(_lastDetectedEntity.Position);

            if (newScan.EntityId == _lastDetectedEntity.EntityId && !newScan.IsEmpty())
            {
                _logger.LogLine($"On holding. EntityId: {_lastDetectedEntity.EntityId}");
            }
            else
            {
                _logger.LogLine("Oops. Target was lost.");
            }
            
            _lastDetectedEntity = newScan;
        }
    }
}