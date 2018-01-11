using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour
{

    [SerializeField] ManagerConfig managerConfig;
    // Use this for initialization
    void Start()
    {
        managerConfig = GetComponentInParent<ManagerConfig>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            //Trigger menu
            managerConfig.GameEnd.SetActive(true);
            //shutdown controls
            managerConfig.apeSelectionController.DeselectAllApes();

        }
    }
}
