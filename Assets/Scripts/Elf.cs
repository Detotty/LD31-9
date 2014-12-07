using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

class Elf : Enemy
    {
        internal override void Awake()
        {
            actualSize = new Vector2(3f,3f);

            hairStyle = Random.Range(0, 3) + 1;
            headStyle = Random.Range(0, 3) + 1;

            hairAnim = transform.FindChild("Hair").GetComponent<tk2dSpriteAnimator>();
            clothesAnim = transform.FindChild("Clothes").GetComponent<tk2dSpriteAnimator>();

            CooldownModifier = 2f;

            BaseHealth = 10f;
            Health = BaseHealth;
            CanUseWeapons = true;

            base.Awake();

            Target = arena.FindChild("Center").position + (Random.insideUnitSphere * 6f);
            Target.y = 0f;
        }
    }
