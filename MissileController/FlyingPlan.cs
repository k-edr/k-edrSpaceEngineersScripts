using System.Collections.Generic;

namespace IngameScript
{
    class FlyingPlan
    {
        private List<FlyingPoint> _points = new List<FlyingPoint>();

        public void Add(FlyingPoint point)
        {
            _points.Add(point);
        }

        public void Remove(FlyingPoint point)
        {
            _points.Remove(point);
        }

        public Queue<FlyingPoint> GetQueue()
        {
            return new Queue<FlyingPoint>(_points);
        }
    }
}