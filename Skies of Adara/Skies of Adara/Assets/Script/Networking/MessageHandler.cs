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
    public static bool loaded = false;

    
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
        //we are using the connectionId as player name only to exemplify
        //int player = message.conn.connectionId;
        string text = message.ReadMessage<StringMessage>().value;
        if (text == "Connected")
        {
            StringMessage newmessage = new StringMessage();
            newmessage.value = "Connected";
            NetworkServer.SendToAll(chatMessage, newmessage);
            /*StringMessage Message = new StringMessage();
            int firstplayer = Random.Range(0, 2);
            Message.value = "Start";
            NetworkServer.SendToClient(firstplayer, chatMessage, Message);
            StringMessage Message1 = new StringMessage();
            Message1.value = "Wait";
            NetworkServer.SendToClient(1 - firstplayer, chatMessage, Message1);*/

        }
        //sending to all connected clients
        else
        {
            StringMessage myMessage = new StringMessage();
            myMessage.value = text;
            NetworkServer.SendToAll(chatMessage, myMessage);
        }
    }

    private static void ReceiveMessage(NetworkMessage message)
    {
        //reading message
        string text = message.ReadMessage<StringMessage>().value;
        //// INPUT WHAT TO DO WITH MESSAGE
        Debug.Log(text);
        //Inserts switch statement for gamestate
        switch (text)
        {
            case ("Connected"):                
                SceneManager.LoadScene("Scenes/BattleScene");
                break;
            case ("Start"):
                GameManager.instance.StartFirst = true;
                break;
            case ("Wait"):
                GameManager.instance.StartFirst = false;
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
