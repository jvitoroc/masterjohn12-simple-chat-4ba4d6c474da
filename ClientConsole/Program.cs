using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using Terminal.Gui;

namespace ClientConsole
{
    class Program
    {
        private static LoginWindow loginWindow;
        private static EnterChannelWindow enterChannelWindow;
        private static ChannelWindow channelWindow;

        public static Server server;

        static void Main(string[] args)
        {
			Application.Init();

            enterChannelWindow = new EnterChannelWindow();
            enterChannelWindow.CreateChannel = OnCreateChannel;
            enterChannelWindow.JoinChannel = OnJoinChannel;

            loginWindow = new LoginWindow();
            loginWindow.OnTryConnect = OnTryConnect;
            loginWindow.Show();

            Console.CancelKeyPress += CancelKeyPress;

            Application.Run();
		}

        private static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private static void OnTryConnect()
        {
            TcpClient _server = new TcpClient(loginWindow.ServerIP, 13000);
            server = new Server(_server);
            server.RegisterMyself(loginWindow.Username);
            server.ChannelChanged = OnChannelChange;
            server.ChannelUsersUpdated = UpdateChannelUsers;
            server.TextMessageReceived = ReceiveTextMessage;
            enterChannelWindow.Show();
        }

        private static void OnCreateChannel()
        {
            try
            {
                server.CreateChannel();
            }
            catch (Exception e)
            {
                //MessageBox.ErrorQuery(100, 50, "Erro", "Ocorreu um problema ao conectar ao servidor: " + e.Message, "OK");
            }
        }

        private static void OnTrySend(string message)
        {
            try
            {
                server.SendTextMessage(message);
            }
            catch (Exception e)
            {
                //MessageBox.ErrorQuery(100, 50, "Erro", "Ocorreu um problema ao conectar ao servidor: " + e.Message, "OK");
            }
        }

        private static void OnChannelChange()
        {
            channelWindow = new ChannelWindow();
            channelWindow.TrySend = OnTrySend;
            channelWindow.Show();
        }

        private static void OnJoinChannel(string channelId)
        {
            server.JoinChannel(channelId);
        }

        private static void OnChannelLeave()
        {
            channelWindow = null;
            enterChannelWindow.Show();
        }

        private static void UpdateChannelUsers(string[] usernames)
        {
            if(channelWindow != null)
            {
                channelWindow.UpdateChannelUsers(usernames);
            }
        }

        private static void ReceiveTextMessage(string message, string from)
        {
            if (channelWindow != null)
            {
                channelWindow.AddMessage(message, from);
            }
        }
    }
}
