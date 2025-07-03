using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int CollisionDamge = 1;
    public int HealthMax = 1;
    public int HealthCurrent;
    public GameObject[] Chunks;
    public int ChunksMin = 0;
    public int ChunksMax = 4;
    public float ExplodeDist = 0.5f;
    public float ExplosionForce = 10f;

    private void Start()
    {
        HealthCurrent = HealthMax;

        /*for (int i = 0; i < Chunks.Length; i++)
        {
            Debug.Log(Chunks[i]);
        }
        */

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
        int numChunks = Random.Range(ChunksMin, ChunksMax + 1);

        for (int i = 0; i < numChunks; i++)
        {
            CreateAsteroidChunk();
        }
        
        Destroy(gameObject);
    }

    private void CreateAsteroidChunk()
    {
        if (Chunks == null || Chunks.Length == 0)
            return;

        int randomIndex = Random.Range(0, Chunks.Length);

        //Find an random position to spawn at
        Vector2 spawnPos = transform.position;
        Vector2 newPos = spawnPos;
        spawnPos.x += Random.Range(-ExplodeDist, ExplodeDist);
        spawnPos.y += Random.Range(-ExplodeDist, ExplodeDist);

        GameObject chunk = Instantiate(Chunks[randomIndex], spawnPos, transform.rotation);

        
        Vector2 dir = (spawnPos - newPos).normalized;

        Rigidbody2D rb = chunk.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * ExplosionForce);
    }
}
