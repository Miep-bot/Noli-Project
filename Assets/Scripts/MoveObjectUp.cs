using UnityEngine;

public class MoveObjectUp : MonoBehaviour
{
    public float riseSpeed = 1f;      // Units per second
    public float riseDuration = 3f;   // Duration in seconds

    private float timeElapsed = 0f;
    private bool isRising = false;

    public void StartRising()
    {
        isRising = true;
        timeElapsed = 0f;
    }

    private void Update()
    {
        if (isRising)
        {
            if (timeElapsed < riseDuration)
            {
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
                timeElapsed += Time.deltaTime;
            }
            else
            {
                isRising = false;
            }
        }
    }
}
