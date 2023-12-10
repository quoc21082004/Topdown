using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : SkillSet
{
    public Player player;
    public PlayerAttack playerAttack;
    public Animator myanim;
    public Transform MuzzlePoint;
    public Transform SkillMuzzlePoint;
    public GameObject fireballprefab;
    public GameObject manaBurstprefab;
    bool isdouble = true;
    Transform builetPool;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        builetPool = GameObject.FindGameObjectWithTag("Pool").GetComponent<Transform>();
        playerAttack = GetComponent<PlayerAttack>();
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
    public void CastFireBall()
    {
        GameObject clone = Instantiate(fireballprefab, MuzzlePoint.transform.position, MuzzlePoint.transform.rotation,builetPool.transform);
        if (playerAttack.isDoubleShot)
        {
            StartCoroutine(DoubleShot());
        }
    }
    public void CastManaBurst()
    {
        GameObject clone = Instantiate(manaBurstprefab, SkillMuzzlePoint.transform.position, SkillMuzzlePoint.transform.rotation, builetPool.transform); 
    }
    IEnumerator DoubleShot()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject clone2 = Instantiate(fireballprefab, MuzzlePoint.transform.position, MuzzlePoint.transform.rotation, builetPool.transform);
    }

}
