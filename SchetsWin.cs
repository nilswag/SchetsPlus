using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class SchetsWin : Form
{
    private MenuStrip menuStrip;
    private SchetsControl schetscontrol;
    private ISchetsTool huidigeTool;
    private Panel paneel;
    private bool vast;

    public SchetsWin()
    {
        ISchetsTool[] deTools = {
            new PenTool(),
            new LijnTool(),
            new RechthoekTool(),
            new VolRechthoekTool(),
            new TekstTool(),
            new GumTool(),
            new CirkelTool(),
            new VolCirkelTool()
        };

        string[] deKleuren = { "Black", "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan" };

        this.ClientSize = new Size(700, 500);
        huidigeTool = deTools[0];

        schetscontrol = new SchetsControl
        {
            Location = new Point(64, 10)
        };

        schetscontrol.MouseDown += (s, e) =>
        {
            vast = true;
            huidigeTool.MuisVast(schetscontrol, e.Location);
        };

        schetscontrol.MouseMove += (s, e) =>
        {
            if (vast)
                huidigeTool.MuisDrag(schetscontrol, e.Location);
        };

        schetscontrol.MouseUp += (s, e) =>
        {
            if (vast)
                huidigeTool.MuisLos(schetscontrol, e.Location);
            vast = false;
        };

        schetscontrol.KeyPress += (s, e) =>
        {
            huidigeTool.Letter(schetscontrol, e.KeyChar);
        };

        this.Controls.Add(schetscontrol);

        menuStrip = new MenuStrip
        {
            Visible = false
        };
        this.Controls.Add(menuStrip);

        MaakFileMenu();
        MaakToolMenu(deTools);
        MaakActieMenu(deKleuren);
        MaakToolButtons(deTools);
        MaakActieButtons(deKleuren);

        this.Resize += VeranderAfmeting;
        VeranderAfmeting(null, null);
    }

    private void VeranderAfmeting(object sender, EventArgs e)
    {
        schetscontrol.Size = new Size(this.ClientSize.Width - 70, this.ClientSize.Height - 50);
        paneel.Location = new Point(64, this.ClientSize.Height - 30);
    }

    private void KlikToolMenu(object sender, EventArgs e)
    {
        huidigeTool = (ISchetsTool)((ToolStripMenuItem)sender).Tag;
    }

    private void KlikToolButton(object sender, EventArgs e)
    {
        huidigeTool = (ISchetsTool)((RadioButton)sender).Tag;
    }

    private void Afsluiten(object sender, EventArgs e)
    {
        this.Close();
    }

    private void MaakFileMenu()
    {
        ToolStripMenuItem menu = new ToolStripMenuItem("File")
        {
            MergeAction = MergeAction.MatchOnly
        };

        menu.DropDownItems.Add("Sluiten", null, Afsluiten);
        menuStrip.Items.Add(menu);
    }

    private void MaakToolMenu(ICollection<ISchetsTool> tools)
    {
        ToolStripMenuItem menu = new ToolStripMenuItem("Tool");

        foreach (ISchetsTool tool in tools)
        {
            ToolStripItem item = new ToolStripMenuItem
            {
                Tag = tool,
                Text = tool.ToString(),
                // Image = new Bitmap($"../../../Icons/{tool}.png")
            };

            item.Click += KlikToolMenu;
            menu.DropDownItems.Add(item);
        }

        menuStrip.Items.Add(menu);
    }

    private void MaakActieMenu(string[] kleuren)
    {
        ToolStripMenuItem menu = new ToolStripMenuItem("Actie");
        menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon);
        menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer);

        ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
        foreach (string k in kleuren)
        {
            submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
        }

        menu.DropDownItems.Add(submenu);
        menuStrip.Items.Add(menu);
    }

    private void MaakToolButtons(ICollection<ISchetsTool> tools)
    {
        int t = 0;

        foreach (ISchetsTool tool in tools)
        {
            RadioButton b = new RadioButton
            {
                Appearance = Appearance.Button,
                Size = new Size(45, 62),
                Location = new Point(10, 10 + t * 62),
                Tag = tool,
                Text = tool.ToString(),
                // Image = new Bitmap($"../../../Icons/{tool}.png"),
                TextAlign = ContentAlignment.TopCenter,
                ImageAlign = ContentAlignment.BottomCenter
            };

            b.Click += KlikToolButton;
            this.Controls.Add(b);

            if (t == 0) b.Select();
            t++;
        }
    }

    private void MaakActieButtons(string[] kleuren)
    {
        paneel = new Panel
        {
            Size = new Size(600, 24)
        };
        this.Controls.Add(paneel);

        Button clear = new Button
        {
            Text = "Clear",
            Location = new Point(0, 0)
        };
        clear.Click += schetscontrol.Schoon;
        paneel.Controls.Add(clear);

        Button rotate = new Button
        {
            Text = "Rotate",
            Location = new Point(80, 0)
        };
        rotate.Click += schetscontrol.Roteer;
        paneel.Controls.Add(rotate);

        Label penkleur = new Label
        {
            Text = "Penkleur:",
            Location = new Point(180, 3),
            AutoSize = true
        };
        paneel.Controls.Add(penkleur);

        ComboBox cbb = new ComboBox
        {
            Location = new Point(240, 0),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
        foreach (string k in kleuren)
        {
            cbb.Items.Add(k);
        }
        cbb.SelectedIndex = 0;
        paneel.Controls.Add(cbb);
    }
}
