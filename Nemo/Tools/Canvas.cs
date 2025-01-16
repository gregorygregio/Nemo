
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Nemo.Tools.Drawing;

namespace Nemo.Tools
{
    public class Canvas {
        private IJSRuntime _jsRuntime { get; set; }
        public Canvas(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        private ITool? selectedTool { get; set; }
        public string CursorType { get; set; } = "default";
        public void SelectTool(string tool) {
            switch (tool.ToLower())
            {
                case "pencil":
                    selectedTool = new Pencil(this);
                    CursorType = "cell";
                    break;
                case "rect":
                    selectedTool = new Rect();
                    CursorType = "cell";
                    break;
                case "circle":
                    selectedTool = new Circle();
                    CursorType = "cell";
                    break;
                case "eraser":
                    selectedTool = new Eraser();
                    CursorType = "crosshair";
                    break;
                default:
                    selectedTool = null;
                    CursorType = "default";
                    break;
            }
        }

        public async Task ElementClicked(string elementId) {
            Console.WriteLine("ElementClicked");
            if(selectedTool == null) {
                return;
            }

            var commands = selectedTool.OnElementClicked(elementId);

            foreach(var c in commands) {
                await _jsRuntime.InvokeVoidAsync(c.GetAction(), c.GetParameters());
            }
        }

        public async Task StartToolAction(MouseEventArgs e)
        {
            if(selectedTool == null) {
                return;
            }
            
            var commands = selectedTool.Start(new System.Drawing.Point((int)e.OffsetX, (int)e.OffsetY));    

            foreach(var c in commands) {
                await _jsRuntime.InvokeVoidAsync(c.GetAction(), c.GetParameters());
            }
        }

        public async Task EndToolAction(MouseEventArgs e) {
            if(selectedTool == null) {
                return;
            }
            
            var commands = selectedTool.End(new System.Drawing.Point((int)e.OffsetX, (int)e.OffsetY));

            foreach(var c in commands) {
                await _jsRuntime.InvokeVoidAsync(c.GetAction(), c.GetParameters());
            }
        }

        public async Task MoveTool(MouseEventArgs e)
        {
            if(selectedTool == null) {
                return;
            }

            var commands = selectedTool.OnMove(new System.Drawing.Point((int)e.OffsetX, (int)e.OffsetY));

            foreach(var c in commands) {
                await _jsRuntime.InvokeVoidAsync(c.GetAction(), c.GetParameters());
            }
        }
    }
}