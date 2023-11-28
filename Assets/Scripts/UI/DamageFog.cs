using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFog : MonoBehaviour
{
    private Image overlay;
    [SerializeField] private bool running = false;
    [SerializeField] private float speed = 1;
    [SerializeField] private float intensity = 1;
    private float timer;
    [SerializeField] private AnimationCurve heartbeat;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private AnimationCurve intensityCurve;

    private void Awake()
    {
        overlay = GetComponentInChildren<Image>();
        overlay.enabled = false;
    }
    private void Start()
    {
        EventManager.GetInstance().AddListener<PlayerHealthChangeEvent>(UpdateValues);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!running)
        {
            timer = 0;
            return;
        }
        timer += Time.deltaTime * speed;
        if (timer > 1)
            timer = 0;
        Color temp = overlay.color;
        temp.a = heartbeat.Evaluate(timer) * intensity;
        overlay.color = temp;
    }


    private void UpdateValues(PlayerHealthChangeEvent e)
    {
        Health health = e.Health;
        if (!overlay.enabled)
        {
            if (health.CurrentHealth < health.MaxHealth / 1.5)
            {
                running = true;
                overlay.enabled = true;
            }
        }
        else
        {
            if (health.CurrentHealth < health.MaxHealth / 1.5)
            {
                //Debug.Log(((health.MaxHealth / 1.5f) - health.CurrentHealth) / (health.MaxHealth / 1.5f));
                speed = speedCurve.Evaluate(((health.MaxHealth / 1.5f) - health.CurrentHealth) / 20);
                intensity = intensityCurve.Evaluate(((health.MaxHealth / 1.5f) - health.CurrentHealth) / 20);

            }
            else
            {
                running = false;
                overlay.enabled = false;
            }
        }
    }
}
