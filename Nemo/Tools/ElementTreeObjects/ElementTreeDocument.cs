namespace Nemo.Tools.ElementTreeObjects
{
    public class ElementTreeDocument
    {
        private Canvas _canvas;
        public ElementTreeObject RootElement { get; set; } = new ElementTreeObject(true);
        public ElementTreeObject? CurrentElement { get; set; }
        public ElementTreeDocument(Canvas c)
        {
            _canvas = c;
        }
        public void AddElementTreeObject(ElementTreeObject element) {
            if(CurrentElement == null) {
                CurrentElement = RootElement;
            }
            CurrentElement.Next = element;
            CurrentElement = CurrentElement.Next;
            Task.Run(() => RenderElement(element));
        }

        public async Task Redraw() {
            await _canvas.ExecuteAction("clearCanvas", new object[] {_canvas.Width, _canvas.Height });
            Console.WriteLine("Cleared canvas");
            if(RootElement == null) {
                Console.WriteLine("RootElement is null");
                return;
            }
            Console.WriteLine("RootElement is not null");
            var element = RootElement;
            List<Canvas.BatchCanvasAction> list = new();
            while(element != null) {
                Console.WriteLine(string.Format("Rendering element {0}", element.GetElementAction()));
                if(element.IsRoot) {
                    element = element.Next;
                    continue;
                }
                
                element.Rendered = false;
                list.Add(new Canvas.BatchCanvasAction(element.GetElementAction(), element.GetElementParams()));
                element = element.Next;
            }
            await _canvas.ExecuteActionBatch(list);
        }

        
        public async Task RenderElement(ElementTreeObject element) {
            if(!string.IsNullOrEmpty(element.GetElementAction())) {
                await _canvas.ExecuteAction(element.GetElementAction(), element.GetElementParams());
            }

            element.Rendered = true;
        }
    }
}