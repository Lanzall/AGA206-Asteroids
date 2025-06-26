using UnityEngine;

public class Asteroid : MonoBehaviour
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
        Spaceship ship = collision.gameObject.GetComponent<Spaceship>();
        if(ship != null)
        {
            ship.TakeDamage(CollisionDamge);
        }
    }

    public void TakeDamage(int damage)
    {
        //Reduce the current health
        HealthCurrent = HealthCurrent - damage;

        //HealthCurrent -= damage;  another way of writing the above

        //If current health is zero, then Explode
        if (HealthCurrent < 1)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Debug.Log("Asteroid Destroyed");
        Destroy(gameObject);
    }
}
