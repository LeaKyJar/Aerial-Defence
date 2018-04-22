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

    public GameObject timer;
    private float timeLeft;
    public GameObject MiniGrid;
    public GameObject missilePrefab;

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
    public GameObject EndGamePopUp;
    public GameObject Blackout;
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
            Destroy(instance);
            instance = this;
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
        if (!PreparationPhase)
        {
            timeLeft -= Time.deltaTime;
            if (Math.Round(timeLeft) > 9)
            {
                timer.GetComponentInChildren<Text>().text = "00:" + Math.Round(timeLeft).ToString();
            }
            else if (Math.Round(timeLeft) <= 9)
            {
                timer.GetComponentInChildren<Text>().text = "00:0" + Math.Round(timeLeft).ToString();
            }
            if (timeLeft < 0 && atkPhase)
            {
                EndTurn();
            }
        }
        
	}

    // Called when entering defence phase
    public void EndTurn ()
    {
        instructions.SetActive(true);
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
            character.MakeInactive();
        }
        SetInstructions("Standby for enemy attack!");
        timeLeft = 59f;
    }

    // Called when entering attack phase
    public void StartTurn ()
    {
        instructions.SetActive(true);
        phaseIndicator.gameObject.SetActive(true);
        phaseIndicator.IndicatePhaseCoroutine("Attack Phase");
        atkPhase = true;
        defPhase = false;
        //atkMenu.SetActive(true);
        endBtn.GetComponent<Button>().interactable = true;
        toggleBtn.GetComponent<Toggle>().interactable = true;

        Character[] fullCharArray = { Champion.instance, Bomber.instance, Engineer.instance, Defender.instance,
                GameObject.Find("ChampionBody").GetComponent<Body>(), GameObject.Find("EngineerBody").GetComponent<Body>()};

        int numberOfUndamaged = 0;

        foreach (Character character in fullCharArray)
        {
            if (!character.Dead)
            {
                if (character != Engineer.instance &&
                    character != GameObject.Find("EngineerBody").GetComponent<Body>() &&
                    character != Defender.instance)
                {
                    character.MakeActive();
                }
                numberOfUndamaged += 1;
                Debug.Log("no.Of Undamaged = " + numberOfUndamaged);
            }
        }
        if (numberOfUndamaged<6)
        {
            if (!Engineer.instance.Dead && Engineer.instance.Heal>0)
            {
                Engineer.instance.MakeActive();
            }
            if (!GameObject.Find("EngineerBody").GetComponent<Body>().Dead && Engineer.instance.Heal > 0)
            {
                GameObject.Find("EngineerBody").GetComponent<Body>().MakeActive();
            }
        }
        SetInstructions("Click on a unit to see what it can do!");
        timeLeft = 59f;

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

        timer.SetActive(true);

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
            EnableActiveIndicators();
        }
    }

    public void EnableActiveIndicators()
    {
        try
        {
            Champion.instance.ActiveIndicator.GetComponent<CharacterActiveIndicator>().EnableIndicators();
        }catch (Exception ex)
        {
            Debug.Log(ex);
        }
        Character[] tempCharArray = { Champion.instance, Bomber.instance, Champion.instance.GetComponentInChildren<Body>(), Engineer.instance, Engineer.instance.GetComponentInChildren<Body>() };

        foreach (Character character in tempCharArray)
        {
            if (character.Active && character.ActiveIndicator != null)
            {
                character.ActiveIndicator.GetComponent<RawImage>().enabled = true;
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

        yield return new WaitForSeconds(1);

        EnableActiveIndicators();

        if (!Champion.instance.Active && !Bomber.instance.Active && !Engineer.instance.Active
            && !Champion.instance.GetComponentInChildren<Body>().Active && !Engineer.instance.GetComponentInChildren<Body>().Active && AtkPhase)
        {
            EndTurn();
        }
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
                if (charArray[ii].LastTileName.Equals(ownTile.ToString()))
                {
                    charArray[ii].TakeDamage();
                    break;

                }
                else if (charArray[ii].name.Equals("Champion") || charArray[ii].name.Equals("Engineer"))
                {
                    if (charArray[ii].GetComponentInChildren<Body>().LastTileName.Equals(ownTile.ToString()))
                    {
                        charArray[ii].GetComponentInChildren<Body>().TakeDamage();
                        break;
                    }
                }
                else if (ii == 3)
                {
                    MessageHandler.SendStringMessage("Hit Received");
                    instructions.SetActive(false);
                    FireMissile(new Vector3(1500, 1535, 0));
                    yield return new WaitForSeconds(2);
                    instructions.SetActive(true);
                    break;
                }
            }

        }
        //tile.SetActive(false);

        LoseGame();
    }

    public void FireMissile(Vector3 pos)
    {
        GameObject missile = Instantiate(missilePrefab, new Vector3(-400, 1700, 0), transform.rotation);
        missile.transform.SetParent(GameObject.Find("CombatBG").transform, false);
        missile.GetComponent<Missile>().DesignateOrigin(new Vector3 (-400, 1700, 0));
        missile.GetComponent<Missile>().DesignateTarget(pos);
    }

    public void EmptyEnemyTile()
    {
        StartCoroutine(EnemyEmptyTileNum());
    }

    IEnumerator EnemyEmptyTileNum()
    {
        enemyGrid.SetActive(true);
        Debug.Log(GameManager.instance.previousEnemyTileName);
        GameObject enemyTile = GameObject.Find(previousEnemyTileName);
        Debug.Log(enemyTile.GetComponent<EnemyTile>());
        enemyTile.GetComponent<EnemyTile>().DestroyTile();
        hitMissIndicator.SetActive(true);
        hitMissIndicator.GetComponent<HitMissIndicator>().IndicateMissCoroutine(enemyTile.transform.position);
        yield return new WaitForSeconds(2);
        enemyGrid.SetActive(false);
        ReactivateUnits();
        yield return new WaitForSeconds(1);

        EnableActiveIndicators();

        if (!Champion.instance.Active && !Bomber.instance.Active && !Engineer.instance.Active 
            && !Champion.instance.GetComponentInChildren<Body>().Active && !Engineer.instance.GetComponentInChildren<Body>().Active && AtkPhase)
        {
            EndTurn();
        }
    }

    public void LoseGame()
    {

        if (Champion.instance.Dead && Bomber.instance.Dead && Champion.instance.GetComponentInChildren<Body>().Dead)
        {
            

            phaseIndicator.gameObject.SetActive(true);          
            phaseIndicator.IndicatePhaseCoroutine("Defeat");
            MessageHandler.helper();
        }
    }

    public void LoseScreen(String args)
    {
        Blackout.SetActive(true);
        EndGamePopUp.gameObject.SetActive(true);
        EndGamePopUp.GetComponent<EndGamePopUp>().DefeatPopUp();
        //GameObject.Find("EndGameText").GetComponent<Text>().text = args;
        MiniGrid.GetComponent<MiniGrid>().MakeHitMissSummary(args);
    }



    public void WinScreen(String args)
    {
        Blackout.SetActive(true);
        EndGamePopUp.gameObject.SetActive(true);
        EndGamePopUp.GetComponent<EndGamePopUp>().VictoryPopUp();
        MiniGrid.GetComponent<MiniGrid>().MakeHitMissSummary(args);
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
            "  When she takes damage, she will attack the enemy tile corresponding to her position." +
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
    public String Evaluate()
    {
        String finalstr = "";
        Character[] fullCharArray = { Champion.instance, Champion.instance.GetComponentInChildren<Body>() , Defender.instance , Engineer.instance,
        Engineer.instance.GetComponentInChildren<Body>() , Bomber.instance};
        foreach (Character a in fullCharArray)
        {
            finalstr += "" + a.Dead + ";";
            finalstr += "" + a.LastTileID + ";";
        }
        return finalstr.Substring(0,finalstr.Length-1);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Scenes/Menu");
        CustomNetworkManager.instance.StopMatchMaking();
    }

}
