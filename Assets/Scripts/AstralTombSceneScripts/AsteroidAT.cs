using UnityEngine;

public class AsteroidAT : MonoBehaviour
{
    public int CollisionDamge = 1;
    public int HealthMax = 1;
    public int HealthCurrent;


    private void Start()
    {
        HealthCurrent = HealthMax;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        ATPlayer player = collision.gameObject.GetComponent<ATPlayer>();
        if (player)
        {
            player.TakeDamage(CollisionDamge);
        }
    }

    public void TakeDamage(int damage)
    {
        HealthCurrent = HealthCurrent - damage;

        if (HealthCurrent < 1)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}
