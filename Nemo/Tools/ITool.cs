

using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools {
    public interface ITool {

        public Task Start(Point point);
        public Task End(Point point);
        public Task OnMove(Point point);
        public Task OnElementClicked(string elementId);
        public Task Cancel();
    }
}