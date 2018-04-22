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
//To try and pull out database stats of a player and place onto a textbox 

public class PlaceHolderforPlayerStats : MonoBehaviour {

    //Game Object or canvas the DBRetriever script is tied to
    public Canvas EXP;

    //Make the Class that has the variable we want private
    private DBRetriever otherScriptforPlayerName;

    //Set up a private variable of the same type as the variable you're after
    private string for_opp;
    private string deviceUniqueID; 

    public static PlaceHolderforPlayerStats Instance;
    
    private string results;
    
    //private instance to pull out a player's data
    private DatabaseReference mDatabaseReference;

    // Use this for initialization
    void Start() {
        //Debug.Log("gameobjectword hasbeenlaunched");
        KickStartFb();
        RetrieveStats();
        if (MessageHandler.gamelog != null)
        {
            String blank = "";
            foreach (List<String> a in MessageHandler.gamelog) {
                blank += String.Join(", ", a.ToArray());
                blank += ";";
            }

            GameObject.Find("PlayerStat").GetComponent<TextMeshProUGUI>().text = blank;
            MessageHandler.gamelog = null;
        }
        
    }

    void KickStartFb()
    {
        if (Instance == null) Instance = this;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://aerialdeffiredb.firebaseio.com/");

        mDatabaseReference = FirebaseDatabase.DefaultInstance.GetReference("DeviceIDs/"+deviceUniqueID + "/PlayerStats");
   
    }
    public void RetrieveStats()
    {
        mDatabaseReference.OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //some form of error handling
            }
            else if (task.IsCompleted)
            {
                //It is snapshot that is useful since you are printing more than one key-value pair of the tree's data.
                DataSnapshot playerstats = task.Result;
                results = playerstats.GetRawJsonValue();
                GameObject.Find("PlayerStat").GetComponent<TextMeshProUGUI>().text = results;
            }
        });
    }
}