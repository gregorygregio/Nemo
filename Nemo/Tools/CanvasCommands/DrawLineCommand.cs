using System.Drawing;

namespace Nemo.Tools.CanvasCommands
{
    public class DrawLineCommand : CanvasCommand
    {
        public DrawLineCommand(int x1, int y1, int x2, int y2, string color = "black")
        {
            action = "drawLine";
            args = new object[5] {
                x1, y1, x2, y2, color
            };
        }
    }
}