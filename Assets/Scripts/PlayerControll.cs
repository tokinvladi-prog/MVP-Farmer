using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControll : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction _moveAction;

    private Vector2 _moveAmt;
    private Vector2 _mouseAmt;
    private Rigidbody _rb;

    [SerializeField]
    private float _moveSpeed = 4f;
    [SerializeField]
    private float _rotationSpeed = 4f;
    [SerializeField]
    private LayerMask _groundLayer;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _mouseAmt = Mouse.current.position.ReadValue();

        Rotate();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new(_moveAmt.x, 0, _moveAmt.y);
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * movement);
    }

    private void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mouseAmt);

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, _groundLayer))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
