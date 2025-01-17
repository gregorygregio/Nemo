namespace Nemo.Tools.CanvasCommands
{
    public class DrawCircleCommand : CanvasCommand
    {
        public DrawCircleCommand(int x, int y, int radius, string color = "black")
        {
            action = "drawCircle";
            args = new object[4] {
                x, y, radius, color
            };
        }
    }
}