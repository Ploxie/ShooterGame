using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temp class. Replace with module bullets.
/// </summary>
public class Bullet : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [HideInInspector] public float Velocity;
    [HideInInspector] public Vector3 Position;
    [HideInInspector] public Vector3 Direction;

    [SerializeField] private float lifeTime;

    private void Awake()
    {
        if (lifeTime == 0) lifeTime = 5.0f;
        if (Velocity == 0) Velocity = 10.0f;
    }

    void Update()
    {
        transform.position += Direction * Velocity * Time.deltaTime;
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<UnitHealth>(out var x))
        {
            x.TakeDamage(Damage);
            Destroy(this.gameObject);
        }
    }
}
