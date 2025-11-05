using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

public class SchetsEditor : Form
{
    private MenuStrip menuStrip;

    public SchetsEditor()
    {
        this.ClientSize = new Size(800, 600);
        this.Text = "Schets editor";
        this.IsMdiContainer = true;

        menuStrip = new MenuStrip();
        this.Controls.Add(menuStrip);
        this.MainMenuStrip = menuStrip;

        MaakFileMenu();
        MaakHelpMenu();
    }

    private void MaakFileMenu()
    {
        ToolStripDropDownItem menu = new ToolStripMenuItem("File");
        menu.DropDownItems.Add("Nieuw", null, (o, e) => Nieuw(o, e));
        menu.DropDownItems.Add("Open", null, Openen);
        menu.DropDownItems.Add("Exit", null, Afsluiten);
        menuStrip.Items.Add(menu);
    }

    private void MaakHelpMenu()
    {
        ToolStripDropDownItem menu = new ToolStripMenuItem("Help");
        menu.DropDownItems.Add("Over \"Schets\"", null, About);
        menuStrip.Items.Add(menu);
    }

    private void DeserializeTekenbareElementen(SchetsWin w, string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        Console.WriteLine(path);
        w.SchetsControl.Schets.Elementen = (List<TekenbaarElement>)new BinaryFormatter().Deserialize(fs);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        fs.Close();
    }

    private void Openen(object o, EventArgs e)
    {
        OpenFileDialog path = new OpenFileDialog();
        path.Filter = "PNG (*.png)|*.png|JPG (*.jpg,*.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp|SchetsPlus formaat (*.sp)|*.sp";
        SchetsWin w = Nieuw(null, null);
        if (path.ShowDialog() == DialogResult.OK)
        {
            if (path.FilterIndex == 4)
            {
                DeserializeTekenbareElementen(w, path.FileName);
            } else 
                w.SchetsControl.Schets.LaadPlaatje(path.FileName);
            w.SchetsControl.Invalidate();
        }
    }

    private void About(object sender, EventArgs e)
    {
        MessageBox.Show(
            "Schets versie 2.0\n(c) UU Informatica 2024",
            "Over \"Schets\"",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    private SchetsWin Nieuw(object sender, EventArgs e)
    {
        SchetsWin s = new SchetsWin();
        s.MdiParent = this;
        s.Show();

        return s;
    }

    private void Afsluiten(object sender, EventArgs e)
    {
        this.Close();
    }
}
