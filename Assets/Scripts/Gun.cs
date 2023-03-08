using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use(float time);

    public GameObject bulletImpactPrefab;

    public GameObject espadaThrowablePrefab;
}
