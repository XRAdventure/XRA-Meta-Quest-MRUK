using UnityEngine;
using Meta.XR;
using UnityEngine.Android;
public class RaycastPlacement : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace;

    [SerializeField] private Transform rightController;
    [SerializeField] private Transform leftController;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private EnvironmentRaycastManager raycastManager;

    private const string SCENE_PERMISSION = "com.oculus.permission.USE_SCENE";

    private void Awake()
    {
        Permission.RequestUserPermission(SCENE_PERMISSION);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) 
        {
            TryToPlace(rightController);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            TryToPlace(leftController);
        }
    }

    private void TryToPlace(Transform controller) 
    {
        Ray ray = new Ray(controller.position, controller.forward);

        if (raycastManager.Raycast(ray, out EnvironmentRaycastHit hit)) 
        {
            GameObject objectToPlace = Instantiate(prefabToPlace);

            objectToPlace.transform.position = hit.point;

            Vector3 up = hit.normal.normalized;
            Vector3 forward = Vector3.ProjectOnPlane(playerCamera.position - hit.point, up);
            objectToPlace.transform.rotation = Quaternion.LookRotation(forward, up);

            objectToPlace.transform.localScale = Random.Range(0.5f, 1.0f) * Vector3.one;
        }
    }
}
