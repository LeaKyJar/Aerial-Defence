using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

//Script to Push off My name for my Opponent's Side
//Use this guy to publish onto the menu screen also
public class MyNameForEnemy : MonoBehaviour {

    //Script Reference into MessageHandler
    public static String nametoenemy;
    
    //Have a database reference to dig into the Database and pull out the playername tied to the Device ID
    public static MyNameForEnemy Instance;

    //String variable to send my own name to the opponent.
    public string for_opp = "";

    //String variable for the device's Unique String ID
    public string deviceUniqueID = "";

    private DatabaseReference pulloutname;
    private DatabaseReference datab = FirebaseDatabase.DefaultInstance.RootReference; 
    // Use this for initialization
    void Start () {
       
        KickStartFb();
        MakeDeviceID();
        PullOutMyNameForOpp();
        //Message handler reference here for sending name to enemy
        //MessageHandler.abcdz = nametoenemy;
    }

    void KickStartFb()
    {
        if (Instance == null) Instance = this;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://aerialdeffiredb.firebaseio.com/");
    }

    public string PullOutMyNameForOpp(/*Message Args*/)
    {
        pulloutname = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/" + deviceUniqueID + "/PlayerStats/PlayerName");
        string nametoenemy = pulloutname.GetValueAsync().ToString();
        return nametoenemy;
    }

    //Make the unique the ID of the device when it builds onto the phone
    public string MakeDeviceID()
    {
        string deviceUniqueID = SystemInfo.deviceUniqueIdentifier;
        Debug.Log(deviceUniqueID);
        return deviceUniqueID;
    }

    //Replace on that bubble the PlayerName GUI Textbox with the actual device ID the game is running in
    /*void ReplaceWDeviceID()
    {
        //The String reference matters if your data access ends up being in a bunch of hierachal parents. 
        datab = FirebaseDatabase.DefaultInstance.GetReference("DevicesID");
        Debug.Log("To be or not to be:" + datab);
        //Debug.Log(GameObject.Find("PlayerName"));
        //Debug.Log(GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>());
        GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = for_opp; //for_opp is a string variable 
    }*/

    // Update is called once per frame

}
