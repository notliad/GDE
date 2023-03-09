using UnityEngine;


public class Config : MonoBehaviour
{
    public static string ConfigTag { get; private set; } = "GameConfiguration";
    [SerializeField] public float sprintspeed = 6;
    [SerializeField] public float walkspeed = 3;
    [SerializeField] public float JUMP_FORCE = 200;
    [SerializeField] public float SMOOTH_TIME = 0.15f;
    [SerializeField] public float MOUSE_SENSIVITY = 3;


}
