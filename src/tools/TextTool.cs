
namespace schetsplus.src.tools
{
    internal class TextTool : StartpuntTool
    {
        public override void MouseDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                Graphics gr = s.CreateBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                string tekst = c.ToString();
                SizeF sz =
                gr.MeasureString(tekst, font, start, StringFormat.GenericTypographic);
                gr.DrawString(tekst, font, brush, start, StringFormat.GenericTypographic);
                start.X += (int)sz.Width;
                s.Invalidate();
            }
        }

        public override string ToString()
        {
            return "tekst";
        }
    }
}
