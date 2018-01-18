using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    const int MAX_SCORE = 3;
    

    bool[] scoresCollected = new bool[3];

    [SerializeField] Transform scorePanel;
    [SerializeField] Transform scoreEndPanel;
    [SerializeField] GameObject endPanel;

    LevelSelect levelSelect;

    List<Animator> scoreTransforms = new List<Animator>();
    List<Animator> scoreEndAnimators = new List<Animator>();

    bool activated = false;

    float timer = .5f;
    [SerializeField] float interval;
    int activeAnimator = 0;

	void Start () {
        levelSelect = GetComponent<LevelSelect>();

		for(int i = 0; i < MAX_SCORE; i++)
        {
            scoreTransforms.Add(scorePanel.GetChild(i).GetComponent<Animator>());
            scoreEndAnimators.Add(scoreEndPanel.transform.GetChild(i).GetComponent<Animator>());
        }
    }
	
	void Update () {
        if (endPanel.activeSelf && !activated)
        {
            int currentScore = 0;
            for (int i = 0; i < scoresCollected.Length; i++)
            {
                if(scoresCollected[i] == true)
                {
                    currentScore++;
                }
            }


            levelSelect.UpdateScore(currentScore + 1);

            timer += Time.deltaTime;

            if(timer > interval)
            {
                scoreEndAnimators[activeAnimator].enabled = true;
                scoreEndAnimators[activeAnimator].SetBool("collected", scoresCollected[activeAnimator]);
                activeAnimator++;
                timer = 0;
            }
            if (activeAnimator > 2)
            {
                activated = true;
            }
        }
	}

    public void AddScore(int score)
    {
        scoreTransforms[score].SetBool("collected", true);
        scoresCollected[score] = true;
    }
}
