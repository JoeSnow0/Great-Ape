using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    const int MAX_SCORE = 3;
    List<Animator> scoreTransforms = new List<Animator>();

	void Start () {

		for(int i = 0; i < MAX_SCORE; i++)
        {
            scoreTransforms.Add(transform.GetChild(i).GetComponent<Animator>());
        }
	}
	
	void Update () {
		
	}
    public void AddScore(int score)
    {
        scoreTransforms[score].SetBool("collected", true);
    }
}
