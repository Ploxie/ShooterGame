using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject target;
    [SerializeField] private float aimInterpolation = 0.1f;

    private CinemachineFreeLook freeLookCamera;
    private CinemachineBrain cinemachineBrain;

    private float rotation = 0f;

    private Plane plane = new Plane(Vector2.down, 0f);

    private void Awake()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        freeLookCamera.Follow = target.transform;
    }

    private void Update()
    {
        if (player == null)
            SetPlayer();
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out float distance);
            Vector3 worldPosition = ray.GetPoint(distance);

            Vector3 targetPosition = player.transform.position + ((worldPosition - player.transform.position) * aimInterpolation);
            target.transform.position = targetPosition;

            if (Input.GetKeyDown(KeyCode.X))
            {
                rotation += 90 % 360;
                freeLookCamera.m_XAxis.Value = rotation;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rotation -= 90;
                freeLookCamera.m_XAxis.Value = rotation;
            }
        }
    }

    private void SetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
