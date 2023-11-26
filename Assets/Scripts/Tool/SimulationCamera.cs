using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationCamera : MonoBehaviour
{
    public const int MOVE_SPEED = 5;
    public const int ROTATE_AMOUNT = 1;

    public Player Player;

    public List<GameObject> enemies = new List<GameObject>();
    private Camera simCamera;
    private int enemyIndex;

    // Start is called before the first frame update
    void Start()
    {
        simCamera = GetComponent<Camera>();
        enemyIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            enemyIndex--;
            if (enemyIndex < 0)
                enemyIndex = enemies.Count - 1;
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            enemyIndex++;
            if (enemyIndex > enemies.Count - 1)
                enemyIndex = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Player.transform.position += new Vector3(1, 0, 0) * -MOVE_SPEED * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Player.transform.position += new Vector3(1, 0, 0) * MOVE_SPEED * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Player.transform.Rotate(new Vector3(0, 1, 0), -ROTATE_AMOUNT);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Player.transform.Rotate(new Vector3(0, 1, 0), ROTATE_AMOUNT);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = simCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag("Simulation Surface"))
                {
                    GameObject enemyObject = Instantiate(enemies[enemyIndex], hitInfo.point, Quaternion.AngleAxis(180, new Vector3(0, 1, 0)));
                    Enemy enemyController = enemyObject.GetComponent<Enemy>();
                    enemyController.SimulationEnabled = true;
                }
            }
        }
    }
}
