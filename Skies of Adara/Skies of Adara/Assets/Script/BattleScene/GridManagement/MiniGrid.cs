using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGrid : MonoBehaviour {

    [SerializeField] private GameObject hitMissPrefab;
    [SerializeField] private int numberOfTiles_x = 5;
    [SerializeField] private int numberOfTiles_y = 5;
    [SerializeField] private float distanceBetweenTiles = 5;
    [SerializeField] private float startPositionX = 275;
    [SerializeField] private float startPositionY = 490;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Individually instantiates hitMiss in correct position
    public void MakeHitMiss(int hitMissID, bool hitOrMiss)
    {
        CanvasScaler scaler = this.gameObject.GetComponentInParent<CanvasScaler>();
        float scaledX = scaler.transform.localScale.x;
        float scaledY = scaler.transform.localScale.y;
        int tileID = 0;

        float offset_x = 0;
        float offset_y = 0;
        float scaledStartPositionX = startPositionX * scaledX;
        float scaledStartPositionY = startPositionY * scaledY;

        for (int ii = 0; ii < numberOfTiles_x; ii++)
        {
            for (int jj = 0; jj < numberOfTiles_y; jj++)
            {
                //Vector3 position = new Vector3(scaledStartPositionX + offset_x, scaledStartPositionY + offset_y, 0);
                Vector3 position = new Vector3(scaledStartPositionX + offset_x, scaledStartPositionY + offset_y, 0);
                if(hitMissID == tileID)
                {
                    GameObject hitMiss = Instantiate(hitMissPrefab, position, transform.rotation);
                    hitMiss.transform.SetParent(this.gameObject.transform, false);
                    hitMiss.transform.position = position;
                    hitMiss.name = tileID.ToString();
                    if (hitOrMiss)
                    {
                        hitMiss.GetComponent<HitMiss>().MakeHit();
                    }
                    else
                    {
                        hitMiss.GetComponent<HitMiss>().MakeMiss();
                    }
                }
                tileID += 1;
                //offset_y += distanceBetweenTiles;
                offset_y += (distanceBetweenTiles * scaledY);

            }
            //offset_x += distanceBetweenTiles;
            offset_x += (distanceBetweenTiles * scaledX);
            offset_y = 0;
        }
    }

    public void ParseString(string args)
    {
        string[] splitString = args.Split(';');

        bool tempBoolean = false;
        int tempInt;
        foreach (string s in splitString)
        {
            if (s.Equals("True"))
            {
                tempBoolean = true;
            }
            else if (s.Equals("False"))
            {
                tempBoolean = false;
            }
            
            if(Int32.TryParse(s, out tempInt))
            {
                MakeHitMiss(tempInt, tempBoolean);
            }
        }
        
    }

    //Called by GameManager for overall grid creation
    public void MakeHitMissSummary(string args)
    {
        ParseString(args);
    }
}
