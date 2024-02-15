using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableCoin : CollectableBase
{
    public Collider collider;

    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddCoin();
        collider.enabled = false;
    }
}
