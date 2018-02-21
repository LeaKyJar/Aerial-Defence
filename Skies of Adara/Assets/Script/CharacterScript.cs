﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {

    [SerializeField] private float startPositionX = 0;
    [SerializeField] private float startPositionY = 0;


    // Use this for initialization
    void Start()
    {
        Vector3 startPos = new Vector3(startPositionX, startPositionY, transform.position.z);
        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        Touch myTouch = Input.GetTouch(0);
        float x = myTouch.position.x;
        float y = myTouch.position.y;
        Vector3 newPos = new Vector3(x, y, transform.position.z);
        transform.position = newPos;
    }

    void OpenCharacterFunctions()
    {

    }
}
