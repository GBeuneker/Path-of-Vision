using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossRoom : Room
{

    Vector2 hatchPosition;

    public BossRoom(List<string> roomLayout)
        : base(roomLayout)
    {
        this.roomLayout = roomLayout;
        FindHatch();
    }

    private void FindHatch()
    {
        for (int y = 0; y < this.Height; ++y)
            for (int x = 0; x < this.Width; ++x)
            {
                if (GetTile(x, y) == '+')
                {
                    hatchPosition = new Vector2(x, y);
                    return;
                }
            }
    }

    public void SpawnBoss()
    {
        Enemy randomBoss = new Enemy(RandomBossPath(), this);
        randomBoss.LocalPosition = hatchPosition - new Vector2(0, (randomBoss.enemyObject.renderer.bounds.size.y / 2));

        if (randomBoss.LocalPosition != Vector2.zero)
            enemyList.Add(randomBoss);
    }

    private string RandomBossPath()
    {
        return "Enemy Prefabs/Boss " + RandomGenerator.RandomInt(1, 2); // Amount of enemies we have
    }

    public void OpenHatch()
    {
        SetTile((int)hatchPosition.x, (int)hatchPosition.y, '=');
        return;
    }

}
