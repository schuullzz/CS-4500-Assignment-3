using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    //Variables used to hold mouse input and overall operation.
    [SerializeField]
    Transform character;
    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    public float sensitivity = 1;
    public float smoothing = 2;

    //Resets when character is spawned.
    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    //Function is called when program starts to gain control of mouse.
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Updates mouse movements.
    void Update()
    {
        // Get smooth mouse look.
        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
        appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
        currentMouseLook += appliedMouseDelta;
        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

        // Rotate camera and controller.
        transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
    }
}
