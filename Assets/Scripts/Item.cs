using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public GameObject gameObject;

    public abstract void Use(float time);
    public abstract void LetGo();
}
