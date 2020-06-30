using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Terminal.Gui;

namespace ClientConsole
{
    public class ChannelWindow : CustomWindow
    {
		private Window winChat;
		private Window winUsers;
		private ListView lvUsers;
		private TextField txtMessage;
		private TextView txtMessages;
		private Button btnSend;
		private List<string> usernames;

		public Action<string> TrySend;

		public ChannelWindow()
		{
			win = new Window("Você está conectado em um canal")
			{
				X = 0,
				Y = 0,

				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			winUsers = new Window("Usuários")
			{
				X = 0,
				Y = 0,

				Width = Dim.Percent(30),
				Height = Dim.Fill()
			};

			winChat = new Window("Chat")
			{
				X = Pos.Right(winUsers),
				Y = 0,

				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			txtMessage = new TextField("")
			{
				X = 0,
				Y = Pos.Bottom(winChat) - 3,

				Width = Dim.Percent(80),
				Height = 1
			};

			txtMessages = new TextView()
			{
				ColorScheme = Colors.TopLevel,
				ReadOnly = true,

				X = 0,
				Y = 0,

				Width = Dim.Fill(),
				Height = Dim.Height(winChat) - Dim.Height(txtMessage) - 5
			};

			winChat.Add(txtMessages);

			winChat.Add(txtMessage);

			btnSend = new Button("Enviar", true)
			{
				X = Pos.Right(txtMessage),
				Y = Pos.Top(txtMessage)
			};

			btnSend.Clicked = OnSend;

			winChat.Add(btnSend);

			usernames = new List<string>();

			lvUsers = new ListView(usernames)
			{
				X = 0,
				Y = 0,

				Width = Dim.Fill(),
				Height = Dim.Fill()
			};

			winUsers.Add(lvUsers);

			win.Add(
				winUsers,
				winChat
			);
		}

		public override void Show()
		{
			base.Show();
		}

		public void UpdateChannelUsers(string[] usernames)
		{
			this.usernames.Clear();
			for (int i = 0; i < usernames.Length; i++)
            {
				this.usernames.Add(usernames[i]);
            }
		}

		public void AddMessage(string message, string from)
		{
			txtMessages.Text += from + ": " + message + '\n';
		}

		private void OnSend()
		{
			if(TrySend != null)
            {
				TrySend(txtMessage.Text.ToString());
				txtMessage.Text = "";
			}
		}
	}
}
