using Assets.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Network : MonoBehaviour
{
    public string ipAddress = "localhost";
    public int port = 9000;

    private TcpClient m_Client;
    private static NetworkStream m_NetStream = null;
    private byte[] m_Buffer = new byte[49152];

    public static Dictionary<string, Action<object>> NetworkMessageHandles = new Dictionary<string, Action<object>>()
    {
        { "MoveResult", HandleMove },
        { "OnRegisterCallback", RegistrationCallBack },
        { "OnLoginCallback", LoginCallback },
        { "InitializeGame",  InitializeGame },
        { "Invitation", InvitationReceived },
        { "MoveSuccess", OnMoveSuccess },
        { "MoveFailure", OnMoveFailure },
        { "GameResult", OnGameEnd },
        { "GameHistory", OnGameHistoryReceived },
    };

    private void Awake()
    {
        StartConnection();
    }

    /// <summary>
    /// Open TCP connection
    /// </summary>
    void StartConnection()
    {
        if(m_Client != null)
        {
            return;
        }

        Debug.Log("Starting connection");

        m_Client = new TcpClient(ipAddress, port);
        m_NetStream = m_Client.GetStream();
        m_NetStream.BeginRead(m_Buffer,0,m_Buffer.Length, new AsyncCallback(ReadAsyncCallback), m_NetStream);
    }

    /// <summary>
    /// Close TCP connection
    /// </summary>
    void CloseConnection()
    {
        m_NetStream.Close();
        m_Client.Close();
    }

    /// <summary>
    /// Send NetworkMessage class to server
    /// </summary>
    /// <param name="message"></param>
    public static void SendDataToServer(string messageType, object data)
    {
        NetworkMessage message = new NetworkMessage(messageType, data);
        var bla = JsonConvert.SerializeObject(message);
        Debug.Log($"Data to send: {bla}");
        ASCIIEncoding encoder = new ASCIIEncoding();
        byte[] buffer = encoder.GetBytes(bla);
        m_NetStream.Write(buffer, 0, buffer.Length);
        m_NetStream.Flush();
    }

    /// <summary>
    /// Callback method for reading network stream
    /// </summary>
    /// <param name="ar"></param>
    public void ReadAsyncCallback(IAsyncResult ar)
    {
        try
        {
            int bytesRead = m_NetStream.EndRead(ar);
            if (bytesRead > 0)
            {
                string text = Encoding.UTF8.GetString(m_Buffer, 0, bytesRead);
                HandleData(text);
            }

            Array.Clear(m_Buffer, 0, m_Buffer.Length);
            m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, new AsyncCallback(ReadAsyncCallback), m_NetStream);
        }
        catch (Exception error)
        {
            Dispatcher.RunOnMainThread(() =>
            {
                Debug.Log($"{error.Message} \n {error.StackTrace}");
                CloseConnection();
            });
        }
    }

    /// <summary>
    /// Method that deserializes json string and calls Actions from NetworkMessageHandles dictionary depending on the NetworkMessage.messageName
    /// </summary>
    /// <param name="jsonString"></param>
    public void HandleData(string jsonString)
    {
        var deserializedObj = JsonConvert.DeserializeObject<NetworkMessage>(jsonString);
        NetworkMessageHandles[deserializedObj.messageName].Invoke(deserializedObj.data);
    }

    public static void HandleMove(object data)
    {
        Dispatcher.RunOnMainThread(() =>
        {
            Debug.Log("HandleMove goes here");
        });
    }

    public static void RegistrationCallBack(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.RegistrationCallback, data));
    }

    public static void LoginCallback(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.LoginCallback, data));
    }

    public static void InitializeGame(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.InitializeGame, data));
    }

    public static void InvitationReceived(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.OnInvitationReceived, data));
    }

    public static void OnMoveSuccess(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.MoveSuccess, data));
    }

    public static void OnMoveFailure(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.MoveFailure, data));
    }

    public static void OnGameEnd(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.GameEnd, data));
    }

    public static void OnGameHistoryReceived(object data)
    {
        Dispatcher.RunOnMainThread(() => MessagingSystem.DispatchMessage(MessageType.GameHistoryReceived, data));
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    private void OnApplicationQuit()
    {
        CloseConnection();
    }
}

public class TestData
{
    public string message;

    public TestData() { }
    public TestData(string _message)
    {
        message = _message;
    }
}
