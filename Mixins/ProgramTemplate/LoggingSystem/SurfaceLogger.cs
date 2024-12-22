using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class SurfaceLogger : ILoggable
    {
        private readonly IMyTextSurface _surface;

        public SurfaceLogger(IMyTextSurface surface)
        {
            _surface = surface;
        }

        public void Log(string message)
        {
            _surface.WriteText(message, append: true);
        }

        public void LogLine(string message)
        {
            _surface.WriteText(message + "\n", append: true);
        }

        public void Clear()
        {
            _surface.WriteText(string.Empty, append: false);
        }
    }
}