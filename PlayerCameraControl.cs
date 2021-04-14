using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    #region Fields
    Camera mainCamera;
    [SerializeField]
    GameObject cameraTarget;
    [SerializeField]
    float cameraMoveSpeed;
    #endregion

    #region Properties
    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            return mainCamera;
        }
    }

    public GameObject CameraTarget
    {
        get
        {
            if (cameraTarget == null)
            {
                cameraTarget = transform.GetChild(0).gameObject;
            }
            return cameraTarget;
        }
    }

    private Vector3 CameraTargetPosition => CameraTarget.transform.position;
    private Quaternion CameraTargetRotation => CameraTarget.transform.rotation;
    #endregion

    #region Methods
    public void MoveMainCamera()
    {
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, CameraTargetPosition, cameraMoveSpeed * Time.deltaTime);
        MainCamera.transform.rotation = CameraTargetRotation;
    }
    #endregion

    private void FixedUpdate()
    {
        MoveMainCamera();
    }
}