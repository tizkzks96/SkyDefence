using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtl : MonoBehaviour {
    bool drop = true;
    public GameObject[] skillTrail;
    public GameObject[] skillEffect;

    private int damage;

    GameObject currentSkillEffect;

    private void Start() {
        SetSkillDamage();
        SetSkillEffect();
        //print(currentSkillEffect);
        //StartCoroutine(DestroyArrow());
        SkillTrail();
    }

    private void Update() {
        if (drop) {
            //DropArow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        print("AA : ");

        if (collision.gameObject.tag == "Mop") {
            collision.gameObject.GetComponent<MopCtl>().Hit(damage);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().Sleep();
            iTween.Stop();
            //transform.position = collision.transform.position;
            if (currentSkillEffect != null) SkillEffect();
            

        }
    }

    void SkillTrail() {

        switch (ArcherCtl.instance.GetSkillState()) {
            case ArcherCtl.Skill.basic:
                //Instantiate(s1Trail, transform);
                break;
            case ArcherCtl.Skill.s1Fire:
                Instantiate(skillTrail[0], transform);
                break;
            case ArcherCtl.Skill.s2:
                Instantiate(skillTrail[1], transform);
                break;
            case ArcherCtl.Skill.s3:
                Instantiate(skillTrail[2], transform);
                break;
        }
    }

    void SetSkillEffect() {
        print(ArcherCtl.instance.GetSkillState());
        switch (ArcherCtl.instance.GetSkillState()) {
            case ArcherCtl.Skill.basic:
                currentSkillEffect = null;
                break;
            case ArcherCtl.Skill.s1Fire:
                currentSkillEffect = skillEffect[0];
                break;
            case ArcherCtl.Skill.s2:
                currentSkillEffect = skillEffect[1];
                break;
            case ArcherCtl.Skill.s3:
                currentSkillEffect = skillEffect[2];
                break;
        }
    }

    void SetSkillDamage()
    {
        switch (ArcherCtl.instance.GetSkillState())
        {
            case ArcherCtl.Skill.basic:
                damage = 1;
                break;
            case ArcherCtl.Skill.s1Fire:
                damage = 3;
               break;
            case ArcherCtl.Skill.s2:
                damage = 5;
                break;
            case ArcherCtl.Skill.s3:
                damage = 7;
                break;
        }
    }

    void SkillEffect() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(currentSkillEffect, transform);
    }

    //IEnumerator DestroyArrow() {
    //    yield return new WaitForSeconds(2.0f);
    //    Destroy(gameObject);
    //}

    void SkillFireArrow() {

    }
}
