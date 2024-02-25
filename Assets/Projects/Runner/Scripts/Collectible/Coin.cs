using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : BaseCollectible
{
    [SerializeField] private IntEventChannelSO _playerCollectCoin;
    [SerializeField] private int _coinScore;
    [SerializeField] private CollectibleParticle _coinParticle;
    protected override void Apply()
    {
        _playerCollectCoin.RaiseEvent(_coinScore);
        _poolManager.Spawn(_coinParticle, transform.position + Vector3.up, Quaternion.identity, null);
    }

}
