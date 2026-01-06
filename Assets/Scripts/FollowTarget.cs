using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public PlayerCharacter target;

    [Header("Distance")]
    public float distance = 4f;

    [Header("Rotation")]
    public float mouseSensitivity = 3f;
    public float minY = -40f;
    public float maxY = 70f;

    private float _rotationX;
    private float _rotationY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!target.isInMenu){
        _rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        _rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        _rotationY = Mathf.Clamp(_rotationY, minY, maxY);
        }
    }

    void LateUpdate()
    {
        transform.position = target.transform.position;

        transform.rotation = Quaternion.Euler(_rotationY, _rotationX, 0);

        transform.position -= transform.forward * distance;
    }
}
