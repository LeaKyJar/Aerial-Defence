using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [SerializeField] private float startPositionX = 0;
    [SerializeField] private float startPositionY = 0;
    private Vector3 lastTile;
    private string lastTileName;
    private bool charSelected = false;
    public bool CharSelected
    {
        get { return charSelected; }
        set { charSelected = value; }
    }
    private bool dead = false;
    public bool Dead
    {
        get { return dead; }
        set { dead = value; }
    }
    private bool active = false;
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }


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
    
    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.PreparationPhase) { 
        Touch myTouch = Input.GetTouch(0);
        float x = myTouch.position.x;
        float y = myTouch.position.y;
        Vector3 newPos = new Vector3(x, y, transform.position.z);
        transform.position = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.instance.PreparationPhase)
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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        lastTile = other.gameObject.transform.position;
        lastTileName = other.gameObject.name;
    }


    public virtual void TakeDamage()
    {
        dead = true;
    }

}
