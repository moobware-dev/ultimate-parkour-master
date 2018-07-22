using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public float groundHeight;
    public float secondsTilRestartIfGameOver;

    Transform player;
    bool gameOverDetected;

	// Use this for initialization
	void Start () {
        Debug.Log("It's alive!!!!");
        player = GameObject.FindWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOverDetected) {
            return;
        }
        Debug.Log(player.position.y);
        if (player.position.y <= groundHeight) {
            gameOverDetected = true;
            StartCoroutine(ResetGameAfterSeconds(secondsTilRestartIfGameOver));
        }
	}

    IEnumerator ResetGameAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(0);
    }
}
