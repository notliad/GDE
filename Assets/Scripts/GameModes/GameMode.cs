using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "GDE/Game/Mode", fileName = "GameMode")]
public class GameMode : ScriptableObject
{
    [SerializeField]
    private string _Name;
    [SerializeField]
    private string _Description;
    [SerializeField]
    private int _MaxPlayers;
    [SerializeField]
    private int _MinPlayers;
    [SerializeField]
    private int _TeamSize;

    public string Name { get => _Name; private set => _Name = value; }
    public string Description { get => _Description; private set => _Description = value; }
    public int MaxPlayers { get => _MaxPlayers; private set => _MaxPlayers = value; }
    public int MinPlayers { get => _MinPlayers; private set => _MinPlayers = value; }
    public int TeamSize { get => _TeamSize; private set => _TeamSize = value; }
}
