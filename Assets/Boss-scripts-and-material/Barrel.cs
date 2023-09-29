using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private int Health = 25;
    // Start is called before the first frame update
    // Update is called once per frame
    movePlayer player;
    private void Start()
    {
        player = FindObjectOfType<movePlayer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("coll");
        Explosion(collision);
    }
    void Explosion(Collision collision)
    {
        if (collision.gameObject.tag == player.gameObject.tag)
        {
            if (Health <= 0)
            {
                gameObject.active = false;
            }
            else
            {
                LooseHealth();
            }
        }
    }
    void LooseHealth()
    {
        Health -= 10;
    }
}
