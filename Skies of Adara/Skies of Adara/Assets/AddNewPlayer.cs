using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

public class AddNewPlayer : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://aerialdeffiredb.firebaseio.com/");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	}

    public class User
    {
        public string username;
        public string email;

        public User()
        {

        }

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }

    //private void 
	
	// Update is called once per frame
	/*void Update () {
		
	}*/
}

