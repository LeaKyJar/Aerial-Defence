using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Body : Character, IPointerClickHandler {
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
    // Use this for initialization
    void Start () {
        Vector3 startPos = new Vector3(StartPositionX, StartPositionY, transform.position.z);
        transform.position = startPos;
        Scaler = this.gameObject.GetComponentInParent<CanvasScaler>();
        BodyDeployed = true;
        /*if (GameManager.instance.PreparationPhase && Champion.instance.BodyDeployed && Engineer.instance.BodyDeployed && Bomber.instance.BodyDeployed && Defender.instance.BodyDeployed)
        {
            GameManager.instance.ready.GetComponent<Button>().interactable = true;
            GameManager.instance.SetInstructions("Click the Ready! button when your're done!");
        }*/
        StopLightingSurroundingTiles();
    }

    void StopLightingSurroundingTiles()
    {
        if (this.gameObject.name == "ChampionBody")
        {
            this.gameObject.GetComponentInParent<Champion>().StopLightingSurroundingTiles();
        }
        else if (this.gameObject.name == "EngineerBody")
        {
            this.gameObject.GetComponentInParent<Engineer>().StopLightingSurroundingTiles();
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (Engineer.instance.CharSelected)
        {
            base.OnPointerClick(eventData);
        }
        else if (CharSelected)
        {
            OnDeselectChar();
        }
        else
        {
            if (Active)
            {
                OnSelectChar();
                if (this.gameObject.name == "ChampionBody" && !Dead)
                {
                    if (GameManager.instance.AtkPhase)
                    {
                        GameManager.instance.GridToggle();
                    }
                    //OnDeselectChar();
                }
                else if (this.gameObject.name == "EngineerBody" && !Dead && Engineer.instance.Heal > 0)
                {
                    this.gameObject.GetComponentInParent<Engineer>().ReadyToHeal();
                    //OnDeselectChar();
                }

            }
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        LastTileObject.GetComponent<Tile>().Occupied = true;
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Character>())
        {
            print("Object Set: " + other.gameObject.name);
            LastTileObject = other.gameObject;
            LastTile = LastTileObject.transform.position;
            LastTileName = LastTileObject.name;
            LastTileID = Int32.Parse(LastTileName);
            print(this.gameObject.name + ": " + LastTileName);
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }
}
