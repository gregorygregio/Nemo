


namespace Nemo.Tools.CanvasCommands {
    public class RemoveElementCommand : CanvasCommand 
    {
        public RemoveElementCommand(string elementId)
        {
            action = "removeSvgElement";
            args = new object[1] {
                elementId
            };
        }
    }
}