using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [System.Serializable]
    public class PlayerStats
    {
        public int Health = 100;
    }

    /* this is for testing, do not use
        private void Update()
        {
            if (transform.position.x >= 10)
            {
                DamagePlayer(1000000);
            }
        }
    */

    public PlayerStats playerStats = new PlayerStats();

    public void DamagePlayer(int damage)
    {
        playerStats.Health -= damage;
        if (playerStats.Health <= 0)
        {
            GameMaster.KillPlayer(this);
        }
    }
}
