using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCoinScript : MonoBehaviour {

    Animator coinAnimator;

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
        //Add score
        //Remove Coin from game
        Destroy(gameObject);   
    }
}
