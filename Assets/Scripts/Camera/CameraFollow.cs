using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public ManagerConfig managerConfig;
    public Vector3 arrowOffset;
    public float cameraLerpScale = 1f;

    void FixedUpdate()
    {
        if(ApeSelectionController.activeApe != null)
        {
            if (!(transform.position == ApeSelectionController.activeApe.transform.position + arrowOffset))
            {
                transform.position = Vector3.Lerp(transform.position, ApeSelectionController.activeApe.transform.position + arrowOffset, cameraLerpScale);
            }
        }
    }
}
