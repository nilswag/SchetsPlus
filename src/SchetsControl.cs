
using System.Drawing.Drawing2D;

namespace schetsplus.src
{
    internal class SchetsControl : UserControl
    {
        private Schets schets;
        public Schets Schets
        {
            get { return schets; }
        }

        private Color penColor;
        public Color PenColor
        {
            get { return penColor; }
        }

        public SchetsControl()
        {
            BorderStyle = BorderStyle.Fixed3D;
            schets = new Schets();
            Paint += draw;
            Resize += changeSize;
            changeSize(null, null);
        }

        private void changeSize(object o, EventArgs e)
        {
            schets.ChangeSize(ClientSize);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void draw(object o, PaintEventArgs e)
        {
            schets.Draw(e.Graphics);
        }

        public Graphics CreateBitmapGraphics()
        {
            Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }

        public void Clean(object o, EventArgs e)
        {
            schets.Clean();
            Invalidate();
        }

        public void Rotate(object o, EventArgs e)
        {
            schets.ChangeSize(new Size(ClientSize.Width, ClientSize.Height));
            schets.Rotate();
            Invalidate();
        }

        public void ChangeColor(object o, EventArgs e)
        {
            string colorName = ((ComboBox)o).Text;
            penColor = Color.FromName(colorName);
        }

        public void ChangeColorThroughMenu(object o, EventArgs e)
        {
            string colorName = ((ToolStripMenuItem)o).Text;
            penColor = Color.FromName(colorName);
        }
    }
}
