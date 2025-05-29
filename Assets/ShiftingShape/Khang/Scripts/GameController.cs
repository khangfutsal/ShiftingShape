using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Khang
{
    public class GameController : Singleton<GameController>
    {
        public GameManager GameManager;

        [SerializeField] private GameState prevGameState;
        [SerializeField] private UnityEvent onLose = new UnityEvent();
        [SerializeField] private UnityEvent onWin = new UnityEvent();
        [SerializeField] private UnityEvent onMainMenu = new UnityEvent();
        [SerializeField] private UnityEvent onPause = new UnityEvent();
        [SerializeField] private UnityEvent onSetup = new UnityEvent();
        [SerializeField] private UnityEvent onStart = new UnityEvent();
        [SerializeField] private UnityEvent onPlaying = new UnityEvent();

        [SerializeField] private bool havingResults;

        public bool GetGameResult() => havingResults;


        private void Start()
        {
            onLose?.AddListener(() =>
            {
                DebugState(GameState.Lose);
            });


            onWin?.AddListener(() =>
            {
                DebugState(GameState.Win);
            });
        }

        private void Update()
        {
            UpdateBehaviour();
        }

        public void UpdateBehaviour()
        {
            UpdateStateGame();
        }

        public void UpdateStateGame()
        {
            if (prevGameState == GameManager.GetGameState()) return;

            switch (GameManager.GetGameState())
            {
                case GameState.None: break;
                case GameState.StartGame: onStart?.Invoke(); break;
                case GameState.Win: onWin?.Invoke(); break;
                case GameState.Lose: onLose?.Invoke(); break;
                case GameState.Pause: onPause?.Invoke(); break;
                case GameState.Setup: onSetup?.Invoke(); break;
                case GameState.MainMenu: onMainMenu?.Invoke(); break;
                case GameState.Playing: onPlaying?.Invoke(); break;

            }
            prevGameState = GameManager.GetGameState();
        }

        public void DebugState(GameState state)
        {
            Debug.Log($"State {state} is trigger");
        }

        public void SetResultGame(bool _result) => havingResults = _result;

    }
}

