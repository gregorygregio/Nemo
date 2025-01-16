

using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools.Drawing
{
    public class Eraser : BaseTool
    {
        public override IEnumerable<CanvasCommand> OnElementClicked(string elementId)
        {
            return new List<CanvasCommand> { new RemoveElementCommand(elementId) };
        }
    }
}