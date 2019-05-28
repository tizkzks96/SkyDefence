using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtl : MonoBehaviour {
    public static CanvasCtl instance;

    public enum SkillBtnState
    {
        none,
        btn1,
        btn2,
        btn3
    }

    public SkillBtnState skillBtnState = SkillBtnState.none;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void ClickAttack() {
        PlayerState.instance.Attack();
    }

    public void ClickSkillBtn1() {
        skillBtnState = SkillBtnState.btn1;
    }
    public void ClickSkillBtn2() {
        skillBtnState = SkillBtnState.btn2;
    }
    public void ClickSkillBtn3() {
        skillBtnState = SkillBtnState.btn3;
    }

    public SkillBtnState GetClickedSkillBtn() {
        return skillBtnState;
    }

    public void InitSkillBtnState() {
        skillBtnState = SkillBtnState.none;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
