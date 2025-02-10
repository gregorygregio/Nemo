using System.Drawing;
using Nemo.Tools.ElementTreeObjects;

namespace Nemo.Tools
{
    public class BaseTool : ITool
    {
        protected readonly Canvas _canvas;
        protected ElementTreeDocument _elementTreeDocument { get; set; }
        public BaseTool(Canvas canvas, ElementTreeDocument etd)
        {
            _canvas = canvas;
            _elementTreeDocument = etd;
        }
        public virtual Task Start(Point point) => Task.CompletedTask;
        public virtual Task End(Point point) => Task.CompletedTask;
        public virtual Task OnMove(Point point) => Task.CompletedTask;
        public virtual Task OnElementClicked(string elementId) => Task.CompletedTask;
        public virtual Task Cancel() => Task.CompletedTask;
    }
}