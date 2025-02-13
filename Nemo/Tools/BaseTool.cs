using System.Drawing;
using Nemo.Tools.ElementTreeObjects;

namespace Nemo.Tools
{
    public class BaseTool : ITool
    {
        protected readonly Canvas _canvas;
        public BaseTool(Canvas canvas)
        {
            _canvas = canvas;
        }
        public virtual Task Start(Point point) => Task.CompletedTask;
        public virtual Task End(Point point) => Task.CompletedTask;
        public virtual Task OnMove(Point point) => Task.CompletedTask;
        public virtual Task OnElementClicked(string elementId) => Task.CompletedTask;
        public virtual Task Cancel() => Task.CompletedTask;
    }
}