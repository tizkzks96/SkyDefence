using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiretion : MonoBehaviour {

    public Joystick joystick;
    Transform parentTr;

    private void Start() {
        parentTr = transform.parent.transform;
    }
    private void Update() {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

        if (moveVector != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);

            //transform.position = new Vector3(parentTr.position.x, parentTr.position.y, parentTr.position.z);
        }
    }
}
