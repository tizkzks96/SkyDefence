using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {
    private static FloatingText popupText;
    private static GameObject target;
    public static GameObject PopupTextParent;
    public static void Initialize(GameObject mop) {
        target = mop.transform.Find("TargetCanvas").gameObject;
        //canvas = GameObject.Find("Canvas");
        if (!popupText) {
            popupText = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
            print(popupText);
        }
            

    }
    
    public static void CreateFloatingText(string text, Transform location) {
        FloatingText instance = Instantiate(popupText);
        Vector2 screenPosition = new Vector2(0, 0);
            //Camera.main.WorldToScreenPoint(new Vector2(location.position.x+3, location.position.y + 4+ Random.Range(-.2f, .2f)));

        instance.transform.SetParent(target.transform, false);
        //instance.transform.position = screenPosition;
        instance.SetText(text);
    }

    
	
}
