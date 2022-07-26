using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgerBullet : MonoBehaviour
{
   public BulletData BulletData;

   [HideInInspector] public Vector2 directionForce;
   public float forceAmount;
}
