using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private bool m_isActive = false;

    Player player;

    Animator anim;

    void Start()
    {
        player = GetComponent<Player>();
        anim = player.apeAnim;
    }

    void Update()
    {
        if (GetApeState())
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
            {
                player.OnJumpInputDown();
                anim.SetTrigger("Jump");
            }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonDown("Jump"))
            {
                player.OnJumpInputUp();
            }
        }
        else
        {
            //Stop input if player has selected another ape
            player.SetDirectionalInput(new Vector2(0,0));
        }
    }

    public void SetApeState(bool state)
    {
        m_isActive = state;
    }
    public bool GetApeState()
    {
        return m_isActive;
    }
}
