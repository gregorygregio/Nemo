namespace Nemo 
{
    public class CanvasImage
    {
        public CanvasImage(string fileName, string contentType)
        {
            FileName = fileName;
            ContentType = contentType;
        }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[]? ImageData { get; set; }
        public bool IsLoaded { get; set; } = false;
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;

        public async Task LoadImage(Stream readStream)
        {
            ImageData = new byte[readStream.Length];
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            long position = 0;
            do {
                bytesRead = await readStream.ReadAsync(buffer, 0, buffer.Length);
                for(int i = 0; i < bytesRead; i++) {
                    ImageData[position] = buffer[i];
                    position++;
                }
            } while(bytesRead > 0);

            IsLoaded = true;
        }
    }
}