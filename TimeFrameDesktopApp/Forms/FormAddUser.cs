using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeFrameDesktopApp.DTOs;
using TimeFrameDesktopApp.ServiceLayer;
using Tulpep.NotificationWindow;

namespace TimeFrameDesktopApp.Forms
{
    public partial class FormAddUser : Form
    {
        RestApiDataAccess restApi = new RestApiDataAccess("https://localhost:5001/api");
        public FormAddUser()
        {
            InitializeComponent();
        }

        private async void bttnOk_Click(object sender, EventArgs e)
        {
            RegisterDto register = new RegisterDto()
            {
                FirstName = textBoxFName.Text,
                LastName = textBoxLName.Text,
                Email = textBoxEmail.Text,
                Password = textBoxPass.Text
            };
            if (await restApi.RegisterUserAsync(register))
            {
                textBoxFName.Clear();
                textBoxLName.Clear();
                textBoxEmail.Clear();
                textBoxPass.Clear();
                DialogResult result = MessageBox.Show("Added user to database", "Success", MessageBoxButtons.OK);
            } else
            {
                textBoxPass.Clear();
                DialogResult result = MessageBox.Show("Failed to add user, please check user doesnt already exist.", "Error", MessageBoxButtons.OK);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
