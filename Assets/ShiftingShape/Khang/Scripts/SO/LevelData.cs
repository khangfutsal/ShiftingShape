using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    [CreateAssetMenu(menuName = "Khang/SO/LevelData")]
    public class LevelData : ScriptableObject
    {
        public LevelType levelType;
        public List<ShapeType> shapeTypes;

        public int rewardForWinner;
        public int rewardForWinnerAfterAds;

        public int rewardForLoser;
        public bool isFinishedLevel;
    }

}
