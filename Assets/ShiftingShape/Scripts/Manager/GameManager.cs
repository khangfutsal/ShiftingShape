using System;
using System.Collections;
using System.Collections.Generic;
using Connect4;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace ShiftingShape
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Level Manager")]
        [SerializeField] private List<LevelSO> listLevelsSO;
        [SerializeField] private List<BaseShape> shapesLevel;
        [SerializeField] private int countChoice;

        [Header("Status Game")]
        [SerializeField] private GameState currentState;

        [Header("Object Game")]
        [SerializeField] private Player player;
        [SerializeField] private Bot bot1;
        [SerializeField] private Bot bot2;
        [SerializeField] private Bot bot3;
        public List<BaseShape> GetListShapesFromLevel() => shapesLevel;
        public List<LevelSO> GetLevelSO() => listLevelsSO;


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SetGameState(GameState gameState)
        {
            this.currentState = gameState;
        }

        public void StartGame()
        {
            currentState = GameState.StartGame;
        }

        public void FinishGame()
        {
            Debug.Log("Win Game");
            currentState = GameState.WinGame;
        }

        public GameState GetGameState() => currentState;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //InitReference();
            if (scene.name != "Start")
            {
                LevelSO levelSO = listLevelsSO[0];
                Debug.Log("LevelSO :" + levelSO);
                if (levelSO != null)
                {
                    for (int i = 0; i < countChoice; i++)
                    {
                        var listShapes = ShapeManager.Ins.GetListShapes();
                        var shape = listShapes.Find(shape => shape.shapeSO.shapeData.shapeType == levelSO.shapeTypes[i]);
                        Debug.Log("Shape : " + shape.name);
                        shapesLevel.Add(shape);
                    }
                    player.InitializeList(shapesLevel);

                    bot1.InitializeList(shapesLevel);
                    bot2.InitializeList(shapesLevel);
                    bot3.InitializeList(shapesLevel);

                    UIManager.Ins.uiGameplay.IntializeList(player.GetListShapes());

                }

            }
            Debug.Log($"Scene Loaded: {scene.name}, Load Mode: {mode}");
        }

        private void OnValidate()
        {
            if (player == null)
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }
            if (bot1 == null)
            {
                bot1 = GameObject.Find("Bot1").GetComponent<Bot>();
            }
            if (bot2 == null)
            {
                bot2 = GameObject.Find("Bot2").GetComponent<Bot>();
            }
            if (bot3 == null)
            {
                bot3 = GameObject.Find("Bot3").GetComponent<Bot>();
            }
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}

