using System;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Register : Form
    {
        public Server Server;

        public Register()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Common.Message m = Server.RegisterMyself(txtUsername.Text);
            if (m.ResponseCode.ToString().StartsWith("1"))
            {
                MessageBox.Show("Mensagem do servidor: " + m.message, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Mensagem do servidor: " + m.message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
