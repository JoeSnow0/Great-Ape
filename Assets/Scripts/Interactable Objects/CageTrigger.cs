using UnityEngine;
using System.Collections;

public class CageTrigger : MonoBehaviour
{
    [SerializeField] private ManagerConfig m_managerConfig;
    [Header("These should always be the same type of ape")]
    [SerializeField] private GameObject m_apeModel;
    [SerializeField] private Player m_apeToSpawn;
    [Header("SpawnPoint")]
    [SerializeField] private Transform m_modelTransform;
    private BoxCollider2D m_collider2d;

    
    void Start()
    {
        m_managerConfig = FindObjectOfType<ManagerConfig>();
        m_collider2d = GetComponent<BoxCollider2D>();
        //add apeModel to cage
        Instantiate(m_apeModel, m_modelTransform);
    }

    void TriggerCage()
    {
        //Spawn Ape
        m_managerConfig.apeSelectionController.AddApe(m_apeToSpawn, m_modelTransform.position, m_modelTransform.rotation);
        m_modelTransform.gameObject.SetActive(false);
        //Disable cage
        m_collider2d.enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == ApeSelectionController.activeApe.gameObject)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.F))
            {
                TriggerCage();
            }
        }
    }
}
