using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MopCtl : MonoBehaviour
{
    NavMeshAgent nvAgent;
    Transform mopTr;
    Transform playerTr;
    GameObject playerGO;
    GameObject hpGage;
    GameObject targetCanvas;
    Transform targetTr;
    Animator animator;

    public GameObject particleOfDeath;
    public GameObject floatingTextPrefab;
    public float mopScaleX;             //the scaleX of the mop starts the game with
    public float mopScaleY;             //the scaleY of the mop starts the game with
    public float speed = 2.0f;          //the speed of the mop starts the game with
    public float traceDist = 10.0f;     
    public float attackDist = 2.0f;
    public float attackBaseCoolTime;
    public float attackBaseDelayTime;
    public int damageOfBase = 1;
    public enum State {
        idle,
        trace,
        attack,
        die
    }
    public State state = State.idle;
    bool isDie = false;

    public float initHp;
    float hp;

    // Use this for initialization
    void Start() {
        //FloatingTextController.Initialize(gameObject);

        hpGage = gameObject.transform.Find("TargetCanvas/HpGage").gameObject;
        targetCanvas = gameObject.transform.Find("TargetCanvas").gameObject;
        targetTr = targetCanvas.transform;
        hp = initHp;
        nvAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mopTr = GetComponent<Transform>();
        playerGO = FindTarget();
        playerTr = playerGO.transform;
        SetScale();
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    private GameObject FindTarget() {
        return GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "player") {
            state = State.trace;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        
    }
    // Update is called once per frame

    
    public void SetHp(float setHp) {
        hp = setHp;
    }

    public float GetHp() {
        return hp;
    }

    public void SetScale() {
        mopScaleX = transform.localScale.x;
        mopScaleY = transform.localScale.y;
    }

    public void Hit(int amount) {
        if (!isDie) {
            //FloatingTextController.CreateFloatingText(amount.ToString(), transform);
            if (floatingTextPrefab != null)
            {
                ShowFloatingText(amount);
            }

            if ((hp -= amount) <= 0) {
                state = State.die;
                isDie = true;
                animator.SetTrigger("death");
                gameObject.GetComponent<MopCtl>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.transform.Find("Shadow").gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Instantiate(particleOfDeath, transform.position, Quaternion.identity);
                //Destroy(newParticleOfDeath, newParticleOfDeath.GetComponent<ParticleSystem>().duration);
                targetCanvas.SetActive(false);
            }
            else {
                animator.SetTrigger("hit");
            }
            
            
            hpGage.GetComponent<Image>().fillAmount = hp / initHp;
        }
    }

    void ShowFloatingText(int amount) {
        floatingTextPrefab.transform.Find("PopupText").GetComponent<Text>().text = amount + "";

        Instantiate(floatingTextPrefab, targetTr.position, Quaternion.identity, targetTr);
    }

    private void MoveCtl() {
        //state에 집어넣으면 프레임에의해 자연스럽지 못해짐
        if (state == State.trace) {
            if (mopTr.position.x - playerTr.position.x < 0) {
                transform.localScale = new Vector3(-mopScaleX, mopScaleY, 0);
                targetCanvas.GetComponent<TargetCanvasCtl>().ChangeCanvsScale();
            }
            else {
                transform.localScale = new Vector3(mopScaleX, mopScaleY, 0);
                targetCanvas.GetComponent<TargetCanvasCtl>().ChangeCanvsScale();
            }

            transform.position = Vector2.MoveTowards(mopTr.position, playerTr.position, speed * Time.deltaTime);
        }
    }

    public void MopCoolTimeManger() {
        if (attackBaseCoolTime >= attackBaseDelayTime) {
            attackBaseDelayTime += Time.deltaTime;
        }
    }

    public void Attack() {
        if (attackBaseCoolTime <= attackBaseDelayTime) {
            attackBaseDelayTime = 0;
            animator.SetTrigger("attack");
            playerGO.GetComponent<PlayerState>().Hit(damageOfBase);
        }
    }

    IEnumerator CheckMonsterState() {
        while (!isDie) {
            float dist = Vector3.Distance(playerTr.position, mopTr.position);
            if(dist <= attackDist) {
                state = State.attack;
            } else if(dist <= traceDist) {
                state = State.trace;
            } else {
                state = State.idle;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator MonsterAction() {
        while (!isDie) {
            switch (state) {
                case State.idle:
                    animator.SetBool("walk", false);
                    break;
                case State.trace:
                    animator.SetBool("walk", true);
                    break;
                case State.attack:
                    animator.SetBool("walk", false);
                    Attack();
                    break;
                case State.die:
                    animator.SetTrigger("death");
                    break;
            }
            yield return new WaitForSeconds(.3f);
        }
    }





    void Update() {
        MoveCtl();
        MopCoolTimeManger();
    }
}
