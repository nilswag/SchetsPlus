
namespace schetsplus.src.tools
{
    internal interface ISchetsTool
    {
        void MouseDown(SchetsControl s, Point p);
        void MouseUp(SchetsControl s, Point p);
        void MouseDrag(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
    }
}
