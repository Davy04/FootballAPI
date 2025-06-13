using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BadgeDB", menuName = "DB/Badge Database")]
public class BadgeDb : ScriptableObject
{
    public List<ClubBadge> badges;
}