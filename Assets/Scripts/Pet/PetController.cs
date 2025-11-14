using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PetController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 3f;
    [SerializeField]
    private float _rotationSpeed = 5f;
    [SerializeField]
    private float _maxDistance = 1f;

    private Transform _player;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        LookToPlayer();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void LookToPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, _player.position);
        if(distance > _maxDistance)
        {
            _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * transform.forward);
        }
    }
}
