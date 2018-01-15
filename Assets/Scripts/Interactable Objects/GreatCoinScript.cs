using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCoinScript : MonoBehaviour {

    Animator coinAnimator;
    [SerializeField] ScorePanel mainMenu;
    [Range(0, 2)][SerializeField] int coinIndex;
    
    private void Start()
    {
        coinAnimator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            CoinCollected();
        }
    }

    private void CoinCollected()
    {
        //Update score with collected coin
        mainMenu.AddScore(coinIndex);
        //sound effect

        //Remove Coin from game
        gameObject.SetActive(false);   
    }
}
