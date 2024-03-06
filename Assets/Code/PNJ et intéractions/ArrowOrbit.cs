using UnityEngine;

public class ArrowOrbit : MonoBehaviour
{
    public static Transform target; // The global target, static.
    public float orbitDistance = 1.0f; // Distance from the player to the arrow.
    private Transform player;

    void Start()
    {
        player = transform.parent; // The player is the parent of this arrow.
    }

    void Update()
    {
        if (target != null && player != null)
        {
            // Calculate direction from player to target.
            Vector3 directionToTarget = (target.position - player.position).normalized;

            // Calculate the arrow rotation to point at the target.
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;

            // Position the arrow on the orbit around the player.
            Vector3 orbitPosition = player.position + (rotation * Vector3.right * orbitDistance);
            transform.position = orbitPosition;
        }
    }


    public static void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}


