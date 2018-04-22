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

    [SerializeField] private Texture deadChampionImage;
    public Texture DeadChampionImage
    {
        get { return deadChampionImage; }
    }
    [SerializeField] private Texture liveChampionImage;
    public Texture LiveChampionImage
    {
        get { return liveChampionImage; }
    }
    [SerializeField] private Texture healChampionImage;
    public Texture HealChampionImage
    {
        get { return healChampionImage; }
    }

    [SerializeField] private Texture deadEngineerImage;
    public Texture DeadEngineerImage
    {
        get { return deadEngineerImage; }
    }
    [SerializeField] private Texture liveEngineerImage;
    public Texture LiveEngineerImage
    {
        get { return liveEngineerImage; }
    }
    [SerializeField] private Texture healEngineerImage;
    public Texture HealEngineerImage
    {
        get { return healEngineerImage; }
    }
    void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        Vector3 startPos = new Vector3(StartPositionX, StartPositionY, transform.position.z);
        transform.position = startPos;
        Scaler = this.gameObject.GetComponentInParent<CanvasScaler>();
        BodyDeployed = true;
        if (this.gameObject.name == "ChampionBody")
        {
            DeadImage = DeadChampionImage;
            HealImage = HealChampionImage;
            LiveImage = LiveChampionImage;
            this.Ship = Champion.instance.Ship;
        }
        if (this.gameObject.name == "EngineerBody")
        {
            Debug.Log("engineerBodyImage loaded");
            DeadImage = DeadEngineerImage;
            HealImage = HealEngineerImage;
            LiveImage = LiveEngineerImage;
            this.Ship = Engineer.instance.Ship;
        }
        this.gameObject.GetComponent<RawImage>().texture = LiveImage;
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
        base.OnPointerClick(eventData);
        if (CharSelected)
        {
            OnDeselectChar();
            Character[] charArray = { Champion.instance, Defender.instance, Bomber.instance, Engineer.instance,
                GameObject.Find("ChampionBody").GetComponent<Body>(), GameObject.Find("EngineerBody").GetComponent<Body>()};
            foreach (Character character in charArray)
            {
                if (character.Dead)
                {
                    character.gameObject.GetComponent<RawImage>().texture = character.gameObject.GetComponent<Character>().DeadImage;
                    character.gameObject.GetComponent<Character>().OnDeselectChar();
                }
            }
            GameManager.instance.EnableActiveIndicators();
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
        if (!other.gameObject.GetComponent<Character>() && !other.gameObject.GetComponent<EnemyTile>())
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
