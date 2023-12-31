using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.CoreUtils;

namespace Assets.Scripts.Entity
{

    public class KeyFinder : MonoBehaviour
    {
        // Start is called before the first frame update
        Player player;
        GameObject target;
        GameObject target2;
        GameObject root;
        bool returnCheck = false;
        void Start()
        {
            root = transform.GetChild(0).gameObject;
            root.SetActive(false);
            EventManager.GetInstance().AddListener<KeyNeededEvent>(SetTarget);
        }

        // Update is called once per frame
        void Update()
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }
            else
            {
                transform.position = player.transform.position;
                if (target != null)
                {
                    float x1 = transform.position.x;
                    float z1 = transform.position.z;
                    float x2 = target.transform.position.x;
                    float z2 = target.transform.position.z;

                    float radians = Mathf.Atan2((z2 - z1), (x2 - x1));
                    Vector3 newRotation = new (90,0 ,radians * Mathf.Rad2Deg);

                    root.transform.rotation = Quaternion.Euler(newRotation);

                    
                    if (Vector3.Distance(transform.position, target.transform.position) < 5)
                    {
                        if (returnCheck)
                        {
                            root.SetActive(false);
                        }
                        else
                        {
                            target = target2;
                            returnCheck = true;
                        }
                    }
                }

            }
        }

        public void SetTarget(KeyNeededEvent e)
        {
            target = e.key.gameObject;
            target2 =e.door.gameObject;
            root.SetActive(true);
        }
    }
}
