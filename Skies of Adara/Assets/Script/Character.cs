using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    [SerializeField] private float startPositionX = 0;
    [SerializeField] private float startPositionY = 0;
    private Vector3 lastTile;
    private bool charSelected = false;
    private bool dead = false;
    private bool active = false;
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }
    public GameObject atkButton;


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
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (GameManager.instance.AtkPhase && !charSelected && GameManager.instance.ChampionActive)
        {
            atkButton.SetActive(true);
            charSelected = true;
        }
        else if(GameManager.instance.AtkPhase && charSelected)
        {
            atkButton.SetActive(false);
            charSelected = false;
        }
    }

    public void TakeDamage()
    {
        dead = true;
    }

}
