using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyTile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Texture enemyIndicator;
    [SerializeField] private Texture destroyedTileIndicator;
    private Texture originalEnemyTile;
    private bool tileDestroyed = false;
    public bool TileDestroyed
    {
        get { return tileDestroyed; }
        set { tileDestroyed = value; }
    }

    // Use this for initialization
    void Start () {
		originalEnemyTile = this.GetComponent<RawImage>().texture;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Target enemy tile for attack
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        print(this.gameObject.transform.position.x + ", " + this.gameObject.transform.position.y);
        if (Bomber.instance.Active && Bomber.instance.CharSelected) {
            
            Bomber.instance.Active = false;
            Bomber.instance.OnDeselectChar();
            AttackTile();
        }
        else if (Champion.instance.Active && Champion.instance.CharSelected ||
            Champion.instance.GetComponentInChildren<Body>().Active && Champion.instance.GetComponentInChildren<Body>().CharSelected)
        {
            Champion.instance.Active = false;
            Champion.instance.OnDeselectChar();
            Champion.instance.GetComponentInChildren<Body>().Active = false;
            Champion.instance.GetComponentInChildren<Body>().OnDeselectChar();
            AttackTile();
        }
        
        
    }

    public void AttackTile()
    {
        //this.gameObject.SetActive(false);
        tileDestroyed = true;
        GameManager.instance.EnemyTargeted(this.name);
    }
    
    public void DestroyTile()
    {
        this.GetComponent<RawImage>().texture = destroyedTileIndicator;
    }

    public void EnemyPresent()
    {
        
        this.GetComponent<RawImage>().texture = enemyIndicator;
    }
    
}
