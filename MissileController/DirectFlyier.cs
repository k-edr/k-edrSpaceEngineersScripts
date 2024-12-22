using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    class DirectFlyier : IFlyible
    {
        private List<IMyGyro> _gyros;
        private IMyShipController _controller;
        private ILoggable _logger;
        
        public DirectFlyier(ILoggable logger, ICollection<IMyGyro> gyros, IMyShipController controller)
        {
            _logger = logger;
            _gyros = (List<IMyGyro>)gyros;
            _controller = controller;
        }
        
        public void NavigateTo(FlyingPoint point)
        {
            var angles = GetNavAngles(point.Point) * 5;
            
            foreach (var gyro in _gyros)
            {
                //var anglesLocal = TransformAnglesToLocal(angles, gyro.WorldMatrix);
                gyro.GyroOverride = true;
                gyro.Yaw = (float)angles.X;
                gyro.Pitch = (float)angles.Y;
                gyro.Roll = (float)angles.Z;
            }
            
            _logger.LogLine($"Navigating to {point.Point}");
        }

        private Vector3D TransformAnglesToLocal(Vector3D angles, MatrixD gyroMatrix)
        {
            return new Vector3D(
                Vector3D.Dot(angles, gyroMatrix.Right),
                Vector3D.Dot(angles, gyroMatrix.Up),
                Vector3D.Dot(angles, gyroMatrix.Backward)
            );
        }
        
        private Vector3D GetNavAngles(Vector3D target)
        {
            var center = _controller.GetPosition();
            var forward = _controller.WorldMatrix.Forward;
            var up = _controller.WorldMatrix.Up;
            var left = _controller.WorldMatrix.Left;
            
            var targetVector = target - center;

            var targetPitch = Math.Acos(Vector3D.Dot(
                up,
                Vector3.Normalize(Vector3D.Reject(targetVector, left))
            )) - (Math.PI / 2);
            
            double targetYaw = Math.Acos (Vector3D.Dot(left,
                Vector3D.Normalize(Vector3D.Reject(targetVector, up)))) - (Math.PI/2);
            
            double targetRoll = Math.Acos(Vector3D.Dot(left,
                Vector3D.Normalize(Vector3D. Reject(up, forward)))) - (Math.PI/2);
            
            _logger.LogLine($"Target pitch: {targetPitch:0.00}, yaw: {targetYaw:0.00}, roll: {targetRoll:0.00}");
            
            return new Vector3D(targetYaw, -targetPitch, targetRoll);
        }
    }
}