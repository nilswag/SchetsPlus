using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schetsplus.src
{
    internal class SchetsEditor : Form
    {
        private MenuStrip menuStrip;

        public SchetsEditor()
        {
            this.ClientSize = new Size(800, 600);
            menuStrip = new MenuStrip();
            this.Controls.Add(menuStrip);
            this.createFileMenu();
            this.createHelpMenu();
            this.Text = "Schets editor";
            this.IsMdiContainer = true;
            this.MainMenuStrip = menuStrip;

        }

        private void createFileMenu()
        {
            ToolStripDropDownItem menu = new ToolStripMenuItem("File");
            menu.DropDownItems.Add("Nieuw", null, start);
            menu.DropDownItems.Add("Exit", null, stop);
            menuStrip.Items.Add(menu);
        }

        private void createHelpMenu()
        {
            ToolStripDropDownItem menu = new ToolStripMenuItem("Help");
            menu.DropDownItems.Add("Over \"Schets\"", null, about);
            menuStrip.Items.Add(menu);
        }

        private void about(object o, EventArgs e)
        {
            MessageBox.Show(
                "Schets versie 2.0\n(c) UU Informatica 2024",
                "Over \"Schets\"", 
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void start(object o, EventArgs e)
        {
            SchetsWindow s = new SchetsWindow();
            s.MdiParent = this;
            s.Show();
        }

        private void stop(object o, EventArgs e)
        {
            Close();
        }

    }
}
