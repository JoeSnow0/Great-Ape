using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCoinScript : MonoBehaviour {

    Animator coinAnimator;
    ScorePanel mainMenu;
    [Range(0, 2)][SerializeField] int coinIndex;

    private void Start()
    {
        coinAnimator = GetComponentInChildren<Animator>();
        if (GameObject.FindGameObjectWithTag("Menu") != null)
        {
            mainMenu = GameObject.FindGameObjectWithTag("Menu").GetComponentInChildren<ScorePanel>();
        }
        else
        {
            Debug.Log("No menu found");
        }
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
        Destroy(gameObject);   
    }
}
