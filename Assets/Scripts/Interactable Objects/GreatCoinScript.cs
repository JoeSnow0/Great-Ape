using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCoinScript : MonoBehaviour {

    Animator coinAnimator;

    private void Start()
    {
        coinAnimator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CoinCollected();
        }
    }

    private void CoinCollected()
    {
        //Add score
        //Remove Coin from game
        Destroy(this);   
    }
}
