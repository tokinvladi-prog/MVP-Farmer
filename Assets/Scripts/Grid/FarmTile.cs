using System.Collections;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    [SerializeField]
    private GameObject _plowed;
    [SerializeField]
    private GameObject _planted;
    [SerializeField]
    private GameObject _ready;

    [SerializeField]
    private float _timeGrow = 15f;

    private enum TileState { Empty, Plowed, Planted, Ready }

    private TileState _currentState = TileState.Empty;

    public void Interact()
    {
        switch (_currentState)
        {
            case TileState.Empty:
                Plow();
                break;
            case TileState.Plowed:
                Plant();
                break;
            case TileState.Ready:
                Harvest();
                break;
        }
    }

    private void Plow()
    {
        _currentState = TileState.Plowed;
        _plowed.SetActive(true);
    }

    private void Plant()
    {
        _currentState = TileState.Planted;
        _planted.SetActive(true);
        StartCoroutine(Grow());
    }

    private void Harvest()
    {
        _currentState = TileState.Plowed;
        _ready.SetActive(false);
    }

    private IEnumerator Grow()
    {
        yield return new WaitForSeconds(_timeGrow);

        _currentState = TileState.Ready;
        _planted.SetActive(false);
        _ready.SetActive(true);
    }
}
