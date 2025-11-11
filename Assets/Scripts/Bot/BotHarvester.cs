using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotHarvester : MonoBehaviour
{
    [Header("Harvest Settings")]
    [SerializeField]
    private float _checkInterval = 5f;
    [SerializeField]
    private float _checkDistance = 10f;

    private bool _isHarverst = false;
    private List<FarmTile> _readyTiles = new();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _checkDistance);
    }

    private void Start()
    {
        StartCoroutine(CheckHarvest());
    }

    private void Update()
    {
        if (_isHarverst)
        {
            foreach (FarmTile tile in _readyTiles)
            {
                tile.Harvest();
            }

            _isHarverst = false;
        }
    }

    private IEnumerator CheckHarvest()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkInterval);

            if (!_isHarverst)
            {
                FindReadyPlants();

                if (_readyTiles.Count > 0)
                {
                    _isHarverst = true;
                }
            }
        }
    }

    private void FindReadyPlants()
    {
        foreach (FarmTile tile in GridManager.Instance.Grid)
        {
            float distance = Vector3.Distance(transform.position, tile.transform.position);

            if (distance < _checkDistance && tile.IsReady)
            {
                _readyTiles.Add(tile);
            }
        }
    }
}
