namespace Nemo.Tools.CanvasCommands
{
    public class DrawDotCommand : CanvasCommand 
    {
        public DrawDotCommand(int x, int y, string color = "black")
        {
            action = "drawDot";
            args = new object[3] {
                x, y, color
            };
        }
    }
}