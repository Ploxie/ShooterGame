using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersForMusicChange : MonoBehaviour
{
    [Header("Area")]

    [SerializeField] private MusicFMOD area;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            AudioFmodManager.instance.SetMusicArea(area);
        }
    }
}
