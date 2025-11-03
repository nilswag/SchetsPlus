
using schetsplus.src.tools;

namespace schetsplus.src
{
    internal class SchetsWindow : Form
    {
        private MenuStrip menuStrip;
        private SchetsControl schetsControl;
        private ISchetsTool currentTool;
        private Panel panel;
        private bool mousePressed;

        public SchetsWindow()
        {
            ISchetsTool[] tools = 
            { 
                new PenTool(),
                new LijnTool(),
                new RechthoekTool(),
                new VolRechthoekTool(),
                new TextTool(),
                new GumTool()
            };
            String[] colors = { "Black", "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan" };

            ClientSize = new Size(700, 500);
            currentTool = tools[0];

            schetsControl = new SchetsControl();
            schetsControl.Location = new Point(64, 10);
            schetsControl.MouseDown += (object o, MouseEventArgs e) =>
            {
                mousePressed = true;
                currentTool.MouseDown(schetsControl, e.Location);
            };
            schetsControl.MouseUp += (object o, MouseEventArgs e) =>
            {
                mousePressed = false;
                currentTool.MouseUp(schetsControl, e.Location);
            };
            schetsControl.MouseMove += (object o, MouseEventArgs e) =>
            {
                if (mousePressed) currentTool.MouseDrag(schetsControl, e.Location);
            };
            schetsControl.KeyPress += (object o, KeyPressEventArgs e) =>
            {
                currentTool.Letter(schetsControl, e.KeyChar);
            };
            Controls.Add(schetsControl);

            menuStrip = new MenuStrip();
            menuStrip.Visible = false;
            Controls.Add(schetsControl);

            createFileMenu();
            createToolMenu(tools);
            createActionMenu(colors);
            createToolButtons(tools);
            createActionButtons(colors);
            Resize += changeSize;
            changeSize(null, null);
        }

        private void changeSize(object o, EventArgs e)
        {
            schetsControl.Size = new Size(ClientSize.Width - 70, ClientSize.Height - 50);
            panel.Location = new Point(64, ClientSize.Height - 30);
        }

        private void stop(object o, EventArgs e)
        {
            Close();
        }

        private void createFileMenu()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("File");
            menu.MergeAction = MergeAction.MatchOnly;
            menu.DropDownItems.Add("Sluiten", null, stop);
            menuStrip.Items.Add(menu);
        }

        private void createToolMenu(ICollection<ISchetsTool> tools)
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
            foreach (ISchetsTool tool in tools)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Tag = tool;
                item.Text = tool.ToString();
                // item.Image = new Bitmap($"../Icons/{tool.ToString()}.png");
                item.Click += (object o, EventArgs e) => currentTool = (ISchetsTool)((ToolStripMenuItem)o).Tag;
                menu.DropDownItems.Add(item);
            }
            menuStrip.Items.Add(menu);
        }

        private void createActionMenu(String[] colors)
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Actie");
            menu.DropDownItems.Add("Clear", null, schetsControl.Clean);
            menu.DropDownItems.Add("Roteer", null, schetsControl.Rotate);
            ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
             foreach (string k in colors)
                submenu.DropDownItems.Add(k, null, schetsControl.ChangeColorThroughMenu);
            menu.DropDownItems.Add(submenu);
            menuStrip.Items.Add(menu);

        }

        private void createToolButtons(ICollection<ISchetsTool> tools)
        {
            int t = 0;
            foreach (ISchetsTool tool in tools)
            {
                RadioButton b = new RadioButton();
                b.Appearance = Appearance.Button;
                b.Size = new Size(45, 62);
                b.Location = new Point(10, 10 + t * 62);
                b.Tag = tool;
                b.Text = tool.ToString();
                // b.Image = new Bitmap($"../Icons/{tool.ToString()}.png");
                b.TextAlign = ContentAlignment.TopCenter;
                b.ImageAlign = ContentAlignment.BottomCenter;
                b.Click += (object o, EventArgs e) => currentTool = (ISchetsTool)((RadioButton)o).Tag;
                Controls.Add(b);
                if (t == 0) b.Select();
                t++;
            }
        }

        private void createActionButtons(String[] colors)
        {
            panel = new Panel(); Controls.Add(panel);
            panel.Size = new Size(600, 24);
            Button clear = new Button(); panel.Controls.Add(clear);
            clear.Text = "Clear";
            clear.Location = new Point(0, 0);
            clear.Click += schetsControl.Clean;
            Button rotate = new Button(); panel.Controls.Add(rotate);
            rotate.Text = "Rotate";
            rotate.Location = new Point(80, 0);
            rotate.Click += schetsControl.Rotate;
            Label penkleur = new Label(); panel.Controls.Add(penkleur);
            penkleur.Text = "Penkleur:";
            penkleur.Location = new Point(180, 3);
            penkleur.AutoSize = true;
            ComboBox cbb = new ComboBox(); panel.Controls.Add(cbb);
            cbb.Location = new Point(240, 0);
            cbb.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb.SelectedValueChanged += schetsControl.ChangeColor;
            foreach (string k in colors)
                cbb.Items.Add(k);
            cbb.SelectedIndex = 0;

        }

    }
}
