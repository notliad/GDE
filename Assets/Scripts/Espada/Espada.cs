using UnityEngine;

public abstract class Espada : Item
{
    public abstract override void Use(float time);

    public GameObject espadaThrowablePrefab;
}
