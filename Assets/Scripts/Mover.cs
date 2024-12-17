using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Waypoint[] _waypoints;

    private int _currentWaypointIndex;

    private void Update()
    {
        Move();

        if (transform.position == _waypoints[_currentWaypointIndex].transform.position)
        {
            _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Length;
            RotateToCurrentWaypoint();
        }
    }

    private void RotateToCurrentWaypoint()
    {
        transform.LookAt(_waypoints[_currentWaypointIndex].transform.position);
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].transform.position, _speed * Time.deltaTime);
    }
}