using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDebuff : MonoBehaviour
{
    public PlayerController playerController;

    private void Start()
    {
        SlowPlayer();
    }

    public void SlowPlayer() {
        playerController.speed /= 2;
    }
}
