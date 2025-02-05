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
            CurrentElement.Child = element;
            CurrentElement = CurrentElement.Child;
            Task.Run(() => RenderElement(element));
        }

        public async Task RenderElement(ElementTreeObject element) {
            if(element.Rendered) {
                return;
            }
            
            if(!string.IsNullOrEmpty(element.GetElementAction())) {
                await _canvas.ExecuteAction(element.GetElementAction(), element.GetElementParams());
            }

            element.Rendered = true;
        }
    }
}