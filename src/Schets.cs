
namespace schetsplus.src
{
    internal class Schets
    {
        private Bitmap bitmap;
        public Graphics BitmapGraphics 
        {
            get { return Graphics.FromImage(bitmap); }
        }
        
        public Schets()
        {
            bitmap = new Bitmap(1, 1);
        }

        public void ChangeSize(Size size)
        {
            if (size.Width > bitmap.Size.Width || size.Height > bitmap.Size.Height)
            {
                Bitmap new_ = new Bitmap(
                    Math.Max(size.Width, bitmap.Size.Width),
                    Math.Max(size.Height, bitmap.Size.Height)
                );

                Graphics g = Graphics.FromImage(new_ );
                g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
                g.DrawImage(bitmap, 0, 0);
                bitmap = new_;
            }
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(bitmap, 0, 0);
        }

        public void Clean()
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }

        public void Rotate()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

    }
}
