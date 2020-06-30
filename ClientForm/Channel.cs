using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Channel : Form
    {
        private Server server;
        private readonly SynchronizationContext syncContext;
        private readonly Font boldFont;
        private readonly Font regularFont;

        public Server Server { get { return server; } set { SetServer(value); } }
        public string ChannelID { get { return txtChannelID.Text; } set { txtChannelID.Text = value; } }

        public Channel()
        {
            InitializeComponent();
            syncContext = AsyncOperationManager.SynchronizationContext;
            boldFont = new Font(txtMessageLog.Font, FontStyle.Bold);
            regularFont = new Font(txtMessageLog.Font, FontStyle.Regular);
        }

        public void SetServer(Server server)
        {
            this.server = server;
            this.server.ChannelUsersUpdated = UpdateUsers;
            this.server.TextMessageReceived = AddMessage;
            txtMessageLog.Clear();
            txtMessageText.Clear();
            lvUsers.Items.Clear();
        }

        public void UpdateUsers(string[] usernames)
        {
            syncContext.Post((e) =>
            {
                lvUsers.Items.Clear();
                for (int i = 0; i < usernames.Length; i++)
                {
                    ListViewItem lvItem = new ListViewItem(usernames[i]);
                    lvUsers.Items.Add(lvItem);
                }
            }, null);
        }

        public void AddMessage(string message, string from)
        {
            syncContext.Post((e) =>
            {
                txtMessageLog.SelectionFont = boldFont;
                txtMessageLog.AppendText(from + ": ");
                txtMessageLog.SelectionFont = regularFont;
                txtMessageLog.AppendText(message + "\r\n");
            }, null);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessageText.Text.Trim() != "")
            {
                this.server.SendTextMessage(txtMessageText.Text);
                txtMessageText.Text = "";
            }
        }

        private void Channel_Shown(object sender, EventArgs e)
        {
            server.AudioManager.StartListening();
            server.AudioManager.StartRecording();
        }
    }
}
