using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Scripts.Entity

{
    public class TutorialWin : MonoBehaviour
    {
        private bool triggered = false;

        private float time = 0;
        private float timer = 10;
        [SerializeField] private GameObject hiddenText;
        [SerializeField] private List<Light> lights = new List<Light> ();

        private void Update()
        {
            if (triggered)
            {
                time += Time.deltaTime;

                if (time > timer / 2)
                {
                    hiddenText.SetActive(true);
                    foreach (Light light in lights)
                    {
                        light.color = Color.red;
                    }
                }

                if (time > timer)
                {
                    SceneManager.LoadScene("GamePlayLoop"); // Change to Play scene
                }
            }

        }
        protected void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                if (collider.gameObject.TryGetComponent(out Player p))
                {
                    triggered = true;
                }
            }
        }
    }
}
