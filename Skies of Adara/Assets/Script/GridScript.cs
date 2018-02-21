using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int numberOfTiles_x = 5;
    [SerializeField] private int numberOfTiles_y = 5;
    [SerializeField] private float distanceBetweenTiles= 50;
    private float startPositionX = 32;
    private float startPositionY = 32;
    public Canvas canvas;

	// Use this for initialization
	void Start () {
        CreateTiles();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void CreateTiles()
    {
        Canvas newCanvas = Instantiate(canvas) as Canvas;
        float offset_x = 0;
        float offset_y = 0;
        for (int ii = 0; ii < numberOfTiles_x; ii++)
        {
            for (int jj = 0; jj < numberOfTiles_y; jj++)
            {
                Vector3 position = new Vector3(startPositionX + offset_x, startPositionY + offset_y, 0);
                GameObject tile = Instantiate(tilePrefab, position, transform.rotation);
                tile.transform.SetParent(newCanvas.transform, false);
                tile.transform.position = position;
                offset_y += distanceBetweenTiles;
            }
            offset_x += distanceBetweenTiles;
            offset_y = 0;
        }
    }
}
