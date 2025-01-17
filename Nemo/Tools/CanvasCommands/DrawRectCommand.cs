namespace Nemo.Tools.CanvasCommands
{
    public class DrawRectCommand : CanvasCommand 
    {
        public DrawRectCommand(int x, int y, int width, int height, string color="black")
        {  
            action = "drawRect";
            args = new object[5] {
                x, y, width, height, color
            };
        }
    }
}