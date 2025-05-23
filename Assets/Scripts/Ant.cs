using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] private List<Transform> checkpoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float reachThreshold = 0.1f;
    [SerializeField] private int startChamber = 0;

    private int currentIndex = 0;
    private int direction = 1;
    private bool facingRight = true;

    void Update()
    {
        if (checkpoints == null || checkpoints.Count < 2)
            return;

        Transform target = checkpoints[currentIndex];
        Vector2 directionToTarget = target.position - transform.position;

        Vector2 newPosition = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (directionToTarget != Vector2.zero)
        {
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        if ((directionToTarget.x > 0 && !facingRight) || (directionToTarget.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        if (Vector2.Distance(transform.position, target.position) < reachThreshold)
        {
            currentIndex += direction;

            if (currentIndex >= checkpoints.Count)
            {
                currentIndex = checkpoints.Count - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }
        }
    }
}
