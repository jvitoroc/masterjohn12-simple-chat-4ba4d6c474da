using System;
using Terminal.Gui;

namespace ClientConsole
{
    class EnterChannelWindow : CustomWindow
    {
        private Button btnJoinChannel;
        private Button btnCreateChannel;
        private TextField txtChannelID;
        private Label lblChannelID;

        public Action CreateChannel;
        public Action<string> JoinChannel;

        public EnterChannelWindow()
        {
            win = new Window("Chat - Entrar em um canal")
            {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            btnJoinChannel = new Button(10, 11, "Entrar");
            btnJoinChannel.Clicked = OnJoinChannel;
            btnCreateChannel = new Button(10, 12, "Criar canal", true);
            btnCreateChannel.Clicked = OnCreateChannel;

            txtChannelID = new TextField(10, 10, 40, "");
            lblChannelID = new Label(10, 9, "Cole embaixo o ID do canal que deseja entrar");

            win.Add(btnJoinChannel, btnCreateChannel, txtChannelID, lblChannelID);
        }

        public override void Show()
        {
            base.Show();
            txtChannelID.EnsureFocus();
        }

        private void OnCreateChannel()
        {
            if(CreateChannel != null) 
                CreateChannel();
        }

        private void OnJoinChannel()
        {
            if (JoinChannel != null)
                JoinChannel(txtChannelID.Text.ToString());
        }
    }
}
