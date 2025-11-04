using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class SchetsControl : UserControl
{
    private readonly Schets schets;
    private Color penkleur;
    private SchetsWin win;

    public Color PenKleur { get { return penkleur; } }

    public Schets Schets { get { return schets; } }

    public SchetsControl(SchetsWin win)
    {
        this.BorderStyle = BorderStyle.Fixed3D;
        this.schets = new Schets();
        this.Paint += Teken;
        this.Resize += VeranderAfmeting;

        VeranderAfmeting(null, null);
        this.win = win;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Leeg zodat achtergrond niet standaard wordt gewist
    }

    private void Teken(object sender, PaintEventArgs e)
    {
        schets.Teken(e.Graphics);
        win.Opeslagen = false;
    }

    private void VeranderAfmeting(object sender, EventArgs e)
    {
        schets.VeranderAfmeting(this.ClientSize);
        this.Invalidate();
    }

    public Graphics MaakBitmapGraphics()
    {
        Graphics g = schets.BitmapGraphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        return g;
    }

    public void Schoon(object sender, EventArgs e)
    {
        schets.Schoon();
        this.Invalidate();
    }

    public void Roteer(object sender, EventArgs e)
    {
        schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
        schets.Roteer();
        this.Invalidate();
    }

    public void VeranderKleur(object sender, EventArgs e)
    {
        if (sender is ComboBox cbb)
        {
            penkleur = Color.FromName(cbb.Text);
        }
    }

    public void VeranderKleurViaMenu(object sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem item)
        {
            penkleur = Color.FromName(item.Text);
        }
    }
}
