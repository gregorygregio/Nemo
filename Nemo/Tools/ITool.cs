using System.Drawing;

namespace Nemo.Tools {
    public interface ITool {

        public Task Start(Point point);
        public Task End(Point point);
        public Task OnMove(Point point);
        public Task OnElementClicked(string elementId);
        public Task Cancel();
    }
}