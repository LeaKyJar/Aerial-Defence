using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject enemyTilePrefab;
    [SerializeField] private int numberOfTiles_x = 5;
    [SerializeField] private int numberOfTiles_y = 5;
    [SerializeField] private float distanceBetweenTiles= 50;
    private float startPositionX = 32;
    private float startPositionY = 32;
    public Canvas canvas;
    public Canvas canvasPrefab;
    private bool enemy = false;
    private GameObject grid;

	// Use this for initialization
	void Start () {
        grid = this.gameObject;
        IsEnemy();
        CreateTiles();
        if (enemy)
        {
            grid.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void IsEnemy()
    {
        if (this.name == "EnemyGrid")
        {
            enemy = true;
        }
    }

    void CreateTiles()
    {
        Canvas newCanvas = Instantiate(canvasPrefab);
        newCanvas.transform.SetParent(grid.transform, false);
        int tileID = 0;
        if (enemy)
        {
            tileID += 100;
        }
        float offset_x = 0;
        float offset_y = 0;
        for (int ii = 0; ii < numberOfTiles_x; ii++)
        {
            for (int jj = 0; jj < numberOfTiles_y; jj++)
            {
                Vector3 position = new Vector3(startPositionX + offset_x, startPositionY + offset_y, 0);
                if (!enemy)
                {
                    GameObject tile = Instantiate(tilePrefab, position, transform.rotation);
                    tile.transform.SetParent(newCanvas.transform, false);
                    tile.transform.position = position;
                    tile.name = tileID.ToString();
                    tileID += 1;
                    
                }
                else
                {
                    GameObject tile = Instantiate(enemyTilePrefab, position, transform.rotation);
                    tile.transform.SetParent(newCanvas.transform, false);
                    tile.transform.position = position;
                    tile.name = tileID.ToString();
                    tileID += 1;
                }
                offset_y += distanceBetweenTiles;

            }
            offset_x += distanceBetweenTiles;
            offset_y = 0;
        }
    }
}
