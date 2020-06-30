using System;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class ConnectChannel : Form
    {
        public Server Server;
        public Common.Message Response;


        public ConnectChannel()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Response = Server.JoinChannel(txtChannelID.Text);
            if (Response.ResponseCode.ToString().StartsWith("1"))
            {
                MessageBox.Show("Mensagem do servidor: " + Response.message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Mensagem do servidor: " + Response.message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
