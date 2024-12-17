using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BulletsShooter : MonoBehaviour
{
    [SerializeField] private float _force = 1000f;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _repeatShootTime = 0.3f;
    [SerializeField] private bool _isShooting = true;
    [SerializeField] private Mover _target;
    [SerializeField] private int _poolDefaultCapacity = 10;
    [SerializeField] private int _poolMaxSize = 100;

    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Bullet>(
            createFunc: () => CreateNewBullet(),
            actionOnGet: (bullet) => ActionOnGet(bullet),
            actionOnRelease: (bullet) => ActionOnRelease(bullet),
            actionOnDestroy: (bullet) => ActionOnDestroy(bullet),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(ShootBullets());
    }

    private Bullet CreateNewBullet()
    {
        Bullet newBullet = Instantiate(_bulletPrefab);
        newBullet.Destroying += ReleaseBullet;
        return newBullet;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.Init(_target, _force);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ReleaseBullet(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        bullet.Destroying -= ReleaseBullet;
        Destroy(bullet);
    }

    private IEnumerator ShootBullets()
    {
        var wait = new WaitForSeconds(_repeatShootTime);
        
        while (_isShooting)
        {
            _bulletPool.Get();

            yield return wait;
        }

        yield break;
    }
}