using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [SerializeField]
    public GameObject ER;
    [SerializeField]
    public GameObject EM;
    [SerializeField]
    public GameObject EK;
    bool isAlive = false;
    int numberAlive = 0;
    void Update()
    {
        CreateRandomEnemy();
    }
    void CreateRandomEnemy()
    {
        int numb = Random.Range(0,2);
        numberAlive = numb;
        if (isAlive == false)
        {
            if (numb == 0)
            {
                Instantiate(ER, transform.position, Quaternion.identity);
                isAlive = true;
            }
            else if(numb == 1)
            {
                Instantiate(EM, transform.position, Quaternion.identity);
                isAlive = true;
            }
            else {
                Instantiate(EK, transform.position, Quaternion.identity);
                isAlive = true;
            }
        }

    }
    void checkIfAlive()
    {
        if (numberAlive == 0 && ER.active == false)
        {
            isAlive = false;
        }
        else if (numberAlive == 1 && EM.active == false)
        {
            isAlive = false;
        }
        else if(numberAlive == 2 && EK.active == false)
        {
            isAlive = false;
        }
    }
}
