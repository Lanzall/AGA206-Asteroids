using System.Security.Cryptography;
using UnityEngine;

public class BlockLerper : MonoBehaviour
{

    public Transform LerpBlock;
    public float LerpSpeed = 0.1f;

    private Vector3 newPos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            LerpPosition(new Vector3(10, 0, 0));

        if (Input.GetKeyDown(KeyCode.A))
            LerpPosition(new Vector3(-10, 0, 0));

        if (Input.GetKeyDown(KeyCode.W))
            LerpPosition(new Vector3(0, 0, 10));

        if (Input.GetKeyDown(KeyCode.S))
            LerpPosition(new Vector3(0, 0, -10));

        LerpBlock.position = Vector3.Lerp(LerpBlock.position, newPos, LerpSpeed * Time.deltaTime);
    }

    private void LerpPosition(Vector3 _newPos)
    {
        newPos += _newPos;
    }
}
