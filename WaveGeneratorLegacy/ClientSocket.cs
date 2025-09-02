using System.Net;
using System.Net.Sockets;
using System.Text;
using TartarosLogger;

namespace WaveGeneratorLegacy
{
	class ClientSocket
	{
		public string ServerAddress;
		public int Port;

		private IPAddress? _ipAddress;
		private IPEndPoint? _remoteEndPoint;
		private Socket? _socket;

		public ClientSocket(string serverAddress, int port)
		{
			this.ServerAddress = serverAddress;
			this.Port = port;
			try
			{
				_ipAddress = IPAddress.Parse(this.ServerAddress);
				_remoteEndPoint = new IPEndPoint(_ipAddress, this.Port);
				_socket = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				try
				{
					Logger.Info($"Connecting... [{this.ServerAddress}:{this.Port}]");
					_socket.Connect(_remoteEndPoint);
					Logger.Info($"Connecting finished");
					if (_socket.RemoteEndPoint != null)
					{
						Console.WriteLine("Socket connected to " + _socket.RemoteEndPoint.ToString());
					}
				}
				catch (SocketException)
				{
					Logger.Error($"Socket not responding");
					Environment.Exit(1);
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message}");
				Environment.Exit(2);
			}
		}

		public void Disconnect()
		{
			if (_socket != null && _socket.IsBound)
			{
				Logger.Info("Closing socket");
				_socket.Shutdown(SocketShutdown.Both);
				_socket.Close();
			}
		}

		public void SendData(string data)
		{
			Thread.Sleep(10);
			try
			{
				byte[] bDataToServer = Encoding.UTF8.GetBytes(data + '\n');
				Logger.Debug("Data Length: " + bDataToServer.Length);
				if (_socket != null)
				{
					int bytesSend = _socket.Send(bDataToServer);
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
		}
	}
}
