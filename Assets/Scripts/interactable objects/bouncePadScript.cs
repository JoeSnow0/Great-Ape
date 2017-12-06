using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerConfig))]
public class bouncePadScript : MonoBehaviour {

    // Use this for initialization
    public PlayerConfig playerConfig;
    public float bouncePower = 600;
    
	void Start () {
        bouncePower= bouncePower * 0.11f;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {

            //  player = other.gameObject;
            other.GetComponent<PlayerConfig>().velocity.y =
-1 * other.GetComponent<PlayerConfig>().velocity.y;//bouncePower;
           
        }
    }
}
