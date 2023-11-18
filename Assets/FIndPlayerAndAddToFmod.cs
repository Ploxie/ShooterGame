using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Assets.Scripts.Entity;
using UnityEditor;
using Unity.VisualScripting;

public class FIndPlayerAndAddToFmod : MonoBehaviour
{
    public StudioListener listener;
    public Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        listener = FindObjectOfType<StudioListener>();
    }
}
