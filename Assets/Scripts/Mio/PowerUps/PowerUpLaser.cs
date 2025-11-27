using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLaser : PowerUp
{
    [SerializeField] private GameObject _laser;
    [SerializeField] private List<GameObject> _laserList;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private float _shootingTime = 1f;
    protected new void Start()
    {
        base.Start();
        stackeable = false;
        AddLasersToPool(_poolSize);
        
    }

    private void AddLasersToPool(int amount)
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject laser = Instantiate(_laser);
            laser.SetActive(false);
            _laserList.Add(laser);
            laser.transform.parent = transform;
        }
    }

    private GameObject RequestLaser()
    {
        for(int i = 0; i < _laserList.Count; i++)
        {
            if (!_laserList[i].activeSelf)
            {
                _laserList[i].SetActive(true);
                return _laserList[i];
            }
        }
        return null;
    }
    public override void ApplyEffect()
    {
        GameManager.Instance.AddLifes(1);// Aumentamos la vida del GameManager
    }

    public override void Remove()
    {

    }
}
