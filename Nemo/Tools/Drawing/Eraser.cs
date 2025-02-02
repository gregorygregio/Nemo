

using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools.Drawing
{
    public class Eraser : BaseTool
    {
        private Point? _lastPoint;
        public Eraser(Canvas canvas) : base(canvas)
        {
        }
        public override Task OnElementClicked(string elementId)
        {
            return Task.CompletedTask;
        }

        public override async Task Start(Point point)
        {
            _lastPoint = point;
            await _canvas.ExecuteAction("clearRect", new object[] { point.X, point.Y, 2, 2 });
        }
        public override async Task OnMove(Point point)
        {
            if(!_lastPoint.HasValue)
            {
                return;
            }
            // for(int i = _lastPoint.Value.X; i < point.X; i++)
            // {
            //     for(int j = _lastPoint.Value.Y; j < point.Y; j++)
            //     {
            //         await _canvas.ExecuteAction("clearRect", new object[] { i, j, 5, 5 });
            //     }
            // }

            _lastPoint = point;
            await _canvas.ExecuteAction("clearRect", new object[] { point.X, point.Y, 5, 5 });
        }
        public override Task End(Point point)
        {
            _lastPoint = null;
            return Task.CompletedTask;
        }
    }
}