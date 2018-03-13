using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MessageHandler 
{
    //just a random number
    private const short chatMessage = 131;

    
    // Use this for initialization
    

    public static void Register() {
        if (NetworkServer.active)
        {
            //registering the server handler
            NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
        }
        NetworkManager.singleton.client.RegisterHandler(chatMessage, ReceiveMessage);
    }

    private static void ServerReceiveMessage(NetworkMessage message)
    {
        StringMessage myMessage = new StringMessage();
        //we are using the connectionId as player name only to exemplify
        int player = message.conn.connectionId;
        myMessage.value = message.ReadMessage<StringMessage>().value;

        //sending to all connected clients
        NetworkServer.SendToAll(chatMessage, myMessage);
    }

    private static void ReceiveMessage(NetworkMessage message)
    {
        //reading message
        string text = message.ReadMessage<StringMessage>().value;
        //// INPUT WHAT TO DO WITH MESSAGE

        //Inserts switch statement for gamestate
        switch (text)
        {
            case ("Connected"):
                SceneManager.LoadScene("Scenes/BattleScene");
                break;
            default:
                GameManager.instance.TileHit(text);
                break;

        }

        
    }

    public static void SendStringMessage(string input)
    {
        StringMessage myMessage = new StringMessage();
        myMessage.value = input;

        //sending to server
        Debug.Log("SendStringMessage");
        NetworkManager.singleton.client.Send(chatMessage, myMessage);
    }
}
