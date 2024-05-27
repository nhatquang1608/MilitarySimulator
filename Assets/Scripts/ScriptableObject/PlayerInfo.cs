using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "ScriptableObjects/PlayerInfo", order = 1)]
public class PlayerInfo : ScriptableObject
{
    [Header("Infomation")]
    // 3: Harry
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    [Header("Weapon")]
    // 0: noweapon; 1: AK47;
    public int mainWeaponId;
    public bool isUsedMainWeapon;
    public bool isUsedGrenade;

    [Header("Ammo")]
    public int totalBullet;
    public int presentBullet;
    public int remainBullet;
    public int totalGrenade;

    [Header("Health")]
    public int maxHealth = 100;
    public int presentHealth;
    public int totalBandages;
    public int totalFirstAidKit;
    public int totalAdrenaline;
}
