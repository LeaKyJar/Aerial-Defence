using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Champion : Character, IPointerClickHandler
{
    public static Champion instance = null;
    public GameObject atkButton;
    private Vector3 priorPos;
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

    //Only used for atk phase
    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        try
        {

            if (!Dead && !Engineer.instance.CharSelected && !GameManager.instance.PreparationPhase)
            {
                GameManager.instance.SetInstructions("Champions can attack enemies!" +
                    " Click on an enemy tile below to attack it." +
                    " Tiles that have already been hit can be hit again in case a ship has been repaired.");
                TargetEnemy();
            }
            else
            {
                base.OnPointerClick(pointerEventData);
            }
        }
        catch(Exception ex)
        {
            print(ex);
        }
        
    }

    public void TargetEnemy()
    {
        if (GameManager.instance.AtkPhase && !this.CharSelected && this.Active)
        {
            GameManager.instance.GridToggle();
            OnSelectChar();
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }

    //Used to allow tile movement during preparation phase
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (this.gameObject.GetComponentInChildren<Body>() != null)
        {
            this.gameObject.GetComponentInChildren<Body>().BeingDragged = true;
        }
    }

    //Used to snap character to last touched tile on grid
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (!BodyDeployed && LastTileObject != null)
        {
            this.DeployExtraBody = true;
            LightSurroundingTiles();
            GameManager.instance.SetInstructions("Touch any of the glowing tiles to deploy the Champion's Body!");
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

    //Unoccupies away an empty occupied tile
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    //Keeps current tile occupied
    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    //Stops surrounding tiles from lighting up
    public override void OnBeginDrag(PointerEventData eventData)
    {
        //base.OnBeginDrag(eventData);
        if (LastTileObject != null)
        {
            StopLightingSurroundingTiles();
            priorPos = LastTile;
        }
        if (this.gameObject.GetComponentInChildren<Body>() != null)
        {
            this.gameObject.GetComponentInChildren<Body>().OnBeginDrag(eventData);
        }
    }

    //Indicates feasible body deployment areas
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

    //Stops indicating feasible body deployment areas
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

    //Records most recent tile object collision
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void MakeInactive()
    {
        this.gameObject.GetComponentInChildren<Body>().MakeInactive();
        base.MakeInactive();
    }

    public override void Healed()
    {
        base.Healed();
        MessageHandler.turn.Add("H:Champion:" + this.LastTileID);
    }

}
