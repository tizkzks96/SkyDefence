using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour {
    public static PlayerState instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    Vector3 mousePos;

    Animator animator;
    GameObject hpGage;
    GameObject targetCanvas;
    Transform targetTr;

    public GameObject floatingTextPrefab;
    public Joystick joystick;
    public float speed;
    public float initHp;

    public bool isDie;
    private float hp;
    private bool triggerImmunity;

    float currentSpeed;
    float playerScaleX = 1.0f;
    float playerScaleY = 1.0f;
    float playerScaleZ = 1.0f;

    public bool TriggerImmunity
    {
        get {
            return triggerImmunity;
        }

        set {
            triggerImmunity = value;
        }
    }

    private void Start() {
        //FloatingTextController.Initialize(gameObject);
        targetCanvas = gameObject.transform.Find("TargetCanvas").gameObject;
        targetTr = targetCanvas.transform;
        hpGage = gameObject.transform.Find("TargetCanvas/HpGage").gameObject;
        targetCanvas = gameObject.transform.Find("TargetCanvas").gameObject;
        currentSpeed = speed;
        animator = GetComponent<Animator>();
        isDie = false;
        hp = initHp;
    }

    // Update is called once per frame
    void Update() {
        Move();
        IsDie();
    }


    public float GetCurrentSpeed() {
        print("currentSpeed" + currentSpeed);
        return currentSpeed;
    }

    public void SetCurrentSpeed(float speed) {
        currentSpeed = speed;
    }

    public float GetSpeed() {
        return speed;
    }

    public float GetHp() {
        return hp;
    }

    public void SetHp(float setHp) {
        hp = setHp;
    }

    public bool GetState() {
        return isDie;
    }

    public void Attack() {
        ArcherCtl.instance.Attack();

    }

    void ShowFloatingText(int amount) {

        if (TriggerImmunity == false)
        {
            floatingTextPrefab.transform.Find("PopupText").GetComponent<Text>().text = amount + "";
            Instantiate(floatingTextPrefab, targetTr.position, Quaternion.identity, targetTr);
        }

        else
        {
            floatingTextPrefab.transform.Find("PopupText").GetComponent<Text>().text = "면역";
            Instantiate(floatingTextPrefab, targetTr.position, Quaternion.identity, targetTr);
        }
    }

    public void Hit(int amount) {
        if (!isDie) {
            //FloatingTextController.CreateFloatingText(amount.ToString(), transform);
            if (floatingTextPrefab != null) {
                ShowFloatingText(amount);
            }
            //면역 상태 체크
            if (TriggerImmunity == false) {
                if ((hp -= amount) <= 0) {
                    //죽었을때
                    isDie = true;
                    animator.SetTrigger("death");
                    gameObject.GetComponent<PlayerState>().enabled = false;
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    //gameObject.transform.Find("Shadow").gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                else {//살았을때
                    animator.SetTrigger("hit");
                }
                hpGage.GetComponent<Image>().fillAmount = hp / initHp;
            }
            
        }
    }


    public void IsDie() {
        if (gameObject.GetComponent<PlayerState>().GetState()) {
            gameObject.GetComponent<ArcherCtl>().enabled = false;
        }
    }

    void Move() {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

        if (moveVector != Vector3.zero) {
            transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
            animator.SetBool("walk", true);
            if (moveVector.x < 0)
                transform.localScale = new Vector3(playerScaleX, playerScaleY, playerScaleZ);
            else
                transform.localScale = new Vector3(-playerScaleX, playerScaleY, playerScaleZ);
        }
        else animator.SetBool("walk", false);

        
        //keybord
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, currentSpeed * Time.deltaTime, 0);
            if (!animator.GetBool("walk")) {
                animator.SetBool("walk", true);
            }

        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, -currentSpeed * Time.deltaTime, 0);
            if (!animator.GetBool("walk")) {
                animator.SetBool("walk", true);
            }
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-currentSpeed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(playerScaleX, playerScaleY, playerScaleZ);
            targetCanvas.GetComponent<TargetCanvasCtl>().ChangeCanvsScale();
            if (!animator.GetBool("walk")) {
                animator.SetBool("walk", true);
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(currentSpeed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(-playerScaleX, playerScaleX, playerScaleZ);
            targetCanvas.GetComponent<TargetCanvasCtl>().ChangeCanvsScale();
            if (!animator.GetBool("walk")) {
                animator.SetBool("walk", true);
            }
        }

        //if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
        //    if (animator.GetBool("walk")) {
        //        animator.SetBool("walk", false);
        //    }
        //}


    }

    public void CheckPlayerDirectionOnceAttack() {
        Vector2 target;
        target.x = mousePos.x - transform.position.x;
        if (target.x > 0) {
            transform.localScale = new Vector3(-playerScaleX, playerScaleY, playerScaleZ);
            targetCanvas.GetComponent<TargetCanvasCtl>().ChangeCanvsScale();
        }
        else {
            transform.localScale = new Vector3(playerScaleX, playerScaleY, playerScaleZ);
            targetCanvas.GetComponent<TargetCanvasCtl>().ChangeCanvsScale();
        }
    }



}
