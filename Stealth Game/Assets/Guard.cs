using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {
    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 5;
    public float waitTime = 0.3f;
    public float turnSpeed = 90f;
    public float timeToSpotPlayer = 0.5f;

    public Light spotlight;
    public float viewDistance;
    float viewAngle;
    public LayerMask viewMask;
    Color originalSpotlightColor;
    float suspicionLevel;

    public Transform path;
    Transform player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotlightColor = spotlight.color;
        viewAngle = spotlight.spotAngle;

        Vector3[] waypoints = new Vector3[path.childCount];
        for (int i = 0; i < waypoints.Length; ++i) {
            waypoints[i] = path.GetChild(i).position;
            waypoints[i].y = transform.position.y;
        }
        StartCoroutine(Patrol(waypoints));
    }

    void Update() {
        if (CanSeePlayer()) {
            suspicionLevel = Mathf.Clamp01(suspicionLevel + Time.deltaTime / timeToSpotPlayer);
            spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, suspicionLevel);
            if (suspicionLevel == 1) {
                OnGuardHasSpottedPlayer?.Invoke();
            }
        } else {
            if (suspicionLevel != 0) {
                suspicionLevel = Mathf.Clamp01(suspicionLevel - Time.deltaTime / timeToSpotPlayer);
                spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, suspicionLevel);
            }
        }
    }

    bool CanSeePlayer() {
        if (Vector3.Distance(transform.position, player.position) < viewDistance) {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
                if (!Physics.Linecast(transform.position, player.position, viewMask)) {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator Patrol(Vector3[] waypoints) {
        transform.position = waypoints[0];
        transform.LookAt(waypoints[1 % waypoints.Length]);
        while (true) {
            for (int i = 0; i < waypoints.Length; ++i) {
                while (transform.position != waypoints[i]) {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[i], speed * Time.deltaTime);
                    yield return null;
                }

                yield return new WaitForSeconds(waitTime);

                Vector3 direction = (waypoints[(i + 1) % waypoints.Length] - transform.position).normalized;
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, angle)) > 0.05f) {
                    transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle, turnSpeed * Time.deltaTime);
                    yield return null;
                }
                transform.eulerAngles = Vector3.up * angle;
            }
        }
    }

    void OnDrawGizmos() {
        Vector3 startPosition = path.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in path) {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}