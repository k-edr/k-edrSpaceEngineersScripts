using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    
    class Lidar
    {
        private List<IMyCameraBlock> _cameras;

        private ILoggable _logger;

        public Lidar (ILoggable logger, List<IMyCameraBlock> cameras)
        {
            _cameras = cameras;
            _logger = logger;

            EnableCameraRaycast(_cameras);
            _logger.Log($"Lidar initialized. Initialized cameras: {_cameras.Count}.");
        }

        public double MaxAvailableDistance()
        {
            double max = _cameras[0].AvailableScanRange;
            foreach (var camera in _cameras)
            {
                if (camera.AvailableScanRange > max)
                {
                    max = camera.AvailableScanRange;
                }
            }
            
            return max;
        }
        
        public MyDetectedEntityInfo Scan(Vector3D position)
        {
            IMyCameraBlock camera;

            if (TryGetAvailableCamera(position, out camera))
            {
                return camera.Raycast(position);
            }

            return new MyDetectedEntityInfo();
        }
        
        public MyDetectedEntityInfo Scan(float distance)
        {
            IMyCameraBlock camera;

            if (TryGetAvailableCamera(distance, out camera))
            {
                _logger.LogLine($"Scanning with camera: {camera.CustomName}");
                return camera.Raycast(distance);
            }

            return new MyDetectedEntityInfo();
        }
        
        private bool TryGetAvailableCamera(Vector3D position, out IMyCameraBlock camera)
        {
            camera = null;
            foreach (var cam in _cameras)
            {
                if (cam.CanScan(position))
                {
                    camera = cam;
                    return true;
                }
            }

            return false;
        }

        private bool TryGetAvailableCamera(float distance, out IMyCameraBlock camera)
        {
            camera = null;
            foreach (var cam in _cameras)
            {
                if (cam.CanScan(distance))
                {
                    camera = cam;
                    return true;
                }
            }
            
            _logger.LogLine($"No camera found for distance: {distance}");
            return false;
        }

        private void EnableCameraRaycast(List<IMyCameraBlock> cameras, bool enable = true)
        {
            foreach (var camera in _cameras)
            {
                camera.EnableRaycast = enable;
            }
        }
    }
}
