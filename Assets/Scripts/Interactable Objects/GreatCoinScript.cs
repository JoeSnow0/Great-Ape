using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCoinScript : MonoBehaviour {

    Animator coinAnimator;
    [SerializeField] ScorePanel mainMenu;
    [Range(0, 2)][SerializeField] int coinIndex;
    CircleCollider2D col;
    public AudioClip collectSFX;
    
    private void Start()
    {
        coinAnimator = GetComponentInChildren<Animator>();
        col = GetComponent<CircleCollider2D>();
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
        coinAnimator.SetBool("collected", true);
        //sound effect
        SoundManager.instance.PlayPitched(collectSFX, 0.9f, 1.1f);
        //Remove Coin from game
        col.enabled = false; 
    }
}
