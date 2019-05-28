using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCtl : MonoBehaviour {
    public static ArcherCtl instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    Vector3 mousePos;
    Vector3 target;

    public float speedDuringShootArrow;


    Animator animator;

    public GameObject arrow;
    public float attackArrowCoolTime;
    public float attackArrowDelayTime;
    public float shootArrowDelay;
    public float arrowRange;
    PlayerState ps;
    public GameObject particle;


    public enum Skill
    {
        basic,
        s1Fire,
        s2,
        s3
    }
    public Skill skill = Skill.basic;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        ps = gameObject.GetComponent<PlayerState>();
        
	}

    public Skill GetSkillState() {
        return skill;
    }

    public void SetSkillState(Skill state) {
        skill = state;
    }

    public void ArcherCoolTimeManger() {
        if (attackArrowCoolTime >= attackArrowDelayTime) {
            attackArrowDelayTime += Time.deltaTime;
        }
    }

    public void Attack() {
        if (attackArrowCoolTime <= attackArrowDelayTime) {
            attackArrowDelayTime = 0;
            ps.SetCurrentSpeed(speedDuringShootArrow); //화살을 쏘는 도중 속도 감소
            animator.SetTrigger("attack"); //

            StartCoroutine("ShootArrow");
        }
    }

    Vector2 CheckShootDirection(float tX, float tY, float initArrowGap) {
        float directionX;
        float directionY;

        if (tX > 0) {
            directionX = initArrowGap;
            //ps.CheckPlayerDirectionOnceAttack();
        }
        else {
            directionX = -initArrowGap;
            //ps.CheckPlayerDirectionOnceAttack();
        }
        if (tY > 0) directionY = initArrowGap; else directionY = -initArrowGap;
        return new Vector2(directionX, directionY);
    }

    void SetSkill(CanvasCtl.SkillBtnState clickedBtn) {
        //몇번째 버튼이 눌렸는지 체크
        //CanvasCtl.instance.GetClickedSkillBtn()
        print("SetSkill" + clickedBtn);
        switch (clickedBtn) {
            case CanvasCtl.SkillBtnState.none:
                SetSkillState(Skill.basic);
                break;
            case CanvasCtl.SkillBtnState.btn1:
                SetSkillState(Skill.s1Fire);
                print(skill);
                break;
            case CanvasCtl.SkillBtnState.btn2:
                SetSkillState(Skill.s2);
                break;
            case CanvasCtl.SkillBtnState.btn3:
                SetSkillState(Skill.s3);
                break;
        }
        //스킬에 맞는거 출력
        switch (skill) {
            case Skill.basic:

                break;
            case Skill.s1Fire:
                break;
            case Skill.s2:
                break;
            case Skill.s3:
                break;
        }
    }



    IEnumerator ShootArrow() {
        float initArrowGap = 1f;
        target = transform.Find("Direction").gameObject.transform.up;
        Vector2 Shootdirection = CheckShootDirection(target.x, target.y, initArrowGap);

        yield return new WaitForSeconds(shootArrowDelay);

        ps.SetCurrentSpeed(ps.GetSpeed()); //속도 화살을 쏘고난 후 원래 속도로 변환
        float angle = -1 * Mathf.Rad2Deg * Mathf.Atan2(target.x, target.y) + 90; //화살이 나아가는 각도
        Vector3 arrowInitPostion = new Vector3(gameObject.transform.position.x + Shootdirection.x, gameObject.transform.position.y + Shootdirection.y + 0.4f, 0); //화살위치 초기화
        SetSkill(CanvasCtl.instance.GetClickedSkillBtn()); // 화살선택
        GameObject newArrow = Instantiate(arrow, arrowInitPostion, Quaternion.identity); //화살 생성
        newArrow.transform.Rotate(new Vector3(0, 0, angle + 180)); //화살 각도 변환
        CanvasCtl.instance.InitSkillBtnState(); // 다음 화살을 기본화살로 초기화

        //print( * arrowRange);
        //iTween.MoveTo(newArrow, new Vector3(mousePos.x, mousePos.y, 0), 0.3f); //화살 발사 객체 터치시 작동
        iTween.MoveTo(newArrow, transform.position + target * arrowRange, 0.3f); //화살 발사 바라보는 방향 //아처 객체 아래의 디렉션이 바라보는 방향
        
        yield return new WaitForSeconds(2.0f);
        Destroy(newArrow);
        
    }

    public void AttackBtn() {
        Attack();
    }

    // Update is called once per frame
    void Update() {
        ArcherCoolTimeManger();

        //// || Input.GetTouch(0).phase == TouchPhase.Ended
        //if (Input.GetMouseButtonDown(0)) {
        //    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Camera.main.transform.forward);

        //    //if (hit.transform.gameObject.tag == "Mop") {
        //        Attack();
        //    //}
        //}
    }


}
