using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject ready;
    //public GameObject atkMenu;
    public GameObject endBtn;
    public GameObject atkBtn;
    public GameObject toggleBtn;
    public GameObject friendlyGrid;
    public GameObject enemyGrid;
    public PhaseIndicator phaseIndicator;
    public GameObject shield;
    public GameObject instructions;
    public GameObject hitMissIndicator;
    public GameObject cloud1;
    public GameObject cloud2;
    private string previousEnemyTileName;
    Character[] charArray;


    private bool startFirst = true;
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

    private bool championPreparationPhase = false;
    public bool ChampionPreparationPhase
    {
        get { return championPreparationPhase; }
    }

    private bool defenderPreparationPhase = false;
    public bool DefenderPreparationPhase
    {
        get { return defenderPreparationPhase; }
    }

    private bool bomberPreparationPhase = false;
    public bool BomberPreparationPhase
    {
        get { return bomberPreparationPhase; }
    }

    private bool engineerPreparationPhase = false;
    public bool EngineerPreparationPhase
    {
        get { return engineerPreparationPhase; }
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
        Bomber.instance.gameObject.SetActive(false);
        Defender.instance.gameObject.SetActive(false);
        Engineer.instance.gameObject.SetActive(false);
        ready.GetComponent<Button>().interactable = false;
        toggleBtn.GetComponent<Toggle>().interactable = false;
        endBtn.GetComponent<Button>().interactable = false;
        Bomber.instance.gameObject.SetActive(false);
        Defender.instance.gameObject.SetActive(false);
        Engineer.instance.gameObject.SetActive(false);
        hitMissIndicator.SetActive(false);
        EnterPreparation();
        charArray = new Character[] { Champion.instance, Bomber.instance, Engineer.instance, Defender.instance };
    }
	
	// Update is called once per frame
	void Update () {
        /*if (!Champion.instance.Active && !Bomber.instance.Active && !Defender.instance.Active && !Engineer.instance.Active && atkPhase)
        {
            EndTurn();
        }*/
        
	}

    // Called when entering defence phase
    public void EndTurn ()
    {
        if (!preparationPhase) {
            MessageHandler.SendStringMessage("YourTurn");
        }
        phaseIndicator.gameObject.SetActive(true);
        phaseIndicator.IndicatePhaseCoroutine("Defense Phase");
        print("def phase");
        atkPhase = false;
        defPhase = true;
        //atkMenu.SetActive(false);
        endBtn.GetComponent<Button>().interactable = false;
        toggleBtn.GetComponent<Toggle>().interactable = false;
        foreach(Character character in charArray)
        {
            character.Active = false;
        }
        SetInstructions("Standby for enemy attack!");
    }

    // Called when entering attack phase
    public void StartTurn ()
    {
        phaseIndicator.gameObject.SetActive(true);
        phaseIndicator.IndicatePhaseCoroutine("Attack Phase");
        atkPhase = true;
        defPhase = false;
        //atkMenu.SetActive(true);
        endBtn.GetComponent<Button>().interactable = true;
        toggleBtn.GetComponent<Toggle>().interactable = true;

        Character[] fullCharArray = { Champion.instance, Defender.instance, Bomber.instance, Engineer.instance,
                GameObject.Find("ChampionBody").GetComponent<Body>(), GameObject.Find("EngineerBody").GetComponent<Body>()};
        foreach (Character character in fullCharArray)
        {
            if (!character.Dead)
            {
                character.Active = true;
            }
        }
        SetInstructions("Click on a unit to see what it can do!");

    }

    // Self-explanatory
    public void EnterPreparation()
    {
        phaseIndicator.IndicatePhaseCoroutine("Preparation Phase");
        preparationPhase = true;
    }

    public void Ready()
    {
        MessageHandler.SendStringMessage("Ready");
        ready.GetComponent<Button>().interactable = false;
    }

    // Self-explanatory
    public void ExitPreparation()
    {
        print("exitPrep");
        if (startFirst)
        {
            StartTurn();
        }
        else
        {
            EndTurn();
        }
        preparationPhase = false;

        //toggleBtn.GetComponent<Toggle>().interactable = true;

        //instructions.SetActive(false);
    }

    // Used for normal toggle and atk selection
    public void GridToggle()
    {
        StartCoroutine(GridToggleNum());
    }

    IEnumerator GridToggleNum()
    {
        //yield return new WaitForSeconds(1);

        if (friendlyGrid.activeSelf)
        {
            atkBtn.SetActive(false);
            friendlyGrid.SetActive(false);
            
            foreach (Character character in charArray)
            {
                character.gameObject.SetActive(false);
            }
            shield.SetActive(false);
            cloud1.GetComponent<Animation>().Play();
            cloud2.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(1);
            enemyGrid.SetActive(true);
        }
        else
        {
            cloud1.GetComponent<Animation>().Play();
            cloud2.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(1);
            Champion.instance.OnDeselectChar();
            Bomber.instance.OnDeselectChar();
            enemyGrid.SetActive(false);
            friendlyGrid.SetActive(true);
            foreach (Character character in charArray)
            {
                character.gameObject.SetActive(true);
            }
            if (!shield.GetComponent<Shield>().Dead)
            {
                shield.SetActive(true);
            }

        }
    }
    

    //Declare attack on enemy
    public void EnemyTargeted(string enemyTileName)
    {
        previousEnemyTileName = enemyTileName;
        //EmptyEnemyTile();
        
        friendlyGrid.SetActive(true);
        hitMissIndicator.SetActive(true);
        hitMissIndicator.GetComponent<HitMissIndicator>().IndicateTargetedCoroutine(GameObject.Find(enemyTileName).transform.position);

        
        //enemyGrid.SetActive(false);

        //Sends attack coord to enemy
        MessageHandler.SendStringMessage(enemyTileName);
    }

    public void ReactivateUnits()
    {
        foreach (Character character in charArray)
        {
            character.gameObject.SetActive(true);

        }

        if (!Defender.instance.Dead)
        {
            shield.SetActive(true);
        }
    }

    //Indicates you scored a valid hit on Enemy grid
    public void HitAlert(string tileName)
    {
        StartCoroutine(HitAlertNum(tileName));
    }

    IEnumerator HitAlertNum(string tileName)
    {
        int enemyTile = Int32.Parse(tileName) + 100;
        print("HitAlert: " + enemyTile);
        enemyGrid.SetActive(true);
        GameObject enemy = GameObject.Find(enemyTile.ToString());
        enemy.GetComponent<EnemyTile>().EnemyPresent();
        hitMissIndicator.SetActive(true);
        hitMissIndicator.GetComponent<HitMissIndicator>().IndicateHitCoroutine(enemy.transform.position);
        yield return new WaitForSeconds(2);
        enemyGrid.SetActive(false);
        ReactivateUnits();
    }

    //Called when receiving enemy's attack coord
    public void TileHit(string tileName)
    {
        StartCoroutine(TileHitNum(tileName));        
    }

    IEnumerator TileHitNum(string tileName)
    {
        int ownTile = Int32.Parse(tileName) - 100;
        GameObject tile = GameObject.Find(ownTile.ToString());

        //Indicate incoming hit
        hitMissIndicator.SetActive(true);
        hitMissIndicator.GetComponent<HitMissIndicator>().IndicateIncomingCoroutine(tile.transform.position);
        yield return new WaitForSeconds(2);
        if (shield.GetComponent<Shield>().LastTileName.Equals(ownTile.ToString()) && !shield.GetComponent<Shield>().Dead)
        {
            shield.GetComponent<Shield>().TakeDamage();
        }
        else
        {
            //MessageHandler.SendStringMessage("Hit Received");
            tile.GetComponent<Tile>().BreakTile();
            //Determines if character was hit
            for (int ii=0; ii<4; ii++)
            {
                print(charArray[ii].name + ": " + charArray[ii].LastTileName);
                if (charArray[ii].LastTileName.Equals(ownTile.ToString()) && !charArray[ii].Dead)
                {
                    charArray[ii].TakeDamage();
                    break;

                }
                else if (charArray[ii].name.Equals("Champion") || charArray[ii].name.Equals("Engineer"))
                {
                    if (charArray[ii].GetComponentInChildren<Body>().LastTileName.Equals(ownTile.ToString()) && !charArray[ii].GetComponentInChildren<Body>().Dead)
                    {
                        charArray[ii].GetComponentInChildren<Body>().TakeDamage();
                        break;
                    }
                }
                else if (ii == 3)
                {
                    MessageHandler.SendStringMessage("Hit Received");
                    break;
                }
            }

        }
        //tile.SetActive(false);

        LoseGame();
    }

        public void EmptyEnemyTile()
    {
        StartCoroutine(EnemyEmptyTileNum());
    }

    IEnumerator EnemyEmptyTileNum()
    {
        enemyGrid.SetActive(true);
        GameObject enemyTile = GameObject.Find(previousEnemyTileName);
        enemyTile.GetComponent<EnemyTile>().DestroyTile();
        hitMissIndicator.SetActive(true);
        hitMissIndicator.GetComponent<HitMissIndicator>().IndicateMissCoroutine(enemyTile.transform.position);
        yield return new WaitForSeconds(2);
        enemyGrid.SetActive(false);
        ReactivateUnits();
    }

    public void LoseGame()
    {
        if(Champion.instance.Dead && Bomber.instance.Dead && Champion.instance.GetComponentInChildren<Body>().Dead)
        {
            phaseIndicator.gameObject.SetActive(true);
            phaseIndicator.IndicatePhaseCoroutine("Defeat");
            MessageHandler.SendStringMessage("Victory");
            SceneManager.LoadScene("Scenes/Menu");
            CustomNetworkManager cnm = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
            //cnm.StopMatchMaking();
        }
    }

    public void WinGame()
    {
        phaseIndicator.gameObject.SetActive(true);
        phaseIndicator.IndicatePhaseCoroutine("Victory");
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void SetInstructions(String stringInput)
    {
        instructions.GetComponentInChildren<Text>().text = stringInput;
    }

    public void ChampionToDefender()
    {
        championPreparationPhase = false;
        defenderPreparationPhase = true;
        SetInstructions("The Defender cannot attack. Instead, she can deploy a shield anywhere on the grid." +
            " When the shield is hit," +
            " the Defender takes the hit instead, leaving the protected ship or tile untouched.");
        Defender.instance.gameObject.SetActive(true);
    }

    public void DefenderToEngineer()
    {
        defenderPreparationPhase = false;
        engineerPreparationPhase = true;
        SetInstructions(" Like the Defender, the Engineer can't attack." +
            " He can, however, repair a damaged ship once per game." +
            " The enemy will not know when repairs are carried out." +
            " The Engineer also occupies two tiles."
            );
        Engineer.instance.gameObject.SetActive(true);
    }

    public void EngineerToBomber()
    {
        engineerPreparationPhase = false;
        bomberPreparationPhase = true;
        SetInstructions("Like the Champion, the Bomber can attack." +
            " She does a little more than that though." +
            " When she takes damage, she will attack the enemy tile corresponding to her position." +
            " A true kamikaze warrior!");
        Bomber.instance.gameObject.SetActive(true);
    }

    public void BombertoReady()
    {
        bomberPreparationPhase = false;
        SetInstructions("Click the Ready Button when you're ready!");
        if (GameManager.instance.PreparationPhase && Champion.instance.BodyDeployed && Engineer.instance.BodyDeployed && Defender.instance.BodyDeployed && Bomber.instance.BodyDeployed)
        {
            ready.GetComponent<Button>().interactable = true;
        }
    }

}
