using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 1;
    public int CollisionDamage = 1;
    public GameObject ExplosionPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if(asteroid)
        {
            asteroid.TakeDamage(CollisionDamage);
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
