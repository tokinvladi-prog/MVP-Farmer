using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public InputActionReference HotbarSelectionAction;

    [SerializeField]
    private Item[] _items = new Item[9];
    private int _selectedIndex = 0;
    private GameObject _currentItem;

    [Header("References")]
    [SerializeField]
    private Transform _handTransform;

    private void Start()
    {
        UpdateSelectedItem();
    }

    private void OnEnable()
    {
        HotbarSelectionAction.action.Enable();
        HotbarSelectionAction.action.performed += OnHotbarSelected;
    }

    private void OnDisable()
    {
        HotbarSelectionAction.action.Disable();
        HotbarSelectionAction.action.performed -= OnHotbarSelected;
    }

    private void OnHotbarSelected(InputAction.CallbackContext context)
    {
        int keyIndex = (int)context.ReadValue<float>() - 1;
        SelectTool(keyIndex);
    }

    private void SelectTool(int index)
    {
        _selectedIndex = index;
        UpdateSelectedItem();
    }

    private void UpdateSelectedItem()
    {
        if(_currentItem != null)
        {
            Destroy(_currentItem);
        }

        Item item = _items[_selectedIndex];
        if (item != null && item.Prefab != null)
        {
            _currentItem = Instantiate(item.Prefab, _handTransform);
        }
    }

    public Item GetSelectedItem() => _items[_selectedIndex];
}
