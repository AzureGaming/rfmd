using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int jumps;
    public int successfulDodges;
    public int damageDone;
    public int enemiesKilled;
    public int currency;

    public PlayerData(int jumps, int successfulDodges, int damageDone, int enemiesKilled, int currency)
    {
        this.jumps = jumps;
        this.successfulDodges = successfulDodges;
        this.damageDone = damageDone;
        this.enemiesKilled = enemiesKilled;
        this.currency = currency;
    }
}
