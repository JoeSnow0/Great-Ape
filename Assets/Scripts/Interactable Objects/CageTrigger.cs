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

    
    void Start()
    {
        managerConfig = FindObjectOfType<ManagerConfig>();
        //add apeModel to cage
        Instantiate(apeModel, modelTransform);
    }

    void DestroyCage()
    {
        //Spawn Ape
        managerConfig.apeSelectionController.AddApe(apeToSpawn, modelTransform.position, modelTransform.rotation);
        //Destroy cage
        Destroy(gameObject);

    }
}
