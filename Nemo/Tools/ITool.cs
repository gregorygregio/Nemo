

using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools {
    public interface ITool {

        public IEnumerable<CanvasCommand> Start(Point point);
        public IEnumerable<CanvasCommand> End(Point point);
        public IEnumerable<CanvasCommand> OnMove(Point point);
        public IEnumerable<CanvasCommand> OnElementClicked(string elementId);
    }
}