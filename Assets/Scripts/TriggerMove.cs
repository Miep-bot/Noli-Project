using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 2f;         // Speed of movement
    public float height = 1f;        // Total vertical distance

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.PingPong(Time.time * speed, height);
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
