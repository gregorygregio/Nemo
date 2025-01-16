

namespace Nemo.Tools.CanvasCommands
{
    public class CanvasCommand {

        protected string action { get; set; } = "";
        protected object?[]? args { get; set; } = new object[0];

        public string GetAction() => action;
        public object?[]? GetParameters() => args;
    }
}