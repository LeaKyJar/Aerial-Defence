using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public static class MessageHandler 
{
    //just a random number
    private const short chatMessage = 131;
    //public static bool loaded = false;
    static bool ready = false;
    static bool accept = false;
    public static bool listener = false;
    //public static String previousMessage;


    // Use this for initialization


    public static void Register() {
        if (NetworkServer.active)
        {
            //registering the server handler
            NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
        }
        NetworkManager.singleton.client.RegisterHandler(chatMessage, ReceiveMessage);
        //NetworkManager.singleton.dontDestroyOnLoad = true;
    }

    private static void ServerReceiveMessage(NetworkMessage message)
    {
        //we are using the connectionId as player name only to exemplify
        //int player = message.conn.connectionId;
        string text = message.ReadMessage<StringMessage>().value;
        StringMessage myMessage;
        switch (text)
        {
            case ("Connected"):
                myMessage = new StringMessage();
                myMessage.value = text;
                NetworkServer.SendToAll(chatMessage, myMessage);
                break;
            case ("Deny"):
                myMessage = new StringMessage();
                myMessage.value = text;
                NetworkServer.SendToAll(chatMessage, myMessage);
                break;
            case ("Ready"):
                if (!ready)
                {
                    ready = true;
                }
                else
                {
                    StringMessage Message = new StringMessage();
                    int firstplayer = UnityEngine.Random.Range(0, 2);
                    Message.value = "First";
                    NetworkServer.SendToClient(firstplayer, chatMessage, Message);
                    StringMessage Message1 = new StringMessage();
                    Message1.value = "Second";
                    NetworkServer.SendToClient(1 - firstplayer, chatMessage, Message1);
                }
                break;
            case ("Accept"):
                if (!accept)
                {
                    accept = true;
                }
                else
                {
                    myMessage = new StringMessage();
                    myMessage.value = "Start";
                    NetworkServer.SendToAll(chatMessage, myMessage);
                    break;
                }
                break;
            default:
                int sender = message.conn.connectionId;
                myMessage = new StringMessage();
                myMessage.value = text;
                NetworkServer.SendToClient(1 - sender, chatMessage, myMessage);
                break;
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
                //SceneManager.LoadScene("Scenes/BattleScene");
                CustomNetworkManager.instance.inmatchmaking = false;
                CustomNetworkManager.instance.changeScene();
                CustomNetworkManager.instance.waitingrequests = true;
                CustomNetworkManager.instance.expiry = Time.time + 10f;
                break;

            case ("Start"):
                CustomNetworkManager.instance.waitingrequests = false;
                CustomNetworkManager.instance.startGame();
                break;

            case ("Deny"):
                CustomNetworkManager.instance.cancelrequest();
                break;
            case ("First"):
                GameManager.instance.StartFirst = true;
                GameManager.instance.ExitPreparation();
                break;
            case ("Second"):
                GameManager.instance.StartFirst = false;
                GameManager.instance.ExitPreparation();
                break;
            case ("YourTurn"):
                GameManager.instance.StartTurn();
                break;
            case ("Victory"):
                GameManager.instance.WinGame();
                CustomNetworkManager.instance.StopMatchMaking();
                break;
            case("Hit Received"):
                Debug.Log("Previous Message");
                GameManager.instance.EmptyEnemyTile();
                break;
            default:
                //Your tile that enemy has hit
                Debug.Log(text);
                if (Int32.Parse(text) >= 100) {
                    GameManager.instance.TileHit(text);
                }
                //You have a valid hit on the enemy
                else
                {
                    GameManager.instance.HitAlert(text);
                }
                break;
                

        }

    }

    public static void SendStringMessage(string input)
    {
        StringMessage myMessage = new StringMessage();
        myMessage.value = input;

        //sending to server
        Debug.Log("SendStringMessage");
        Debug.Log(input);
        NetworkManager.singleton.client.Send(chatMessage, myMessage);

        //previousMessage = input;
    }

}
