using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Defender : Character, IPointerClickHandler
{

    
    public static Defender instance = null;
    private bool defDeployed = false;
    public bool DefDeployed
    {
        get { return defDeployed; }
        set { defDeployed = value; }
    }
    private GameObject shield;


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

        shield.GetComponent<Shield>().Dead = true;
        shield.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        float scaledStartPositionX = StartPositionX * Scaler.transform.localScale.x;
        float scaledStartPositionY = StartPositionY * Scaler.transform.localScale.y;
        if (GameManager.instance.PreparationPhase)
        {
            //Deploy Unit
            if (LastTileObject != null)
            {
                transform.position = LastTile;
                LastTileObject.GetComponent<Tile>().Occupied = true;
                BodyDeployed = true;    
            }
            //Snap back to start
            else
            {
                ReturnToStart(scaledStartPositionX, scaledStartPositionY);
            }
            StopLightingAllTiles();
        }
        if (!DefDeployed && LastTileObject != null)
        {
            /*Champion.instance.gameObject.SetActive(false);
            Bomber.instance.gameObject.SetActive(false);
            Engineer.instance.gameObject.SetActive(false);*/
            LightAllTiles();
            GameManager.instance.SetInstructions("Deploy your Defender's shield");
        }
        BeingDragged = false;
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    

    public void InstantiateShield(Vector3 shieldDeploymentPos, string shieldDeploymentTileName)
    {
        print("reaches InstantiateShield()");
        DefDeployed = true;
        if (GameManager.instance.PreparationPhase)
        {
            GameManager.instance.DefenderToEngineer();
        }

        //instantiates Shield
        GameObject shieldPrefab = Resources.Load("Shield") as GameObject;
        shield = Instantiate(shieldPrefab, shieldDeploymentPos, transform.rotation);
        shield.transform.SetParent(this.gameObject.GetComponentInParent<Canvas>().transform, false);
        shield.transform.SetSiblingIndex(Bomber.instance.transform.GetSiblingIndex()+1);


        //assigns shield variables
        Shield shieldScript = shield.gameObject.GetComponent<Shield>();
        shieldScript.StartPositionX = shieldDeploymentPos.x;
        shieldScript.StartPositionY = shieldDeploymentPos.y;
        shieldScript.LastTileName = shieldDeploymentTileName;
        shield.name = "Shield";
    }

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!Dead && !Engineer.instance.CharSelected)
        {
            GameManager.instance.SetInstructions("Defenders don't have actions during combat." +
                    " Instead, the shield that you deployed earlier redirects damage from the tile to her." +
                    " The shield cannot be moved during combat phases.");
            OnDeselectChar();
        }
        else {
            base.OnPointerClick(pointerEventData);
        }
            
    }
    public override void Healed() {
        base.Healed();
        MessageHandler.turn.Add("H:Defender:" + this.LastTileID);
    }

}
