using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools
{
    public class BaseTool : ITool
    {
        public virtual IEnumerable<CanvasCommand> Start(Point point) => new List<CanvasCommand>();
        public virtual IEnumerable<CanvasCommand> End(Point point) => new List<CanvasCommand>();
        public virtual IEnumerable<CanvasCommand> OnMove(Point point) => new List<CanvasCommand>();
        public virtual IEnumerable<CanvasCommand> OnElementClicked(string elementId) => new List<CanvasCommand>();
    }
}