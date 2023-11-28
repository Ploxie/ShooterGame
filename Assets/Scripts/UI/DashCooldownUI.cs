using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image progressCircle;
    private Player player;
    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        else
        {
            progressCircle.fillAmount = player.DashCooldownTimer / player.DashCooldown;
        }
    }
}
