using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;

    private float _speed;
    private Rigidbody _rigidbody;

    public event UnityAction<Bullet> Destroying;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Mover target, float force)
    {
        SetRotation(target);
        AddForce(target, force);
        StartCoroutine(DelayedDestroy());
    }

    private void SetRotation(Mover target)
    {
        transform.LookAt(target.transform.position);
    }

    private void AddForce(Mover target, float force)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce((target.transform.position - transform.position).normalized * force, ForceMode.Force);
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);

        Destroying?.Invoke(this);

        yield break;
    }

}
