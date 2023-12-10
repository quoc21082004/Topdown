using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyHurt : MonoBehaviour
{
    public Enemy enemy;
    LootSpawner lootspawner;
    Transform builetpool;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        builetpool = GameObject.Find("BuiletPool").GetComponent<Transform>();
        lootspawner = GetComponent<LootSpawner>();
    }
    private void Update()
    {
        if (enemy.isDead)
        {
            Destroy(this.gameObject);
        }
    }
    public virtual void TakeDamage(float amount, bool isCrit)
    {
        enemy.heath = Mathf.Clamp(enemy.heath - amount, 0, enemy.maxhealth);
        enemy.myanim.SetTrigger("Hit");
        DamagePopManager.instance.CreateDamagePop(isCrit, amount, new Vector2(transform.position.x, transform.position.y + 0.75f), transform.parent); 
        if (enemy.heath <= 0)
            EnemyDead();
    }
    private void DropLootItem()
    {
        lootspawner.SpawnLoot(transform.position, builetpool.transform);
        PartyController.AddGold(enemy.enemystat.goldReward);
        PartyController.AddExperience(enemy.enemystat.expReward);
    }
    public void EnemyDead()
    {
        enemy.isDead = true;
        DropLootItem();
        PartyController.EnemyKilled(enemy.type.ToString());
        Destroy(this.gameObject);
    }
}
