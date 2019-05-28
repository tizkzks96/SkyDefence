using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtl : MonoBehaviour {
    public float createMopTime = 3.0f;
    public float createItemTime = 2.0f;
    public GameObject[] mops;
    public GameObject[] items;
    public Transform[] mopPoints;
    public Transform[] itemPoints;
    public bool isGameOver = false;
    public GameObject gameover;

    public enum Round
    {
        r01,
        r02,
        r03,
        r04,
        r05,
        r06,
        r07,
        r08,
        r09,
        r10
    }

    public Round round = Round.r01;

	// Use this for initialization
	void Start () {
        mopPoints = GameObject.Find("MopSpawnPoints").GetComponentsInChildren<Transform>();
        itemPoints = GameObject.Find("ItemSpawnPoints").GetComponentsInChildren<Transform>();
        StartCoroutine(CreateMonster(1));
        StartCoroutine(CreateItem());
    }
	
	// Update is called once per frame
	void Update () {
        GameOver();
    }

    IEnumerator CreateMonster(int mop1) {
        while (!isGameOver) {
            yield return new WaitForSeconds(createMopTime);
            
            int idx = Random.Range(1, mopPoints.Length);
            int mopsIdx = Random.Range(1, mops.Length);
            GameObject _monster = Instantiate(mops[mopsIdx], GameObject.Find("Mops").transform);
            _monster.transform.position = mopPoints[idx].position;
        }
    }

    IEnumerator CreateItem() {
        while (!isGameOver) {
            yield return new WaitForSeconds(createItemTime);

            int idx = Random.Range(0, itemPoints.Length);
            int itemsIdx = Random.Range(0, items.Length);
            GameObject _item = Instantiate(items[itemsIdx], GameObject.Find("Items").transform);
            _item.transform.position = itemPoints[idx].position;
        }
    }

    public void RoundCtl() {
        switch (round) {
            case Round.r01:

                break;
        }
    }

    public void GameOver()
    {
        if (PlayerState.instance.GetState())
        {
            gameover.SetActive(true);
        }
    }
}
