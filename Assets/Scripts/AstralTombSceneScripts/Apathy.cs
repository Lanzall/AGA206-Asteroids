using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Apathy : MonoBehaviour
{
    public AudioClip[] audioClip;
    private AudioSource audioSource;
    public Animator Animator;

    public Transform[] asteroidSpawnPoints;

    public GameObject[] asteroidPrefabs;
    public Transform playerTransform;
    public float asteroidSpeed = 10f;
    public float timeBetweenAttacks = 2f;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Animator = GetComponent<Animator>();
    }


    public void PlayRoarSound()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[0];

        audioSource.Play();
    }

    public void PlayCrackSound()
    {
        if (audioClip == null || audioClip.Length == 0)
            return;

        audioSource.clip = audioClip[1];

        audioSource.Play();
    }


    public void CombatBegin()
    {
        StartCoroutine(AttackSelector());
    }


    IEnumerator AttackSelector()
    {
        StartCoroutine(SpawnAndFireAsteroid());
        yield return null;
    }

    IEnumerator SpawnAndFireAsteroid()
    {
        foreach (Transform spawnPoint in asteroidSpawnPoints)
        {
            // Pick a random asteroid prefab
            GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            // Spawn the asteroid
            GameObject asteroid = Instantiate(selectedAsteroid, spawnPoint.position, Quaternion.identity);

            // Wait for 1 second before firing
            yield return new WaitForSeconds(1f);

            // Fire toward player's current position
            Vector3 direction = (playerTransform.position - asteroid.transform.position).normalized;

            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * asteroidSpeed;
            }
        }

        // Wait before next attack cycle
        yield return new WaitForSeconds(timeBetweenAttacks);
        StartCoroutine(AttackSelector());
    }

}
