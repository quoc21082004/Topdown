using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianSkill : SkillSet
{
    public Player player;
    public Animator myanim;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public override void NormalAttack()
    {
        myanim.SetTrigger("Attack");
    }
    public override void UltimateSkill()
    {
        player.mana -= ultimateskill.skillMpCost;
        myanim.SetTrigger("Ultimate");
    }
}
