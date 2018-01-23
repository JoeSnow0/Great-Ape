using UnityEngine;
using System.Collections;

public class CageTrigger : MonoBehaviour
{
    [SerializeField] private ManagerConfig m_managerConfig;
    [Header("These should always be the same type of ape")]
    [SerializeField]
    private GameObject m_apeModel;
    [SerializeField] private Player m_apeToSpawn;
    [Header("SpawnPoint")]
    [SerializeField]
    private Transform m_modelTransform;
    private BoxCollider2D m_collider2d;

    public AudioClip cageOpenSFX;

    bool m_canUse;

    GameObject m_currentApe;

    void Start()
    {
        m_managerConfig = FindObjectOfType<ManagerConfig>();
        m_collider2d = GetComponent<BoxCollider2D>();
        //add apeModel to cage
        Instantiate(m_apeModel, m_modelTransform);
    }

    private void Update()
    {
        if (m_currentApe != null)
        {
            m_canUse = (m_currentApe == ApeSelectionController.activeApe.gameObject);
        }

        if (!m_canUse)
            return;


        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.E))
        {
            TriggerCage();
        }
    }

    void TriggerCage()
    {
        //Spawn Ape
        m_managerConfig.apeSelectionController.AddApe(m_apeToSpawn, m_modelTransform.position, m_modelTransform.rotation);
        m_modelTransform.gameObject.SetActive(false);
        //play sound
        SoundManager.instance.PlayPitched(cageOpenSFX, 0.3f, 0.4f,0.2f);
        //Disable cage
        m_collider2d.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == ApeSelectionController.activeApe.gameObject)
        {
            m_currentApe = collision.gameObject;
            m_canUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == m_currentApe)
        {
            m_currentApe = null;
            m_canUse = false;
        }
    }
}
