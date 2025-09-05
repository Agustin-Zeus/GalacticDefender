using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Shmup
{
    public class EnemyFactory 
    {
        public GameObject CreateEnemy(EnemyType enemyShip, SplineContainer spline)
        {
            EnemyBuilder builder = new EnemyBuilder()
                .SetBasePrefab(enemyShip.enemyPrefab)
                .SetSpline(spline)
                .SetSpeed(enemyShip.speed);
            
            return builder.Build();
        }
    }

}
