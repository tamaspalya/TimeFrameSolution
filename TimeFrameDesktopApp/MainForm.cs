using System;
using System.Drawing;
using System.Windows.Forms;
using TimeFrameDesktopApp.Forms;

namespace TimeFrameDesktopApp
{
    public partial class MainForm : Form
    {
        //Fields
        private Button _currentButton;
        private Form _activeForm;
        private FormUsers _usersForm;

        //Constructor
        public MainForm()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1280, 720);
        }

        #region Event handlers
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
            OpenChildForm(_usersForm, sender);
        }

        private void btnProjects_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormProjects(), sender);
        }

        private void btnProjectTemplates_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormProjectTemplates(), sender);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSettings(), sender);
        }
        #endregion

        #region Helper methods
        private void LoadUsers()
        {
            _usersForm = new FormUsers();
        }
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (_currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = Color.Indigo;
                    _currentButton = (Button)btnSender;
                    _currentButton.BackColor = color;
                    _currentButton.ForeColor = Color.White;
                    _currentButton.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
        }
        private void DisableButton()
        {
            foreach (Control button in pnlNavigationMenu.Controls)
            {
                if (button.GetType() == typeof(Button))
                {
                    button.BackColor = Color.FromArgb(51, 51, 76);
                    button.ForeColor = Color.White;
                    button.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
        }
        private void OpenChildForm(Form childForm, object btnSender)
        {
            if (_activeForm != null)
            {
                _activeForm.Hide();
            }

            ActivateButton(btnSender);
            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.pnlDesktopPanel.Controls.Add(childForm);
            this.pnlDesktopPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitle.Text = childForm.Text;
        }
        #endregion
    }
}
