using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    private Character[] doubleSpaceCharArray;
    [SerializeField] private Texture originalTile;
    [SerializeField] private Texture selectedTile;
    [SerializeField] private Texture brokenTile;
    private bool occupied = false;
    public bool Occupied
    {
        get { return occupied; }
        set { occupied = value; }
    }
    
    // Use this for initialization
    void Start () {
        doubleSpaceCharArray = new Character[] { Champion.instance, Engineer.instance };
    }
	
	// Update is called once per frame
	void Update () {
		/*if (occupied)
        {
            BreakTile();
        }
        else
        {
            this.gameObject.GetComponent<RawImage>().texture = originalTile;
        }*/
	}

    public void DeployBody(Character character, Vector3 pos)
    {
        if(!character.BodyDeployed)
        {
            character.InstantiateBody(pos, name);
            character.GetComponentInChildren<Body>().LastTileObject = this.gameObject;
            character.GetComponentInChildren<Body>().LastTileName = this.gameObject.name;
            character.GetComponentInChildren<Body>().LastTile = this.gameObject.transform.position;
            character.GetComponentInChildren<Body>().LastTileID = Int32.Parse(this.gameObject.name);
            character.BodyDeployed = true;
            character.DeployExtraBody = false;
            occupied = true;
            //ReactivateUnits();
        }
        if(character == Champion.instance)
        {
            GameManager.instance.ChampionToDefender();
        }
        else if(character == Engineer.instance)
        {
            GameManager.instance.EngineerToBomber();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 pos = this.gameObject.transform.position;
        string tileName = this.gameObject.name;
        int tileID = Int32.Parse(tileName);
        try {
            if (GameManager.instance.PreparationPhase)
            {
                //Spawns additional body for Champion or Engineer
                foreach (Character character in doubleSpaceCharArray)
                {
                    if (character.DeployExtraBody && !Occupied)
                    {
                        print(character.name);
                        if (tileID == character.LastTileID - 5)
                        {
                            DeployBody(character, pos);
                        }

                        if (tileID == character.LastTileID + 5)
                        {
                            DeployBody(character, pos);
                        }

                        if ((character.LastTileID + 1) % 5 != 0)
                        {
                            if (tileID == character.LastTileID + 1)
                            {
                                DeployBody(character, pos);
                            }
                        }
                        if (character.LastTileID % 5 != 0)
                        {
                            if (tileID == character.LastTileID - 1)
                            {
                                DeployBody(character, pos);
                            }
                        }
                    }


                }

                //Deploys shield
                Defender defender = Defender.instance;
                if (!defender.DefDeployed && defender.BodyDeployed)
                {
                    print("satisfies DeployDef");
                    defender.InstantiateShield(pos, name);
                    
                    
                    //ReactivateUnits();
                    GameManager.instance.DefenderToEngineer();
                }

                //ReactivateUnits();

            }
        }
        catch(Exception ex)
        {
            print(ex);
        }
        
       
        
    }

    public void ReactivateUnits()
    {
        Champion.instance.gameObject.SetActive(true);
        Defender.instance.gameObject.SetActive(true);
        Bomber.instance.gameObject.SetActive(true);
        Engineer.instance.gameObject.SetActive(true);
        if (Defender.instance.DefDeployed && GameManager.instance.shield != null)
        {
            GameManager.instance.shield.SetActive(true);
        }
    }

    public void BreakTile()
    {
        this.gameObject.GetComponent<RawImage>().texture = brokenTile;
    }

    public void LightUp()
    {
        this.gameObject.GetComponent<RawImage>().texture = selectedTile;
        this.gameObject.GetComponent<Animation>().Play();
    }

    public void StopLightUp()
    {
        this.gameObject.GetComponent<Animation>().Stop();
        this.gameObject.transform.localScale = new Vector3 (1,1,1);
        this.gameObject.GetComponent<RawImage>().texture = originalTile;
    }
    
}
