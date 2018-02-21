using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyTile : MonoBehaviour, IPointerClickHandler
{
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (GameManager.instance.ChampionActive) { 
            this.gameObject.SetActive(false);
            GameManager.instance.EnemyTargeted();
        }
        
    }
}
