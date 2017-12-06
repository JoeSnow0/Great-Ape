using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public ManagerConfig managerConfig;
    public Vector3 arrowOffset;
    // Update is called once per frame
    void Update()
    {
        if(ApeSelectionController.activeApe != null)
        {
            if (!(transform.position == ApeSelectionController.activeApe.transform.position + arrowOffset))
            {
                transform.position = ApeSelectionController.activeApe.transform.position + arrowOffset;
            }
        }
    }
}
