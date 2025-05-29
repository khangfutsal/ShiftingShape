using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameState gameState;

        public GameState GetGameState() => gameState;
        public void SetGameState(GameState newGameState) => gameState = newGameState;
       
    }
}

