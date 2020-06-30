using System;
using System.Net;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class ConnectServer : Form
    {
        private Server server;

        public Server Server { get => server; }

        public ConnectServer()
        {
            InitializeComponent();
            server = new Server();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress ipAddress = IPAddress.Parse(txtIP.Text);
            server.ConnectTextServer(new IPEndPoint(ipAddress, int.Parse(txtPortaTCP.Text)));
            server.ConnectAudioServer(new IPEndPoint(ipAddress, int.Parse(txtPortaUDP.Text)));
            MessageBox.Show("Conectado ao servidor", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void txtPorta_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
