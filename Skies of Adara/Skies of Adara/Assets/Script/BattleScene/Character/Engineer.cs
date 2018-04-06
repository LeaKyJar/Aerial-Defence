using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engineer : Character, IPointerClickHandler
{

    public static Engineer instance = null;
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
    private Vector3 priorPos;
    private int heal = 1;
    public int Heal
    {
        get { return heal; }
        set { heal = value; }
    }

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
        base.TakeDamage();
    }

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        base.OnPointerClick(pointerEventData);
        
        if (!Dead && heal>0 && !GameManager.instance.PreparationPhase)
        {
            GameManager.instance.SetInstructions("Engineers repair damaged units on the field." +
                    " Click on a damaged unit to repair it." +
                    " Otherwise click on the Engineer again to cancel repairs.");
            ReadyToHeal();
        }

    }

    //Puts Engineer into ready-to-heal state
    public void ReadyToHeal()
    {
        if (GameManager.instance.AtkPhase && !this.CharSelected)
        {
            OnSelectChar();
            Character[] charArray = { Champion.instance, Defender.instance, Bomber.instance, Engineer.instance,
                GameObject.Find("ChampionBody").GetComponent<Body>(), GameObject.Find("EngineerBody").GetComponent<Body>()};
            foreach(Character character in charArray)
            {
                if (character.Dead)
                {
                    character.CanBeHealed();
                }
            }
        }
        else if (GameManager.instance.AtkPhase && this.CharSelected)
        {
            OnDeselectChar();
            gameObject.GetComponentInChildren<Body>().OnDeselectChar();
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
        }
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Debug.Log(BodyDeployed);
        if (!BodyDeployed && LastTileObject != null)
        {
            
            /*Defender.instance.gameObject.SetActive(false);
            Bomber.instance.gameObject.SetActive(false);
            Champion.instance.gameObject.SetActive(false);
            if (Defender.instance.DefDeployed)
            {
                GameManager.instance.shield.SetActive(false);
            }*/
            this.DeployExtraBody = true;
            LightSurroundingTiles();
        }
        if (this.gameObject.GetComponentInChildren<Body>() != null)
        {
            this.gameObject.GetComponentInChildren<Body>().BeingDragged = false;
            if (this.gameObject.GetComponentInChildren<Body>().LastTileObject.GetComponent<Tile>().Occupied)
            {
                this.gameObject.transform.position = priorPos;
            }
            else
            {
                this.gameObject.GetComponentInChildren<Body>().OnEndDrag(eventData);

            }
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (this.gameObject.GetComponentInChildren<Body>() != null)
        {
            this.gameObject.GetComponentInChildren<Body>().BeingDragged = true;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (LastTileObject != null)
        {
            //LastTileObject.GetComponent<Tile>().Occupied = false;
            StopLightingSurroundingTiles();
            priorPos = LastTile;
        }
        if (this.gameObject.GetComponentInChildren<Body>() != null)
        {
            this.gameObject.GetComponentInChildren<Body>().OnBeginDrag(eventData);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public void LightSurroundingTiles()
    {

        if ((LastTileID + 1) % 5 != 0)
        {
            int upperTile = LastTileID + 1;
            GameObject.Find(upperTile.ToString()).GetComponent<Tile>().LightUp();
        }

        if (LastTileID % 5 != 0)
        {
            int lowerTile = LastTileID - 1;
            GameObject.Find(lowerTile.ToString()).GetComponent<Tile>().LightUp();
        }

        if (LastTileID > 4)
        {
            int leftTile = LastTileID - 5;
            GameObject.Find(leftTile.ToString()).GetComponent<Tile>().LightUp();
        }

        if (LastTileID < 20)
        {
            int rightTile = LastTileID + 5;
            GameObject.Find(rightTile.ToString()).GetComponent<Tile>().LightUp();
        }
    }

    public void StopLightingSurroundingTiles()
    {
        if ((LastTileID + 1) % 5 != 0)
        {
            int upperTile = LastTileID + 1;
            GameObject.Find(upperTile.ToString()).GetComponent<Tile>().StopLightUp();
        }

        if (LastTileID % 5 != 0)
        {
            int lowerTile = LastTileID - 1;
            GameObject.Find(lowerTile.ToString()).GetComponent<Tile>().StopLightUp();
        }

        if (LastTileID > 4)
        {
            int leftTile = LastTileID - 5;
            GameObject.Find(leftTile.ToString()).GetComponent<Tile>().StopLightUp();
        }

        if (LastTileID < 20)
        {
            int rightTile = LastTileID + 5;
            GameObject.Find(rightTile.ToString()).GetComponent<Tile>().StopLightUp();
        }
    }
    
    
}
