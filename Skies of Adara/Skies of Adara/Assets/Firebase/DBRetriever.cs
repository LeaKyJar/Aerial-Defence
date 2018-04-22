using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//This is the class that will help me retrieve Data out of the Database.
//Used in order to Push to the player when he wants to see his Statistics

public class DBRetriever : MonoBehaviour
{
    //These are initial variables that have to be set in order to take in values 
    public GameObject miniGrid;
    //an instance of the class has been set. 
    public static DBRetriever instance;
    public GameObject[] list;
    public GameObject NameInputObject;
    //String Variable to make out a device's actual ID
    string deviceUniqueID;

    //String variable to send my own MessageHandler.playername to the opponent.
    private int game_count;

    //Private Database references for each type of data you want to get out from the db
    private DatabaseReference datab;
    private DatabaseReference root;
    private DatabaseReference Alldevices;
    private DatabaseReference referPlayerGames;
    private DatabaseReference selfdevice;

    // Set a count variable for the number of games lost and won
    private int count = 0;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        //All the methods you wanna run for one game's Database interaction (Updating and retrieving)
        deviceUniqueID = SystemInfo.deviceUniqueIdentifier; //Unity's method for getting a device's ID - 32Hex ID
        KickStartFb();
        Debug.Log("Check");
        CheckerOnDB();
    }

    // This is method helps in establishing connection with the Firebase Database. 
    void KickStartFb()
    {
        //Debug.Log(Instance); // helps me check if the instance has been instantiated properly
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://aerialdeffiredb.firebaseio.com/");

        //Making a root reference on the database
        root = FirebaseDatabase.DefaultInstance.RootReference;

        Alldevices = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs");
    }

    //Json Retrieval of all the devices inside the DB already
    public void GetJsonofDeviceID() // Pull out all the devices that have been added into the database already
    {
        
        Alldevices.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) { Debug.Log("Database Retrieval not successful"); }
            else if (task.IsCompleted)
            {
                DataSnapshot devicesindb = task.Result;
                string all_devs_in_db = devicesindb.GetRawJsonValue().ToString();
                Debug.Log("Alldevices+   " + all_devs_in_db + "End");
            }
        }
        );

        //Zooming into a particular device's info on the database. 
        DatabaseReference newdevice = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/" + deviceUniqueID+"/GameHistory");
  
        newdevice.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) { Debug.Log("Database Retrieval not successful"); }
            else if (task.IsCompleted)
            {
                string tobeprinted2 = "No Previous Games";
                DataSnapshot list = task.Result;
                List<object> gamelist = list.Value as List<object>;
                if (gamelist != null)
                {

                    // Printing out the information of the game and publishing it.
                    Dictionary<string, object> gameinfo = gamelist[gamelist.Count - 1] as Dictionary<string, object>;
                    bool victory = (bool)gameinfo["Victory"];
                    string history = (string)gameinfo["History"]; //Every turn is separated by a ; character. Every message is separated by a , character.
                    string enemygrid = (string)gameinfo["EnemyGrid"];
                    //grid is in this order {Champion,ChampionBody,Defender,Engineer,EngineerBody,Bomber}
                    string owngrid = (string)gameinfo["OwnGrid"];
                    string enemyname = (string)gameinfo["Enemy"];

                    miniGrid.GetComponent<MiniGrid>().MakeHitMissSummary(enemygrid);

                    tobeprinted2 = "VS " + enemyname + "\n" + "Turns: " + history.Split(';').Length;
                }

                GameObject.Find("PlayerStat").GetComponent<TextMeshProUGUI>().text = tobeprinted2;
            }
        }
        );
    }

    //Method to get through the data snapshot from the reference
    public void CheckerOnDB()
    {

        Alldevices.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Trouble Connecting up to firebase. Please try again");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot devicesnap = task.Result;
                // Getting a snapshot of all the devices connected into the database. Handlers for if device is present or not respectively.
                if (!devicesnap.HasChild(deviceUniqueID))
                {
                    inputname();
                }

                //The alternative branch if the Unique ID is part of the Database
                //This shows that this device was used to play the game
                else
                {
                    
                    DataSnapshot devicesindb = task.Result;
                    MessageHandler.playername = (((devicesindb.Value as Dictionary<string, object>)[deviceUniqueID] as Dictionary<String, object>)["PlayerStats"] as Dictionary<string, object>)["PlayerName"] as string;
                    Debug.Log(MessageHandler.playername);
                    GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = MessageHandler.playername;
                    activateonceloaded();
                }
            }
        });
    }

    //Function to help initialise up a full new device into the database. 
    private TransactionResult UpdateNewDevice(MutableData md)
    {
        //check on the value of the mutable data
        if (md.Value == null)
        {
            Debug.Log("This Mutable Data has no specific value. Is your reference correct?");
        }

        //If in the event the mutable data is not an empty fella, do the following to push it up
        else if (md.Value != null)
        {
            Debug.Log("MD IS " + md.Value);
            Dictionary<string, object> device = md.Value as Dictionary<string, object>;
            //Initiate a Dictionary Place holder for the new device to be added
            Dictionary<string, object> newDeviceAddition = new Dictionary<string, object>();
            //device = new Dictionary<string, object>();
            Debug.Log(newDeviceAddition);

            //Initiate an extra list that should act as the Player info to be kept under the device unique id
            Dictionary<string, object> PlayerStats = new Dictionary<string, object>();

            PlayerStats["PlayerName"] = MessageHandler.playername;

            PlayerStats["GamesWon"] = "0";
            PlayerStats["GamesLost"] = "0";

            //Initialize a game_count and overwrite it at the top from 0
            PlayerStats["WinRate"] = "N%";
            newDeviceAddition["PlayerStats"] = PlayerStats;

            Debug.Log("ID addition into database is successful");
            device.Add(deviceUniqueID, newDeviceAddition);
            md.Value = device;
        }
        return TransactionResult.Success(md);
    }

    //Method to pull out content from the Database
    private void DBPuller()
    {
        DatabaseReference db = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/" + deviceUniqueID + "/PlayerStats/PlayerName");
    }
    //Input name 
    void inputname()
    {
        foreach (GameObject a in list)
        {
            a.SetActive(false);
        }
        NameInputObject.SetActive(true);
    }

    public void activatemenu()
    {
        MessageHandler.playername = GameObject.Find("NameInput").GetComponent<InputField>().text;
        GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = MessageHandler.playername;
        foreach (GameObject a in list)
        {
            a.SetActive(true);
        }
        Alldevices.RunTransaction(UpdateNewDevice);
        NameInputObject.SetActive(false);
    }

    void activateonceloaded()
    {
        foreach (GameObject a in list)
        {
            a.SetActive(true);
        }

    }

    //Method to add in the game history of a device that has already established a place inside the database. 
    public void updategame(bool victory, string enemygrid,string owngrid, string enemyname, string gamelog)
    {

        selfdevice = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/" + deviceUniqueID);
        selfdevice.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Trouble Connecting up to firebase. Please try again");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot devicesnap = task.Result;
                if (!devicesnap.HasChild("GameHistory"))
                {
                    Debug.Log("i'm in");
                    selfdevice.RunTransaction(data =>
                    {
                        if (data.Value == null) { Debug.Log("Data is wrong"); }

                        else if (data.Value != null)
                        {

                            //
                            Dictionary<string, object> root = data.Value as Dictionary<string, object>;
                            List<object> gamelist = new List<object>();
                            Dictionary<string, object> game = new Dictionary<string, object>();
                            game["Victory"] = victory;
                            game["History"] = gamelog;
                            game["EnemyGrid"] = enemygrid;
                            game["OwnGrid"] = owngrid;
                            game["Enemy"] = enemyname;
                            gamelist.Add(game);
                            root.Add("GameHistory", gamelist);
                            data.Value = root;

                        }
                        return TransactionResult.Success(data);
                    });
                }
                else
                {
                    selfdevice.RunTransaction(data =>
                    {
                        if (data.Value == null) { Debug.Log("Data is wrong"); }

                        else if (data.Value != null)
                        {

                            //Dictionary of the GameHistory
                            Dictionary<string, object> root = data.Value as Dictionary<string, object>;
                            List<object> games = root["GameHistory"] as List<object>;
                            Dictionary<String, object> game = new Dictionary<string, object>();
                            game["Victory"] = victory;
                            game["History"] = gamelog;
                            game["EnemyGrid"] = enemygrid;
                            game["OwnGrid"] = owngrid;
                            game["Enemy"] = enemyname;
                            games.Add(game);
                            root["GameHistory"] = games;
                            data.Value = root;

                        }
                        return TransactionResult.Success(data);
                    });
                }
            }
        });

    }
    //Modifying the number of games the player has lost or won
    public void updatewinloss(bool victory) {
        DatabaseReference winloss = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/"+deviceUniqueID+"/PlayerStats");
        winloss.RunTransaction(data =>
        {
            if (data.Value == null) { Debug.Log("Data is wrong"); }

            else if (data.Value != null)
            {
                Debug.Log("Data is right");
                Dictionary<string, object> root = data.Value as Dictionary<string, object>;

                if (victory)
                {
                    Debug.Log(true);
                    string count = ((string)root["GamesWon"]);
                    root["GamesWon"] = ""+(Int32.Parse(count) + 1);
                    Debug.Log(count);
                }
                else
                {
                    Debug.Log(false);
                    string count = ((string)root["GamesLost"]);
                    root["GamesLost"] = "" + (Int32.Parse(count) + 1);
                    Debug.Log(count);
                }
                data.Value = root;
                Debug.Log("finish");
            }
            return TransactionResult.Success(data);
        });
    }

}
