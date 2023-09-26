using UnityEngine;

/// <summary>
/// Handles player movement and rotation.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float velocity;
    [SerializeField] private GameObject gun;
    [SerializeField] private int floorLayerMaskIndex;

    private int layerMask;
    private GunController gunController;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if(gunController == null) gunController = gun.GetComponent<GunController>();

        layerMask = 1 << floorLayerMaskIndex;
    }

    private void Update()
    {
        HandleRotationInput();
        HandleMovementInput();

        if (Input.GetKey(KeyCode.Mouse0)) gunController.Shoot();
        if (Input.GetKeyDown(KeyCode.R)) gunController.Reload();
    }

    /// <summary>
    /// Rotates the player
    /// </summary>
    private void HandleRotationInput()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane floorPlane = new Plane(Vector3.up, Vector3.zero);

        if (floorPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 point = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(point.x, rb.transform.position.y, point.z));
        }
    }

    /// <summary>
    /// Moves the player.
    /// </summary>
    private void HandleMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(x, 0.0f, z).normalized;

        rb.transform.Translate(moveDirection * Time.deltaTime * velocity, Space.World);
    }
}
