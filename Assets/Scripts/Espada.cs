using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Espada : Item
{
    public abstract override void Use();

    public GameObject espadaThrowablePrefab;
}
