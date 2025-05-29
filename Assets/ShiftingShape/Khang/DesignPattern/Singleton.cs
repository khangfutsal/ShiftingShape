using UnityEngine;

namespace Connect4 {
  public class Singleton<T> : MonoBehaviour where T : Component {
    private static T instance;

    public static T Ins {
      get {
        if (instance == null) {
          instance = FindObjectOfType<T>();
          if (instance == null) {
            GameObject singletonObject = new GameObject();
            instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T).ToString() + " (Singleton)";
            DontDestroyOnLoad(singletonObject);
          }
        }

        return instance;
      }
    }

    protected virtual void Awake() {
      if (instance == null) {
        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
      }
      else {
        Destroy(gameObject);
      }
    }
  }
}