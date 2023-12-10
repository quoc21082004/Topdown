using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO: ScriptableObject
{
    public Sprite skillIcon;
    public int skillMpCost;
    public float skillCD;
}
