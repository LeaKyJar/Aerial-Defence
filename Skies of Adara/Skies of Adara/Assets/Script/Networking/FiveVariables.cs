using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class FiveVariables : MonoBehaviour {

    //19th April (5 variables) 
    //To input up History, EnemyName, MyGrid, EnemyGrid, Status

    public static FiveVariables Instance;

    //Reference to input in the needed items into the database
    private DatabaseReference pushoffinfo;

    //References to pull out game info
    private DatabaseReference game_his;

    //Based on the device's Unique ID used
    public string deviceUniqueID = "";

    // Use this for initialization
    void Start () {

        KickStartFb();
        MakeDeviceID();
        PullOutMyGameHis();
		
	}

    void KickStartFb()
    {
        if (Instance == null) Instance = this;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://aerialdeffiredb.firebaseio.com/");
    }

    public string MakeDeviceID()
    {
        string deviceUniqueID = SystemInfo.deviceUniqueIdentifier;
        Debug.Log(deviceUniqueID);
        return deviceUniqueID;
    }

    public string PullOutMyGameHis()
    {
        game_his = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/" + deviceUniqueID + "/PlayerStats/TotalGames");
        string playerHistory = game_his.GetValueAsync().ToString();
        return playerHistory;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
