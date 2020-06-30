using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace ClientConsole
{
    public class LoginWindow : CustomWindow
    {
		private Label lblUsername;
		private Label lblServerIP;
		private TextField txtUsername;
		private TextField txtServerIP;
		private Button btnConnect;

		public Action OnTryConnect;
		public string ServerIP { get { return txtServerIP.Text.ToString(); } }
		public string Username { get { return txtUsername.Text.ToString(); } }

		public LoginWindow()
        {
			win = new Window("Chat - Conectar em um servidor")
			{
				X = 0,
				Y = 0,

				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			lblUsername = new Label("Nome: ") { X = 3, Y = 2 };
			lblServerIP = new Label("IP do Servidor: ")
			{
				X = Pos.Left(lblUsername),
				Y = Pos.Top(lblUsername) + 2
			};

			txtUsername = new TextField("")
			{
				X = Pos.Right(lblServerIP),
				Y = Pos.Top(lblUsername),
				Width = 30
			};

			txtServerIP = new TextField("127.0.0.1")
			{
				X = Pos.Left(txtUsername),
				Y = Pos.Top(lblServerIP),
				Width = Dim.Width(txtUsername)
			};

			btnConnect = new Button(3, 10, "Connect", true);
			btnConnect.Clicked = OnConnectClick;

			win.Add(
				lblUsername, lblServerIP, txtUsername, txtServerIP,
				btnConnect
			);
		}

		public override void Show()
        {
			base.Show();
			txtUsername.EnsureFocus();
		}

		private void OnConnectClick()
		{
			OnTryConnect();
		}
	}
}
