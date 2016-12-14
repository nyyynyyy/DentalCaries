using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveBar : Bar {

	void Update ()
    {
        Enemy target = EnemyHpBar.instance.target;
        if (target != null && EnemyHpBar.instance.isMoved)
        {
            float dis = target.travelDistancePer;
            RenderBar(NumberToWidth(dis, 1f));
        }
	}

}
