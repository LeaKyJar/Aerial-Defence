using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject ready;
    public GameObject atkMenu;
    public GameObject atkBtn;
    public GameObject friendlyGrid;
    public GameObject enemyGrid;
    private bool championActive=false;
    public bool ChampionActive
    {
        get { return championActive; }
        set { championActive = value; }
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
    //public ArrayList gridInfo;



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

    public void EndTurn ()
    {
        atkPhase = false;
        defPhase = true;
        atkMenu.SetActive(false);
    }

    public void StartTurn ()
    {
        atkPhase = true;
        defPhase = false;
        championActive = true;
    }

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
        StartTurn();
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

    public void EnemyTargeted()
    {
        championActive = false;
        friendlyGrid.SetActive(true);
        enemyGrid.SetActive(false);
    }
}
