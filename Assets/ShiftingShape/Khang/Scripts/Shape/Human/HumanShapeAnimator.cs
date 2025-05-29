using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Khang
{
    public class HumanShapeAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private int currentAnimHash;

        [SerializeField] private List<string> animName = new List<string> { "Climbing", "Running", "Rolling", "Falling" };

        // Animation name constants


        private void Start()
        {
            StartCoroutine(WaitForAnimator());
        }

        private IEnumerator WaitForAnimator()
        {
            yield return new WaitUntil(() => GetComponentInChildren<Animator>() != null);
            _animator = GetComponentInChildren<Animator>();
        }


        public void PlayAnim(string anim)
        {
            foreach (string name in animName)
            {
                _animator.SetBool(name, name == anim);
            }
        }

        private void OnDisable()
        {
            foreach (string name in animName)
            {
                _animator.SetBool(name, false);
            }
        }


    }
}
