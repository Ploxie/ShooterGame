using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private int Health = 25;
    // Start is called before the first frame update
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Explosion(collision);
    }
    void Explosion(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
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
