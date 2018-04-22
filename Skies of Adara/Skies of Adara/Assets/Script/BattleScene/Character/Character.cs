using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    public GameObject Ship;
    private GameObject activeIndicator;
    public GameObject ActiveIndicator
    {
        get { return activeIndicator; }
        set { activeIndicator = value; }
    }

    private bool bodyDeployed = false;
    public bool BodyDeployed
    {
        get { return bodyDeployed; }
        set { bodyDeployed = value; }
    }
    private bool deployExtraBody = false;
    public bool DeployExtraBody
    {
        get { return deployExtraBody; }
        set { deployExtraBody = value; }
    }
    private Vector3 spawnPos;
    private GameObject extraBody;
    private CanvasScaler scaler;
    public CanvasScaler Scaler
    {
        get { return scaler; }
        set { scaler = value; }
    }
    [SerializeField] private float startPositionX = 0;
    public float StartPositionX
    {
        get { return startPositionX; }
        set { startPositionX = value; }
    }
    [SerializeField] private float startPositionY = 0;
    public float StartPositionY
    {
        get { return startPositionY; }
        set { startPositionY = value; }
    }
    private Vector3 lastTile;
    public Vector3 LastTile
    {
        get { return lastTile; }
        set { lastTile = value; }
    }
    private int lastTileID=99999;
    public int LastTileID
    {
        get { return lastTileID; }
        set { lastTileID = value; }
    }
    private GameObject lastTileObject = null;
    public GameObject LastTileObject
    {
        get { return lastTileObject; }
        set { lastTileObject = value; }
    }
    private string lastTileName;
    public string LastTileName
    {
        get { return lastTileName; }
        set { lastTileName = value; }
    }
    private bool charSelected = false;
    public bool CharSelected
    {
        get { return charSelected; }
        set { charSelected = value; }
    }
    private bool dead = false;
    public bool Dead
    {
        get { return dead; }
        set { dead = value; }
    }
    [SerializeField] private Texture deadImage;
    public Texture DeadImage
    {
        get { return deadImage; }
        set { deadImage = value; }
    }
    [SerializeField] private Texture liveImage;
    public Texture LiveImage
    {
        get { return liveImage; }
        set { liveImage = value; }
    }
    [SerializeField] private Texture healImage;
    public Texture HealImage
    {
        get { return healImage; }
        set { healImage = value; }
    }
    private bool active = false;
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }
    public bool BeingDragged = false;


    // Use this for initialization
    void Start()
    {
        Vector3 startPos = new Vector3(startPositionX, startPositionY, transform.position.z);
        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.PreparationPhase) {
            LightAllTiles();
            Touch myTouch = Input.GetTouch(0);
            float x = myTouch.position.x;
            float y = myTouch.position.y;
            Vector3 newPos = new Vector3(x, y, transform.position.z);
            transform.position = newPos;
            BeingDragged = true;
        }
    }

    public void ReturnToStart(float scaledStartPositionX, float scaledStartPositionY)
    {
        Vector3 startPos = new Vector3(scaledStartPositionX, scaledStartPositionY, transform.position.z);
        //print(scaledStartPositionX + ", " + scaledStartPositionY);
        transform.position = startPos;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {

        float scaledStartPositionX = StartPositionX * Scaler.transform.localScale.x;
        float scaledStartPositionY = StartPositionY * Scaler.transform.localScale.y;
        if (GameManager.instance.PreparationPhase)
        {
            //Deploy Unit
            if (lastTileObject != null)
            {
                transform.position = lastTile;
                lastTileObject.GetComponent<Tile>().Occupied = true;
            }
            //Snap back to start
            else
            {
                ReturnToStart(scaledStartPositionX, scaledStartPositionY);
            }
            BeingDragged = false;
            StopLightingAllTiles();
            
        }
    }
    

    public void InstantiateBody(Vector3 adjacentTilePos, string adjacentTileName)
    {        
        //instantiates Body
        extraBody = Resources.Load("Body") as GameObject;
        GameObject body = Instantiate(extraBody, adjacentTilePos, transform.rotation);
        body.transform.SetParent(this.gameObject.transform, false);

        //assigns body variables
        Body bodyScript = body.gameObject.GetComponent<Body>();
        bodyScript.startPositionX = adjacentTilePos.x;
        bodyScript.startPositionY = adjacentTilePos.y;
        bodyScript.LastTileName = adjacentTileName;
        body.name = this.gameObject.name + "Body";       
    }
    
    public virtual void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        //Prevents Characters and occupied Tiles instead of empty Tiles from being selected
        try
        {
            if (!other.gameObject.GetComponent<EnemyTile>() && !other.gameObject.GetComponent<Character>() && !other.gameObject.GetComponent<Tile>().Occupied)
            {
                lastTileObject = other.gameObject;
                lastTile = lastTileObject.transform.position;
                lastTileName = lastTileObject.name;
                lastTileID = Int32.Parse(lastTileName);
                print(this.gameObject.name + ": " + lastTileName);
            }
        }
        catch(Exception ex)
        {
            print(ex);
        }
        
    }

    public virtual void OnTriggerStay(Collider other)
    {
        try
        {
            if (!other.gameObject.GetComponent<EnemyTile>() && !other.gameObject.GetComponent<Character>() && !other.gameObject.GetComponent<Tile>().Occupied && !BeingDragged)
            {
                other.gameObject.GetComponent<Tile>().Occupied = true;
            }
        }catch(Exception ex)
        {
            print(ex);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.GetComponent<Character>() && !other.gameObject.GetComponent<EnemyTile>())
        {
            other.gameObject.GetComponent<Tile>().Occupied = false;
        }
    }


    public virtual void TakeDamage()
    {
        if (!dead)
        {
            dead = true;
            this.gameObject.GetComponent<RawImage>().texture = deadImage;
            MessageHandler.SendStringMessage(LastTileName);
            GameManager.instance.instructions.SetActive(false);
            GameManager.instance.FireMissile(Ship.transform.position);
            Ship.GetComponent<Ship>().Destroyed();
        }
        else
        {
            MessageHandler.SendStringMessage("Hit Received");
        }
        
    }
    
    public virtual void CanBeHealed()
    {
        if (Dead)
        {
            this.gameObject.GetComponent<Animation>().Play();
            this.gameObject.GetComponent<RawImage>().texture = this.gameObject.GetComponent<Character>().HealImage;
        }
    }

    public virtual void Healed()
    {
        print("heal boop");
        dead = false;
        this.gameObject.GetComponent<RawImage>().texture = this.gameObject.GetComponent<Character>().LiveImage;
        this.gameObject.GetComponent<Animation>().Stop();
        this.gameObject.GetComponent<RawImage>().color = new Vector4(200 / 255f, 200 / 255f, 200 / 255f, 1f);
        Ship.SetActive(true);
    }
    
    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {
        print("boop");
        if (Dead && (Engineer.instance.CharSelected || Engineer.instance.GetComponentInChildren<Body>().CharSelected) && Engineer.instance.Heal>0)
        {
            print("registered heal");
            Healed();
            Engineer.instance.OnDeselectChar();
            Engineer.instance.GetComponentInChildren<Body>().OnDeselectChar();
            Engineer.instance.MakeInactive();
            Engineer.instance.GetComponentInChildren<Body>().MakeInactive();
            Engineer.instance.Heal -= 1;
            print(Engineer.instance.Heal);
            Character[] charArray = { Champion.instance, Defender.instance, Bomber.instance, Engineer.instance,
                GameObject.Find("ChampionBody").GetComponent<Body>(), GameObject.Find("EngineerBody").GetComponent<Body>()};
            foreach (Character character in charArray)
            {
                if (character.Dead)
                {
                    character.OnDeselectChar();

                    character.gameObject.GetComponent<RawImage>().texture = character.gameObject.GetComponent<Character>().deadImage;
                }
            }
            if (!Champion.instance.Active && !Bomber.instance.Active && !Engineer.instance.Active)
            {
                GameManager.instance.EndTurn();
            }
        }
        else if (GameManager.instance.PreparationPhase && GameManager.instance.DefenderPreparationPhase && !Defender.instance.DefDeployed && Defender.instance.BodyDeployed)
        {
            Defender.instance.InstantiateShield(lastTile, lastTileName);
        }
    }
    

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (LastTileObject != null)
        {
            LastTileObject.GetComponent<Tile>().Occupied = false;
        }
        
    }

    public void LightAllTiles()
    {
        GameObject.Find("FriendlyGrid").GetComponent<GridScript>().LightTiles();
    }

    public void StopLightingAllTiles()
    {
        GameObject.Find("FriendlyGrid").GetComponent<GridScript>().StopLightingTiles();
    }

    public void OnSelectChar()
    {
        if (GameManager.instance.AtkPhase)
        {
            this.gameObject.GetComponent<Animation>().Play();
            CharSelected = true;
            activeIndicator.GetComponent<CharacterActiveIndicator>().DisableIndicators();
        }
        
    }

    public void OnDeselectChar()
    {
        this.gameObject.GetComponent<Animation>().Stop();
        this.gameObject.GetComponent<RawImage>().color = new Vector4 (200/255f, 200/255f, 200/255f,1f);
        try
        {
            activeIndicator.GetComponent<CharacterActiveIndicator>().EnableIndicators();
        } catch(Exception ex)
        {
            Debug.Log(ex);
        }
        CharSelected = false;
    }

    public virtual void MakeActive()
    {
        active = true;
        InstantiateActiveIndicator();
    }

    public virtual void MakeInactive()
    {
        active = false;
        DestroyActiveIndicator();
    }

    public void InstantiateActiveIndicator()
    {

        //instantiates ActiveIndicator
        GameObject indicator = Resources.Load("CharacterActiveIndicator") as GameObject;
        activeIndicator = Instantiate(indicator, lastTile, transform.rotation);
        activeIndicator.transform.SetParent(this.gameObject.GetComponentInParent<Canvas>().transform, false);
        activeIndicator.transform.SetSiblingIndex(15);

        activeIndicator.transform.position = lastTile;
        activeIndicator.GetComponent<CharacterActiveIndicator>().ActiveCharacter = this.gameObject;
        
    }

    public void DestroyActiveIndicator()
    {
        Destroy(activeIndicator);
        Debug.Log("Active Indicator Destroyed");
    }
}
