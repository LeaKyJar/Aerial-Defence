using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Need references into the Firebase to pull out game dictionary
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

//To use networking to push in the player's history of games played
public class SendPlayerGameHis : MonoBehaviour {

    //Script Reference into MessageHandler
    public static string playerHistory;

    //Have a database reference to dig into the Database and pull out the playername tied to the Device ID
    public static SendPlayerGameHis Instance;

    private DatabaseReference game_his;

    //String variable for the device's Unique String ID
    public string deviceUniqueID = "";



    // Use this for initialization
    void Start () {
        KickStartFb();
        MakeDeviceID();
        PullOutMyGameHis();
        //MessageHandler.player_history = playerHistory;
	}

    void KickStartFb()
    {
        if (Instance == null) Instance = this;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://aerialdeffiredb.firebaseio.com/");

    }

    //Make the unique the ID of the device when it builds onto the phone
    public string MakeDeviceID()
    {
        string deviceUniqueID = SystemInfo.deviceUniqueIdentifier;
        Debug.Log(deviceUniqueID);
        return deviceUniqueID;
    }

    public string PullOutMyGameHis()
    {
        game_his = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/" + deviceUniqueID + "/PlayerStats/TotalGames");
        string playerHistory = game_his.GetValueAsync().ToString(); //Snapshot of all games played?
        return playerHistory;
    }

    // Update is called once per frame
    /*void Update () {
		
	}*/
}
