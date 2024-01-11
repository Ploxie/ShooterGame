using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationCamera : MonoBehaviour
{
    public const int MOVE_SPEED = 5;
    public const int ROTATE_AMOUNT = 1;

    public Player Player;
    public GameObject StartPole;
    public GameObject EndPole;
    public GameObject WallPrefab;

    public SimulationHerald Herald;

    public LayerMask RayWhitelist;

    public List<GameObject> enemies = new List<GameObject>();
    private Camera simCamera;
    private int enemyIndex;
    private GameObject wallInstance;

    private bool creatingWall;

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

        if (Input.GetKey(KeyCode.S))
        {
            Player.transform.position += new Vector3(0, 0, 1) * -MOVE_SPEED * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Player.transform.position += new Vector3(0, 0, 1) * MOVE_SPEED * Time.deltaTime;
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
                    try
                    {
                        GameObject enemyObject = Instantiate(enemies[enemyIndex], hitInfo.point, Quaternion.AngleAxis(180, new Vector3(0, 1, 0)));
                        Enemy enemyController = enemyObject.GetComponent<Enemy>();
                        enemyController.SimulationEnabled = true;
                        enemyController.Herald = Herald;
                        Herald.RegisterEnemy((SimEnemy)enemyIndex, enemyIndex, hitInfo.point);
                    }
                    catch (Exception) { }
                }
            }
        }

        if (creatingWall)
        {
            StartPole.transform.LookAt(EndPole.transform);
            EndPole.transform.LookAt(StartPole.transform);
            float distance = Vector3.Distance(StartPole.transform.position, EndPole.transform.position);
            wallInstance.transform.position = StartPole.transform.position + distance / 2 * StartPole.transform.forward;
            wallInstance.transform.rotation = StartPole.transform.rotation;
            wallInstance.transform.localScale = new Vector3(
                wallInstance.transform.localScale.x,
                wallInstance.transform.localScale.y,
                distance);

            Ray ray = simCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, RayWhitelist))
            {
                if (hitInfo.collider.gameObject.CompareTag("Simulation Surface"))
                {
                    EndPole.transform.position = hitInfo.point + new Vector3(0, EndPole.transform.localScale.y / 2, 0);
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = simCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, RayWhitelist))
            {
                if (hitInfo.collider.gameObject.CompareTag("Simulation Surface"))
                {
                    StartPole.transform.position = hitInfo.point + new Vector3(0, StartPole.transform.localScale.y / 2, 0);
                    wallInstance = Instantiate(WallPrefab, StartPole.transform.position + 
                        new Vector3(0, WallPrefab.transform.localScale.y / 2, 0), Quaternion.identity);
                    creatingWall = true;
                }
            }
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Ray ray = simCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, RayWhitelist))
            {
                if (hitInfo.collider.gameObject.CompareTag("Simulation Surface"))
                {
                    EndPole.transform.position = hitInfo.point + new Vector3(0, EndPole.transform.localScale.y / 2, 0);
                    creatingWall = false;
                }
            }
        }
    }
}
