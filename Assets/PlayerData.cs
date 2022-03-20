using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int jumps;
    public int successfulDodges;
    public int damageDone;
    public int enemiesKilled;

    public PlayerData(int jumps, int successfulDodges, int damageDone, int enemiesKilled)
    {
        this.jumps = jumps;
        this.successfulDodges = successfulDodges;
        this.damageDone = damageDone;
        this.enemiesKilled = enemiesKilled;
    }
}
