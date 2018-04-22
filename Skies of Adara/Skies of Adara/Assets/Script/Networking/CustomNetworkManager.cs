using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager{

    private float RefreshTime;
    public volatile bool inmatchmaking = false;
    public volatile bool waitingrequests = false;
    private const short chatMessage = 131;

    //iniT
    public static CustomNetworkManager instance = null;
    public float expiry;

    [SerializeField]private GameObject matchfound; //set matchfound in hierarchy
    [SerializeField]private GameObject matchmaking; //set matchmaking
    [SerializeField]private Button acceptMatch;
    [SerializeField]private Button cancelMatch;
    [SerializeField]private GameObject mainMenu; //set mainmenu
    public GameObject dcbutton;

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

    IEnumerator Refresher()
    {
        StopMatchMaking();
        yield return new WaitForSeconds(2);
        StartHosting();
        RefreshTime = Time.time + 10f;
    }

    private void Update()
    {
        if (inmatchmaking)
        { 
            if (Time.time >= RefreshTime)
            {
                StartCoroutine(Refresher());
            }
        }
        if (waitingrequests)
        {
            if (Time.time >= expiry)
            {

                cancelrequest();
            }
        }
    }

    public void StartHosting()
    {
        StartMatchMaker();
        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListMatchesComplete);
        inmatchmaking = true;
        RefreshTime = Time.time + 10f;
    }

	private void OnMatchCreated(bool success, string extendedinfo, MatchInfo responsedata)
	{
        StartHost(responsedata);
        if (!MessageHandler.listener)
        { 
            MessageHandler.Register();
            MessageHandler.listener = true;
        }
    }   

    private void HandleListMatchesComplete(bool success, 
		string extendedinfo, 
		List<MatchInfoSnapshot> responsedata)
    {
        if (success&&responsedata.Count>0)
        {
            matchMaker.JoinMatch(responsedata[0].networkId, "", "", "", 0, 0, HandleJoinedMatch);
        }
        else
        {
            matchMaker.CreateMatch("Jasons Match", 4, true, "", "", "", 0, 0, OnMatchCreated);
        }
        
        //test.Register(client);
    }

	private void HandleJoinedMatch(bool success, string extendedinfo, MatchInfo responsedata)
	{
        StartClient(responsedata);
        if (!MessageHandler.listener)
        {
            MessageHandler.Register();
            MessageHandler.listener = true;
        }
    }

    

    public void StopMatchMaking()
    {
        inmatchmaking = false;
        StopHost();
        MessageHandler.listener = false;
    }

    public override void OnServerConnect(NetworkConnection Conn)
    {
        if (Conn.hostId >= 0)
        {
            Debug.Log("OnServerConnect");
            MessageHandler.SendStringMessage("Connected");
        }
    }

    public void changeScene()
    {
       // matchfound = this.gameObject.GetComponent<GameObject>();
        //matchmaking = this.GetComponent<GameObject>();
        //acceptMatch = this.GetComponent<Button>();
        //cancelMatch = this.GetComponent<Button>();

        matchfound.SetActive(true);
        matchmaking.SetActive(false);

        if (matchfound.activeSelf == true)
        {
            acceptMatch.gameObject.SetActive(true);
            cancelMatch.gameObject.SetActive(true);
        }
    }

    public void acceptmatch()
    {
        disablebuttons();
        MessageHandler.SendStringMessage("Accept");
    }

    public void startGame()
    {
        SceneManager.LoadScene("Scenes/BattleScene");
    }

    public void cancelGame()
    {
        MessageHandler.SendStringMessage("Deny");
    }

    public void cancelrequest()
    {
        matchfound.SetActive(false);
        mainMenu.SetActive(true);
        waitingrequests = false;
        MessageHandler.accept = false;
        StopMatchMaking();
    }

    public void disablebuttons() {
        acceptMatch.gameObject.SetActive(false);
        cancelMatch.gameObject.SetActive(false);
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        //StringMessage myMessage = new StringMessage();
        //myMessage.value = "Disconnected";
        //NetworkServer.SendToAll(chatMessage, myMessage);
        if (MessageHandler.halfway)
        {
            StopHost();
            waitingrequests = false;
            SceneManager.LoadScene("Scenes/Menu");
            dcbutton.SetActive(true);

        }

    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        if (MessageHandler.halfway)
        {
            waitingrequests = false;
            SceneManager.LoadScene("Scenes/Menu");
            dcbutton.SetActive(true);
        }
    }
}