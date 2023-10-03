using Cinemachine;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject gun;
    [SerializeField] private int floorLayerMaskIndex;

    private int layerMask;
    private GunController gunController;

    private Player player;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if(gunController == null) gunController = gun.GetComponent<GunController>();

        player = GetComponent<Player>();

        layerMask = 1 << floorLayerMaskIndex;

        var camera = FindObjectOfType<CinemachineVirtualCamera>();
        camera.Follow = transform;
        camera.LookAt = transform;
    }

    private void Update()
    {
        HandleRotationInput();
        HandleMovementInput();

        if (Input.GetKey(KeyCode.Mouse0)) gunController.Shoot();
        //if (Input.GetKeyDown(KeyCode.R)) gunController.Reload();
    }

    private void HandleRotationInput()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane floorPlane = new Plane(Vector3.up, transform.position);

        if (floorPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 point = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(point.x, rb.transform.position.y, point.z));
        }
    }

    private void HandleMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(x, 0.0f, z).normalized;

        
        //rb.transform.Translate(moveDirection * Time.deltaTime * player.MovementSpeed, Space.World);
        rb.velocity = moveDirection * player.MovementSpeed;
    }
}
