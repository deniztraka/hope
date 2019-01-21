using System.Collections;
using System.Collections.Generic;
using DTInventory.MonoBehaviours;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject DragableItemObject;

    public void OnDrag(PointerEventData eventData)
    {
        if (DragableItemObject == null)
        {
            //DragableItemObject = Instantiate(GetItemObject());
            DragableItemObject = Instantiate(gameObject);
            DragableItemObject.transform.parent = GameObject.Find("UI").transform;
            DragableItemObject.GetComponent<ItemDragHandler>().DragableItemObject = GetItemObject();
        }


        
        Touch touch = Input.GetTouch(0);

        //Update the Text on the screen depending on current position of the touch each frame
        DragableItemObject.transform.position = touch.position;
        ///gameObject.transform.parent = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(DragableItemObject);
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetItemSprite();
    }

    GameObject GetItemObject()
    {
        var slotBehaviour = transform.GetComponentInParent<SlotBehaviour>();
        return slotBehaviour.Item.GameObject;        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
