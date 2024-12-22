using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    class FlyControler
    {
        private ILoggable _logger;

        private FlyingPlan _plan;

        private IMyShipController _controller;
        
        private bool _begin;
        
        private float _precision;
        
        private IFlyible _fly;

        public FlyControler(ILoggable logger, IMyShipController controller, IFlyible fly, float precision = 10)
        {
            _logger = logger;
            _controller = controller;
            _precision = precision;
            _fly = fly;
        }

        public void SetNewPlan(FlyingPlan plan)
        {
            _plan = plan;
            _logger.LogLine("New flying plan set.");
        }

        public void BeginFlying()
        {
            if (_plan == null)
            {
                _logger.LogLine("No flying plan set. Cannot begin flying.");
                return;
            }
            
            _begin = true;
            _logger.LogLine("Flying started.");
            Control();
        }

        public void Control()
        {
            if (!_begin) return;

            var pointsQueue = _plan.GetQueue();
            if (pointsQueue.Count == 0)
            {
                _logger.LogLine("No points to navigate to.");
                return;
            }

            if (pointsQueue.Count > 0)
            {
                ProcessPoint(pointsQueue);
            }

            if (pointsQueue.Count == 0)
            {
                _logger.LogLine("Flight plan completed.");
            }
        }

        private void ProcessPoint(Queue<FlyingPoint> pointsQueue)
        {
            var point = pointsQueue.Peek();
           
            _logger.LogLine($"Distance to point: {(_controller.GetPosition() - point.Point).Length()}");
            if ((_controller.GetPosition() - point.Point).Length() < _precision)
            {
                _logger.LogLine($"Navigating to point: {point.Point}");
                    
                if (point.WhenReached != null)
                {
                    point.WhenReached?.Invoke();
                }

                pointsQueue.Dequeue();
            }
            else
            {
                _logger.LogLine($"Distance to point: {(_controller.GetPosition() - point.Point).LengthSquared()}");
                _fly.NavigateTo(point);
            }
        }
    }
}