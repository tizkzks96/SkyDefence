using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtl : MonoBehaviour {
    public GameObject CurrentItem;
    GameObject newSkill;

    
    public enum ItemState
    {
        none,
        shield,
        heal
    }
    public ItemState itemState = ItemState.none;
    // Use this for initialization
    void Start() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            SetItem(gameObject.GetComponent<ItemCtl>().itemState);
            newSkill = Instantiate(CurrentItem, collision.gameObject.transform);

            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Invoke("destroyItem", 5.0f);
            
        }
    }

    void SetItem(ItemState setItem) {
        switch (setItem) {
            case ItemState.shield:
                //CurrentItem 배열에서 받아서와서 세팅
                //CurrentItem = 
                Resources.Load<FloatingText>("Prefabs/ItemSkill/Shield");
                PlayerState.instance.TriggerImmunity = true;
                Invoke("ItemCoolTime", 4.0f);
                Invoke("PlayerState.instance.TriggerImmunity", 4.0f);
                break;
            case ItemState.heal:
                Resources.Load<FloatingText>("Prefabs/ItemSkill/Shield");
                break;
        }
        
    }



    void ItemCoolTime() {
        Destroy(newSkill);
    }

    void destroyItem() {
        PlayerState.instance.TriggerImmunity = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
