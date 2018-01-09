using UnityEngine;
using System.Collections;

public class CageTrigger : MonoBehaviour
{
    [SerializeField] ManagerConfig managerConfig;
    [Header("These should always be the same type of ape")]
    [SerializeField] GameObject apeModel;
    [SerializeField] Player apeToSpawn;
    [Header("SpawnPoint")]
    [SerializeField] Transform modelTransform;
    private BoxCollider2D collider2d;

    
    void Start()
    {
        managerConfig = FindObjectOfType<ManagerConfig>();
        collider2d = GetComponent<BoxCollider2D>();
        //add apeModel to cage
        Instantiate(apeModel, modelTransform);
    }

    void DestroyCage()
    {
        //Spawn Ape
        managerConfig.apeSelectionController.AddApe(apeToSpawn, modelTransform.position, modelTransform.rotation);
        modelTransform.gameObject.SetActive(false);
        //Disable cage
        collider2d.enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == ApeSelectionController.activeApe.gameObject)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.F))
            {
                DestroyCage();
            }
        }
    }
}
