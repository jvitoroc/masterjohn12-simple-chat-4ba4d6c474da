using System;
using System.Windows.Forms;

namespace ClientForm
{
    enum ClientState
    {
        Disconnected,
        ServerConnected,
        ChannelConnected
    }

    public partial class Main : Form
    {
        private ClientForm.Channel channel;
        private Server server;
        private ClientState state;
        private ClientState State { get { return state; } set { SetState(value); } }

        public Main()
        {
            InitializeComponent();
            channel = new ClientForm.Channel();
            channel.MdiParent = this;
            channel.Dock = DockStyle.Fill;
            State = ClientState.Disconnected;
        }

        private void SetState(ClientState state)
        {
            this.state = state;
            switch (State)
            {
                case ClientState.Disconnected:
                    channelToolStripMenuItem.Enabled = false;
                    serverToolStripMenuItem.Enabled = true;
                    channel.Hide();
                    break;
                case ClientState.ServerConnected:
                    channelToolStripMenuItem.Enabled = true;
                    serverToolStripMenuItem.Enabled = true;
                    channel.Hide();
                    break;
                case ClientState.ChannelConnected:
                    channelToolStripMenuItem.Enabled = true;
                    serverToolStripMenuItem.Enabled = true;
                    channel.Show();
                    break;
            }
        }

        public void UpdateChannelForm(string channelId, string[] usernames)
        {
            channel.Server = server;
            channel.ChannelID = channelId;
            channel.UpdateUsers(usernames);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectServer connectServer = new ConnectServer();
            try
            {
                if (connectServer.ShowDialog() == DialogResult.OK)
                {
                    Register register = new Register();
                    try
                    {
                        register.Server = connectServer.Server;
                        if (register.ShowDialog() == DialogResult.OK)
                        {
                            server = connectServer.Server;
                            State = ClientState.ServerConnected;
                        }
                        else
                        {
                            connectServer.Server.Disconnect();
                        }
                    }
                    finally
                    {
                        register.Dispose();
                    }
                }
            }
            finally
            {
                connectServer.Dispose();
            }
        }

        private void channelConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((bool)server?.TextConnected)
            {
                ConnectChannel connectChannel = new ConnectChannel();
                try
                {
                    connectChannel.Server = server;
                    if (connectChannel.ShowDialog() == DialogResult.OK)
                    {
                        State = ClientState.ChannelConnected;
                        UpdateChannelForm(connectChannel.Response.GetHeaderValue("channel_id"), connectChannel.Response.GetHeaderValue("usernames").Split(','));
                    }
                }
                finally
                {
                    connectChannel.Dispose();
                }
            }
        }

        private void createChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.Message m = server.CreateChannel();
            if (!m.ResponseCode.ToString().StartsWith("1"))
            {
                MessageBox.Show("Mensagem do servidor: " + m.message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m = server.JoinChannel(m.GetHeaderValue("channel_id"));
            if (!m.ResponseCode.ToString().StartsWith("1"))
            {
                MessageBox.Show("Mensagem do servidor: " + m.message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (m.ResponseCode.ToString().StartsWith("1"))
            {
                MessageBox.Show("Mensagem do servidor: " + m.message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            UpdateChannelForm(m.GetHeaderValue("channel_id"), m.GetHeaderValue("usernames").Split(','));
            State = ClientState.ChannelConnected;
        }
    }
}
