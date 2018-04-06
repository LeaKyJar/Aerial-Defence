using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Bomber : Character, IPointerClickHandler
{
    public GameObject atkButton;
    public static Bomber instance = null;
    /*
    [SerializeField] private Texture deadImage;
    public Texture DeadImage
    {
        get { return deadImage; }
    }
    [SerializeField] private Texture liveImage;
    public Texture LiveImage
    {
        get { return liveImage; }
    }
    [SerializeField] private Texture healImage;
    public Texture HealImage
    {
        get { return healImage; }
    }
    */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    // Use this for initialization
    void Start () {
        Scaler = this.gameObject.GetComponentInParent<CanvasScaler>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public override void TakeDamage()
    {
        int correspondingEnemyTile = Int32.Parse(LastTileName)+100;
        string correspondingEnemyTileName = correspondingEnemyTile.ToString();
        GameManager.instance.enemyGrid.SetActive(true);
        GameObject kamikazeTargetTile = GameObject.Find(correspondingEnemyTileName);
        if (!kamikazeTargetTile.GetComponent<EnemyTile>().TileDestroyed)
        {
            GameManager.instance.EnemyTargeted(correspondingEnemyTile.ToString());
        }
        GameManager.instance.enemyGrid.SetActive(false);

        base.TakeDamage();
    }

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!Dead && !Engineer.instance.CharSelected && !GameManager.instance.PreparationPhase)
        {
            GameManager.instance.SetInstructions("Bombers can attack enemies!" + 
                    " In addition, they destroy their corresponding tiles when they get damaged." +
                    " Click on an enemy tile below to attack it." +
                    " Tiles that have already been hit can be hit again in case a ship has been repaired.");
            TargetEnemy();
        }
        else
        {
            base.OnPointerClick(pointerEventData);
        }
    }

    public void TargetEnemy()
    {
        if (GameManager.instance.AtkPhase && !this.CharSelected && this.Active)
        {
            //atkButton.SetActive(true);
            GameManager.instance.GridToggle();
            OnSelectChar();
        }
        else if (GameManager.instance.AtkPhase && this.CharSelected)
        {
            atkButton.SetActive(false);
            //OnDeselectChar();
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.instance.PreparationPhase)
        {
            base.OnEndDrag(eventData);
            BodyDeployed = true;
            GameManager.instance.BombertoReady();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }
}


