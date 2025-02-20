

using System.Drawing;
using Nemo.Tools.ElementTreeNodes;

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

        public override Task Start(Point point)
        {
            _lastPoint = point;
            _canvas
                .AddElementTreeObject(new EraserNode(point.X, point.Y, 5, 5));
            return Task.CompletedTask;
        }
        public override Task OnMove(Point point)
        {
            if(!_lastPoint.HasValue)
            {
                return Task.CompletedTask;
            }
            if(_lastPoint.Value.X == point.X && _lastPoint.Value.Y == point.Y)
            {
                return Task.CompletedTask;
            }
            var rootNode = new EraserNode(point.X, point.Y, 5, 5);
            ElementTreeNode currentNode = rootNode;

            Point drawingPoint = new Point(_lastPoint.Value.X, _lastPoint.Value.Y);
            int safety = 0;
            while((drawingPoint.X != point.X || drawingPoint.Y != point.Y) && safety < 1000)
            {
                safety++;
                if(drawingPoint.X < point.X)
                {
                    drawingPoint.X++;
                }
                if(drawingPoint.X > point.X)
                {
                    drawingPoint.X--;
                }
                if(drawingPoint.Y < point.Y)
                {
                    drawingPoint.Y++;
                }
                if(drawingPoint.Y > point.Y)
                {
                    drawingPoint.Y--;
                }

                currentNode.Next = new EraserNode(drawingPoint.X, drawingPoint.Y, 5, 5);
                currentNode = currentNode.Next;
            }

            _lastPoint = point;
            _canvas
                .AddElementTreeObject(rootNode);
                
            return Task.CompletedTask;
        }
        public override Task End(Point point)
        {
            _lastPoint = null;
            return Task.CompletedTask;
        }
    }
}