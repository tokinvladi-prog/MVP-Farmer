using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControll : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction _moveAction;
    private InputAction _interactAction;

    private Vector2 _moveAmt;
    private Vector2 _mouseAmt;
    private Rigidbody _rb;
    private Inventory _inventory;

    [SerializeField]
    private float _moveSpeed = 4f;
    [SerializeField]
    private float _rotationSpeed = 4f;

    private Camera _mainCamera;

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
        _inventory = GetComponent<Inventory>();
        _mainCamera = Camera.main;

        _moveAction = InputSystem.actions.FindAction("Move");
        _interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        Vector3 worldPosition = GetMousePosition();
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _mouseAmt = Mouse.current.position.ReadValue();

        Rotate(worldPosition);

        if (_interactAction.WasPressedThisFrame())
        {
            FarmTile tile = GridManager.Instance.GetTile(worldPosition);
            UseSelectedItem(tile);
        }
    }

    private void UseSelectedItem(FarmTile tile)
    {
        Item currentItem = _inventory.GetSelectedItem();

        if (currentItem == null) return;

        switch (currentItem.Type)
        {
            case ItemType.Tool:
                UseTool(tile, currentItem);
                break;
        }
    }

    private void UseTool(FarmTile tile, Item tool)
    {
        if (tile != null)
        {
            if (tool.Name == "Hoe")
            {
                tile.Plow();
            }
        }
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

    private Vector3 GetMousePosition()
    {
        Vector3 worldPosition = new();

        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _mainCamera.ScreenPointToRay(_mouseAmt);

        if (groundPlane.Raycast(ray, out float position))
        {
            worldPosition = ray.GetPoint(position);
        }

        return worldPosition;
    }

    private void Rotate(Vector3 worldPosition)
    {
        Vector3 lookDirection = worldPosition - transform.position;
        lookDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}