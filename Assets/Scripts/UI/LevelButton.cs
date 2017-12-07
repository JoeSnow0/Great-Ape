using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    [SerializeField] Text levelName;
    [SerializeField] Button button;
    [SerializeField] Image thumbnail;
    [SerializeField] Transform scoreParent;
    [SerializeField] Image[] scoreImages;
    int previous;

    public void SetScore(int score, Color color)
    {
        if(scoreImages.Length <= 0)
        scoreImages = scoreParent.GetComponentsInChildren<Image>();

        if(score > 0)
        {
            button.interactable = true;
        }
        else if(previous == 0)
        {
            button.interactable = false;
            return;
        }
        else
        {
            button.interactable = true;
            return;
        }

        if (score > 3)
        {
            score = 3;
        }

        for (int i = 0; i < score; i++)
        {
            scoreImages[i].color = color;
        }
    }

    public void Initialize(string newName, Sprite image, int previousScore, int score, Color color, string sceneName)
    {
        previous = previousScore;
        SetScore(score, color);
        levelName.text = newName;
        thumbnail.sprite = image;
        button.onClick.AddListener(delegate { LevelSelect.LoadScene(sceneName); });
    }
}
