using System;
using System.Drawing;
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
        menu.DropDownItems.Add("Nieuw", null, Nieuw);
        menu.DropDownItems.Add("Exit", null, Afsluiten);
        menuStrip.Items.Add(menu);
    }

    private void MaakHelpMenu()
    {
        ToolStripDropDownItem menu = new ToolStripMenuItem("Help");
        menu.DropDownItems.Add("Over \"Schets\"", null, About);
        menuStrip.Items.Add(menu);
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

    private void Nieuw(object sender, EventArgs e)
    {
        SchetsWin s = new SchetsWin();
        s.MdiParent = this;
        s.Show();
    }

    private void Afsluiten(object sender, EventArgs e)
    {
        this.Close();
    }
}
