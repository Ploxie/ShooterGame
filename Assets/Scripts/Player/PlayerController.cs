using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject gun;
    [SerializeField] private int floorLayerMaskIndex;

    public Vector3 AimPosition;
    public Vector3 LastAimPosition;

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
        //camera.Follow = transform;
        //camera.LookAt = transform;
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
            var rayHit = cameraRay.GetPoint(rayLength);

            LastAimPosition = AimPosition;
            var aimPos = new Vector3(rayHit.x, rb.transform.position.y, rayHit.z);
            var aimDir = ((aimPos - transform.position) * 100.0f).normalized;
            aimDir.y = 0.0f;
            AimPosition = Vector3.Lerp(LastAimPosition, aimPos, Time.deltaTime * 20.0f);

            //AimPosition = aimPos;
            //transform.LookAt(rb.transform.position + (aimDir * 50.0f));
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20.0f);
            
        }

    }

    private void HandleMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = Quaternion.Euler(0.0f, Camera.main.transform.localEulerAngles.y, 0.0f) * new Vector3(x, 0.0f, z).normalized;

        
        //rb.transform.Translate(moveDirection * Time.deltaTime * player.MovementSpeed, Space.World);
        rb.velocity = moveDirection * player.MovementSpeed;
    }
}
