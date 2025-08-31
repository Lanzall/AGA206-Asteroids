using UnityEngine;

public class PowerBeam : MonoBehaviour
{
    public int Damage = 25;

    public GameObject ExplosionPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AsteroidAT asteroid = collision.gameObject.GetComponent<AsteroidAT>();
        if (asteroid)
        {
            asteroid.TakeDamage(Damage);
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
