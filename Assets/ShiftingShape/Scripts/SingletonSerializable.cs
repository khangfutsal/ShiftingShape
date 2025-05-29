using UnityEngine;

namespace Connect4 {
  public class SingletonSerializable<T> : MonoBehaviour where T : Component {
    private static T instance;

    public static T Ins {
      get {
        if (instance == null) {
          instance = FindObjectOfType<T>();
          if (instance == null) {
            GameObject singletonObject = new GameObject();
            instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T).ToString() + " (SingletonSerializable)";
          }
        }

        return instance;
      }
    }

    protected virtual void Awake() {
      if (instance == null) {
        instance = this as T;
      }
      else if (instance != this) {
        Destroy(gameObject);
      }
    }

    private void OnDestroy() {
      if (instance == this) {
        instance = null;
      }
    }
  }
}