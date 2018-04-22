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
    public static bool accept = false;
    public static bool halfway = true;
    public static List<List<String>> gamelog;
    public static List<String> turn;
    public static bool listener = false;
    /// ///////////////////////////
    public static bool victory = true;
    public static string tobeupdated;
    public static string enemygrid;
    public static string owngrid;
    public static string enemy;
    //public static String previousMessage;

    //Calling out the name variable to send to the enemy (MyNameForEnemy)
    public static string playername;

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
                accept = false;
                ready = false;
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
                Debug.Log(accept);
                if (!accept)
                {
                    accept = true;
                }
                else
                {
                    myMessage = new StringMessage();
                    myMessage.value = "Start";
                    NetworkServer.SendToAll(chatMessage, myMessage);
                    accept = false;
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
            case ("Disconnected"):
                if (halfway)
                {
                    SceneManager.LoadScene("Scenes/Menu");
                }
                break;
            case ("Connected"):
                //SceneManager.LoadScene("Scenes/BattleScene");
                CustomNetworkManager.instance.inmatchmaking = false;
                CustomNetworkManager.instance.changeScene();
                CustomNetworkManager.instance.expiry = Time.time + 10f;
                CustomNetworkManager.instance.waitingrequests = true;
                gamelog = new List<List<string>>();
                break;

            case ("Start"):
                CustomNetworkManager.instance.waitingrequests = false;
                CustomNetworkManager.instance.startGame();
                break;

            case ("Deny"):
                CustomNetworkManager.instance.disablebuttons();
                GameObject.Find("CountdownTimer").GetComponent<CountdownTimer>().reject();
                break;
            case ("First"):
                GameManager.instance.StartFirst = true;
                GameManager.instance.ExitPreparation();
                turn = new List<string>();
                turn.Add("First");
                break;
            case ("Second"):
                GameManager.instance.StartFirst = false;
                GameManager.instance.ExitPreparation();
                turn = new List<string>();
                turn.Add("Second");
                break;
            case ("YourTurn"):
                GameManager.instance.StartTurn();
                if (GameManager.instance.StartFirst) {
                    gamelog.Add(turn);
                    turn = new List<string>();
                }
                break;
            case ("Victory"):
                break;
            case("Hit Received"):
                turn.Add("R:Hit Received");
                Debug.Log("Previous Message");
                GameManager.instance.EmptyEnemyTile();
                break;
            default:
                //Your tile that enemy has hit
                Debug.Log(text);

                int myInt = int.TryParse(text, out myInt) ? myInt : 99999;

                if (myInt == 99999) {
                    if (victory)
                    {
                        halfway = false;
                        Debug.Log(gamelog);
                        turn.Add("E:WIN");
                        gamelog.Add(turn);
                        gamelogtostring();
                        gamelog = new List<List<string>>();
                        owngrid = GameManager.instance.Evaluate();
                        SendStringMessage(GameManager.instance.Evaluate() +";name:" + playername);
                        enemy = text.Substring(text.IndexOf("name:")+5);
                        enemygrid = text.Substring(0, text.IndexOf("name:") - 1);
                        DBRetriever.instance.updategame(victory,enemygrid,owngrid,enemy,tobeupdated);
                        DBRetriever.instance.updatewinloss(victory);
                        GameManager.instance.WinScreen(enemygrid);
                    }
                    else
                    {
                        owngrid = GameManager.instance.Evaluate();
                        enemy = text.Substring(text.IndexOf("name:") + 5);
                        enemygrid = text.Substring(0, text.IndexOf("name:") - 1);
                        DBRetriever.instance.updategame(victory, enemygrid, owngrid, enemy,tobeupdated);
                        DBRetriever.instance.updatewinloss(victory);
                        GameManager.instance.LoseScreen(enemygrid);
                    }
                }
                else if (Int32.Parse(text) >= 100)
                {
                    turn.Add("R:" + text);
                    GameManager.instance.TileHit(text);
                }
                //You have a valid hit on the enemy
                else
                {
                    turn.Add("R:" + text);
                    GameManager.instance.HitAlert(text);
                }
                break;
                

        }

    }

    public static void SendStringMessage(string input)
    {
        StringMessage myMessage = new StringMessage();
        myMessage.value = input;
        if (input != "Connected"&& input != "Yourturn" )
        {
            if (turn != null)
            {
                turn.Add("S:" + input);
            }
        }
        else if(input == "Yourturn" && (!GameManager.instance.StartFirst))
        {
            gamelog.Add(turn);
            turn = new List<string>();
        }
        //sending to server
        Debug.Log("SendStringMessage");
        Debug.Log(input);
        try
        {
            NetworkManager.singleton.client.Send(chatMessage, myMessage);
        }
        catch (System.Exception) { };

        //previousMessage = input;
    }

    internal static void helper()
    {
        MessageHandler.victory = false;
        MessageHandler.halfway = false;
        MessageHandler.turn.Add("E:LOSE");
        MessageHandler.gamelog.Add(MessageHandler.turn);
        gamelogtostring();
        MessageHandler.gamelog = new List<List<string>>();
        MessageHandler.owngrid = GameManager.instance.Evaluate();
        MessageHandler.SendStringMessage(GameManager.instance.Evaluate() + ";name:" + playername);
    }

    public static void gamelogtostring() {
        List<List<string>> variable = gamelog;
        string final = "";
        foreach (List<string> a in gamelog) {
            foreach (string b in a) {
                final += b + ",";
            }
            final = final.Substring(0,final.Length-1)+";";
        }
        if (final != "") {
            final = final.Substring(0, final.Length - 1);
        }
        tobeupdated = final;
    }



    ////////work in progress
    public static void readinghistory(string gameinfo) {
        bool first;
        List<string> history = new List<string>();
        string[] dissect1 = gameinfo.Split(';');
        int totalturns = dissect1.Length;
        for (int i = 0; i < totalturns; i++) {
            bool exit = true;
            string[] turn = dissect1[i].Split(',');
            if (i == 0) {
                if (turn[0] == "First") {
                    first = true;
                }
                while (exit) {


                }
            }


        }
    }
}
