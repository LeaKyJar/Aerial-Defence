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

    // Target enemy tile for attack
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (Bomber.instance.Active && Bomber.instance.CharSelected) {
            Bomber.instance.Active = false;
            AttackTile();
        }
        else if (Champion.instance.Active && Champion.instance.CharSelected)
        {
            Champion.instance.Active = false;
            AttackTile();
        }
        
    }

    public void AttackTile()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.EnemyTargeted(this.name);
    }

    
}
