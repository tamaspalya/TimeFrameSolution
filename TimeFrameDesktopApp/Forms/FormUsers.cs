using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeFrameDesktopApp.DTOs;
using TimeFrameDesktopApp.ServiceLayer;

namespace TimeFrameDesktopApp.Forms
{
    public partial class FormUsers : Form
    {
        private ITimeFrameService _dataAccess;
        private List<UserDto> _users;

        public FormUsers()
        {
            InitializeComponent();
            _dataAccess = new RestApiDataAccess("https://localhost:44319/api");
        }

        #region Event handlers
        private async void FormUsers_Load(object sender, EventArgs e)
        {
            await GetUsersFromDatabase();
        }
        private async void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                UserDto user = (UserDto)dataGridUsers.CurrentRow.Tag;
                if (MessageBox.Show("Delete user with id: " + user.Id + "?", "CONFIRM", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    await RemoveUser(user);
                    txtBoxSearchBar.Clear();
                    PopulateData(_users);
                }
            }
        }
        private void txtBoxSearchBar_TextChanged(object sender, EventArgs e)
        {
            ReloadData();
        }
        private async void btnFetchUsers_Click(object sender, EventArgs e)
        {
            await FetchDatabase();
        }
        #endregion

        #region Helper methods
        private async Task GetUsersFromDatabase()
        {
            DisableUiControls();
            try
            {
                IEnumerable<UserDto> users = await _dataAccess.GetAllAsync();
                _users = users.ToList();
                PopulateData(_users);
                EnableUiControls();
            }
            catch (Exception)
            {
                MessageBox.Show("Could not fetch user data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _users = null;
                btnFetchUsers.Enabled = true;
            }
        }
        private void PopulateData(List<UserDto> users)
        {
            if (users != null)
            {
                dataGridUsers.Rows.Clear();
                foreach (var user in users)
                {
                    dataGridUsers.Rows.Add(new object[] {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        "Edit",
                        "Delete"
                    });
                    dataGridUsers.Rows[dataGridUsers.RowCount - 1].Tag = user;
                }
            }
        }
        private async Task RemoveUser(UserDto user)
        {
            try
            {
                await _dataAccess.DeleteUserById(user.Id);
                _users.Remove(user);
            }
            catch (Exception)
            {
                MessageBox.Show($"Could not remove a user by id: { user.Id}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await GetUsersFromDatabase();
                PopulateData(_users);
            }
        }
        private void DisableUiControls()
        {
            txtBoxSearchBar.Enabled = false;
            btnNewUser.Enabled = false;
            dataGridUsers.Enabled = false;
            btnFetchUsers.Enabled = false;
        }
        private void EnableUiControls()
        {
            txtBoxSearchBar.Enabled = true;
            btnNewUser.Enabled = true;
            dataGridUsers.Enabled = true;
            btnFetchUsers.Enabled = true;
        }
        private void ReloadData()
        {
            string searchCriteria = txtBoxSearchBar.Text.Trim();
            if (searchCriteria.Length > 0)
            {
                IEnumerable<UserDto> results = _users.Where(user => user.Email.Contains(searchCriteria) || user.FirstName.Contains(searchCriteria) || user.LastName.Contains(searchCriteria));
                PopulateData(results.ToList());
            }
            else
            {
                PopulateData(_users);
            }

        }
        private async Task FetchDatabase()
        {
            txtBoxSearchBar.Clear();
            btnFetchUsers.Enabled = false;
            lblFetch.Text = "Fetching user data, please wait...";
            await GetUsersFromDatabase();
            btnFetchUsers.Enabled = true;
            if (_users != null)
            {
                lblFetch.Text = $"User data last fetched: {DateTime.Now:HH:mm}";
            }
            else
            {
                lblFetch.Text = "Fetch failed";
                dataGridUsers.Rows.Clear();
            }
        }

        #endregion

        private void btnNewUser_Click(object sender, EventArgs e)
        {
                FormAddUser useradd = new FormAddUser();
                useradd.Show();
            
        }
    }
}
