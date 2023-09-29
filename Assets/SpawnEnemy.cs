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
    List<GameObject> fb;
    private void Awake()
    {
        fb = new List<GameObject>();
        CreateRandomEnemy();
        //GameObject gb = Instantiate(ER, transform.position, Quaternion.identity);
    }
    void Update()
    {
        
    }    
    void CreateRandomEnemy()
    {
        int numb = 0;
        numberAlive = numb;
        GameObject gb = Instantiate(ER, transform.position, Quaternion.identity);
        //gb.SetActive(true);
        //if (isAlive == false)
        //{
        //    if (numb == 0)
        //    {
        //        GameObject gb = Instantiate(ER, transform.position, Quaternion.identity);
        //        gb.active = true;
        //        fb.Add(gb);
        //        isAlive = true;
        //    }
        //    else if (numb == 1)
        //    {
        //        GameObject gb = Instantiate(EM, transform.position, Quaternion.identity);
        //        gb.active = true;
        //        isAlive = true;
        //        fb.Add(gb);
        //    }
        //    else
        //    {
        //        GameObject gb = Instantiate(EK, transform.position, Quaternion.identity);
        //        gb.active = true;
        //        isAlive = true;
        //        fb.Add(gb);
        //    }
        //}
        //checkIfAlive();

    }
    void checkIfAlive()
    {
        for (int i = 0; i < fb.Count; i++)
        {
            if (numberAlive == 0 && ER.active == false)
            {
                Destroy(fb[i].gameObject);
                fb.RemoveAt(i);
                isAlive = false;
            }
            if (numberAlive == 1 && EM.active == false)
            {
                Destroy(fb[i].gameObject);
                fb.RemoveAt(i);
                isAlive = false;
            }
            if (numberAlive == 2 && EK.active == false)
            {
                Destroy(fb[i].gameObject);
                fb.RemoveAt(i);
                isAlive = false;
            }
        }
        
    }
}
