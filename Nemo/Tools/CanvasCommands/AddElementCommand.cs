
namespace Nemo.Tools.CanvasCommands
{
    public class AddElementCommand : CanvasCommand
    {
        public AddElementCommand(string elementType, string elementId, object? attributes)
        {  
            action = "addSvgElement";
            args = new object[3] {
                elementType,
                elementId,
                attributes
            };
        }
    }
}