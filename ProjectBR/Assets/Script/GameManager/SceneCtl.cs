using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtl : MonoBehaviour {

	public void GameStartBT()
    {
        SceneManager.LoadScene("Map02");
    }

    public void GameOverBT()
    {
        SceneManager.LoadScene("Map02");
    }
}
