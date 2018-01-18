using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    const int MAX_SCORE = 3;
    

    bool[] scoresCollected = new bool[3];

    [SerializeField] AudioClip score0;
    [SerializeField] AudioClip score1;
    [SerializeField] AudioClip score2;
    [SerializeField] AudioClip score3;
    bool m_soundPlayed = false;

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

            //Play sound based on score
            if (!m_soundPlayed)
            {
                Debug.Log(currentScore);
                switch (currentScore)
                {
                    case 1:
                        SoundManager.instance.PlaySound(score1);
                        m_soundPlayed = true;
                        break;
                    case 2:
                        SoundManager.instance.PlaySound(score2);
                        m_soundPlayed = true;
                        break;
                    case 3:
                        SoundManager.instance.PlaySound(score3);
                        m_soundPlayed = true;
                        break;
                    default:
                        SoundManager.instance.PlaySound(score0);
                        m_soundPlayed = true;
                        break;
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
