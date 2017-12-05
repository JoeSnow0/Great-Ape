using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerConfig))]
public class bouncePadScript : MonoBehaviour {

    // Use this for initialization
    GameObject player;
    public PlayerConfig playerConfig;
    public float bouncePower;
    
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            playerConfig = player.GetComponent<PlayerConfig>();
            playerConfig.velocity.y= bouncePower;
        }
    }
}
