using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {

	public int port = 6321;

	private List <ServerClient> clients;
	private List <ServerClient> disconnectList;

	private TcpListener server;
	private bool serverStarted;

	public void Update () {
		if (!serverStarted)
			return;

		foreach (ServerClient c in clients) {
			if (!IsConnected (c.TCP)) {
				c.TCP.Close ();
				disconnectList.Add (c);
				continue;
			} else {
				NetworkStream s = c.TCP.GetStream ();
				if (s.DataAvailable) {
					StreamReader reader = new StreamReader (s, true);
					string data = reader.ReadLine ();

					if (data != null) {
						OnIncomingData (c, data);
					}
				}
			}
		}

		for (int i = 0; i < disconnectList.Count - 1; i++) {
			clients.Remove (disconnectList [i]);
			disconnectList.RemoveAt (i);
		}
	}

	public void init () {
		DontDestroyOnLoad (gameObject);
		clients = new List <ServerClient> ();
		disconnectList = new List <ServerClient> ();

		try {
			server = new TcpListener (IPAddress.Any, port);
			server.Start ();

			startListening ();
			serverStarted = true;
		}
		catch (Exception e) {
			Debug.Log ("Server error" + e.Message);
		}
	}

	private void startListening () {
		server.BeginAcceptTcpClient (AcceptTCPClient, server);
	}

	private void AcceptTCPClient (IAsyncResult ar) {
		TcpListener listener = (TcpListener)ar.AsyncState;
		ServerClient sc = new ServerClient (listener.EndAcceptTcpClient (ar));

		clients.Add (sc);

		startListening ();
		Debug.Log ("A person has joined");
	}

	private void OnIncomingData (ServerClient c, string data) {
		Debug.Log (c.ClientName + " : " + data);
	}

	private bool IsConnected (TcpClient c) {
		try {
			if (c != null && c.Client != null && c.Client.Connected != null) {
				if (c.Client.Poll (0, SelectMode.SelectRead)) {
					return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
				}
				return true;
			} else {
				return false;
			}
		}
		catch {
			return false;
		}
	}
}

public class ServerClient {
	public string ClientName;
	public TcpClient TCP;

	public ServerClient (TcpClient tcp) {
		this.TCP = tcp;
	}
}