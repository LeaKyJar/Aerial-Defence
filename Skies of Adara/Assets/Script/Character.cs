using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [SerializeField] private float startPositionX = 0;
    [SerializeField] private float startPositionY = 0;
    private Vector3 lastTile;


    // Use this for initialization
    void Start()
    {
        Vector3 startPos = new Vector3(startPositionX, startPositionY, transform.position.z);
        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void OpenCharacterFunctions()
    {

    }


    public void OnDrag(PointerEventData eventData)
    {
        Touch myTouch = Input.GetTouch(0);
        float x = myTouch.position.x;
        float y = myTouch.position.y;
        Vector3 newPos = new Vector3(x, y, transform.position.z);
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.position.x > 254)
        {
            Vector3 startPos = new Vector3(startPositionX, startPositionY, transform.position.z);
            transform.position = startPos;
        }
        else
        {
            transform.position = lastTile;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //lastTile;
    }
    private void OnTriggerEnter(Collider other)
    {
        //print("touchy");
        lastTile = other.gameObject.transform.position;
    }



}
