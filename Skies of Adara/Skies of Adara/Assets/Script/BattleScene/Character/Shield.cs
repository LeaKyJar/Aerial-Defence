using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shield : Character {

	// Use this for initialization
	void Start () {
        Vector3 startPos = new Vector3(StartPositionX, StartPositionY, transform.position.z);
        transform.position = startPos;
        Scaler = this.gameObject.GetComponentInParent<CanvasScaler>();
        /*if (GameManager.instance.PreparationPhase && Champion.instance.BodyDeployed && Engineer.instance.BodyDeployed && Bomber.instance.BodyDeployed && Defender.instance.BodyDeployed)
        {
            GameManager.instance.ready.GetComponent<Button>().interactable = true;
            GameManager.instance.SetInstructions("Click the Ready! button when your're done!");
        }*/
        GameManager.instance.shield = this.gameObject;
        Defender.instance.StopLightingAllTiles();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    // Destroys defender and disappears. Does not alert Enemy to own status.
    public override void TakeDamage()
    {
        print("Shield.TakeDamage() called");
        Dead = true;
        Defender.instance.TakeDamage();
        this.gameObject.SetActive(false);
    }

    /*public override void OnEndDrag(PointerEventData eventData)
    {
    }*/

    public override void OnBeginDrag(PointerEventData eventData)
    {
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
                BodyDeployed = true;
            }
            //Snap back to start
            else
            {
                ReturnToStart(scaledStartPositionX, scaledStartPositionY);
            }
            StopLightingAllTiles();

        }
    }

    public override void OnTriggerEnter(Collider other)
    {
            LastTileObject = other.gameObject;
            LastTile = LastTileObject.transform.position;
            LastTileName = LastTileObject.name;
            print(this.gameObject.name + ": " + LastTileName);
    }

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        Character[] characterArray = new Character[] { Champion.instance, Engineer.instance, Bomber.instance };
        foreach(Character character in characterArray)
        {
            if (LastTileID == character.LastTileID)
            {
                character.OnPointerClick(pointerEventData);
            }
        }
        
    }

    public override void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.GetComponent<Character>())
        {
            LastTileObject = other.gameObject;
            LastTile = LastTileObject.transform.position;
            LastTileName = LastTileObject.name;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
    }
}
