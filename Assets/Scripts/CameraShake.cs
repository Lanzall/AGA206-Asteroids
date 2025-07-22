using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = .15f;
    public IEnumerator ShakeRoutine(float ShakeDuration, float strength)
    {
        Vector2 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < ShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.localPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }


}
