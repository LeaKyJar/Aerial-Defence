using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject ready;
    public GameObject atkMenu;
    public GameObject atkBtn;
    public GameObject friendlyGrid;
    public GameObject enemyGrid;

    private bool startFirst = false;
    public bool StartFirst
    {
        get { return startFirst; }
        set { startFirst = value; }
    }

    private bool preparationPhase = false;
    public bool PreparationPhase
    {
        get { return preparationPhase; }
    }

    private bool atkPhase = false;
    public bool AtkPhase
    {
        get { return atkPhase; }
    }

    private bool defPhase = false;
    public bool DefPhase
    {
        get { return DefPhase; }
    }


    // Sets up singleton instance
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        EnterPreparation();

    }
	
	// Update is called once per frame
	void Update () {
	}

    // Called when entering defence phase
    public void EndTurn ()
    {
        atkPhase = false;
        defPhase = true;
        atkMenu.SetActive(false);
    }

    // Called when entering attack phase
    public void StartTurn ()
    {
        atkPhase = true;
        defPhase = false;
        Champion.instance.Active = true;
        Bomber.instance.Active = true;
    }

    // Self-explanatory
    public void EnterPreparation()
    {
        preparationPhase = true;
    }


    public void ExitPreparation()
    {
        print("exitPrep");
        preparationPhase = false;
        ready.SetActive(false);
        atkMenu.SetActive(true);
        if (startFirst)
        {
            StartTurn();
        }
        else
        {
            EndTurn();
        }
    }

    public void GridToggle()
    {
        if (friendlyGrid.activeSelf)
        {
            atkBtn.SetActive(false);
            friendlyGrid.SetActive(false);
            enemyGrid.SetActive(true);
        }
        else
        {
            friendlyGrid.SetActive(true);
            enemyGrid.SetActive(false);
        }
    }

    public void EnemyTargeted(string enemyTileName)
    {
        friendlyGrid.SetActive(true);
        enemyGrid.SetActive(false);
        MessageHandler.SendStringMessage(enemyTileName);
    }

    public void TileHit(string tileName)
    {
        int ownTile = Int32.Parse(tileName)-100;
        GameObject tile = GameObject.Find(ownTile.ToString());
        tile.SetActive(false);
    }
}
