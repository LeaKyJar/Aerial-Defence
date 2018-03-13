using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    private const short chatMessage = 131;

    public void StartHosting()
    {
        StartMatchMaker();
        matchMaker.ListMatches(0, 10, "", true, 0, 0, HandleListMatchesComplete);
    }

	private void OnMatchCreated(bool success, string extendedinfo, MatchInfo responsedata)
	{
        StartHost(responsedata);
        MessageHandler.Register();
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
        MessageHandler.Register();
    }

    

    public void StopMatchMaking() {
        StopHost();
    }

    public override void OnServerConnect(NetworkConnection Conn)
    {
        if (Conn.hostId >= 0)
        {
            Debug.Log("OnServerConnect");
            MessageHandler.SendStringMessage("Connected");
        }
    }

    
}