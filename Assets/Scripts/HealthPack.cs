using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class HealthPack : MonoBehaviour
{

    public float playerHealth = 50;
    GameObject closestHealthPack;
    public GameObject player;
    public GameObject pickUpText;
    float distance;
    float inRange = 2;
    GameObject[] healthPacks;
    float minDist;
    // Start is called before the first frame update
    void Start()
    {
        healthPacks = GameObject.FindGameObjectsWithTag("HealthPack");
    }

    // Update is called once per frame
    void Update()
    {
        healthPacks = GameObject.FindGameObjectsWithTag("HealthPack");

        GetClosestHealthPack(healthPacks);
        Debug.Log(healthPacks.Length);
        if(healthPacks.Length > 0)
        {
            Debug.Log(distance);
            if (Vector3.Distance(closestHealthPack.transform.position, player.transform.position) > inRange)
            {
                pickUpText.SetActive(false);
            }
            if (Vector3.Distance(closestHealthPack.transform.position, player.transform.position) < inRange)
            {
                pickUpText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HealPlayer(50);
                    closestHealthPack.SetActive(false);
                    pickUpText.SetActive(false);
                }
            }
        }
    }
    public void GetClosestHealthPack(GameObject[] healthPacks)
    {
        minDist = Mathf.Infinity;
        foreach(GameObject healthPack in healthPacks)
        {
            distance = Vector3.Distance(healthPack.transform.position, player.transform.position);
            if (distance < minDist)
            {
                closestHealthPack = healthPack;
                minDist = distance;
            }
        }
    }
        
    
    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pickUpText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                HealPlayer(50);
                gameObject.SetActive(false);
                pickUpText.SetActive(false);
            }
        }
        else
        {
            pickUpText.SetActive(false);
        }

    }*/

    public void HealPlayer(float healingAmount)
    {
        playerHealth += healingAmount;
    }
}
