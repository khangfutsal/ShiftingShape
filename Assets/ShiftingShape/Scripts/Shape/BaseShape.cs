using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace ShiftingShape
{
    public abstract class BaseShape : MonoBehaviour
    {
        public ShapeSO shapeSO;
        public bool isUsing;
        public bool isTouchingLayerMask;
        public LayerMask layerMask;
        public bool playerControl;


        public abstract void TransitionShape();
        public abstract void CompleteShape();
        
        public virtual void SubscribeEventPlayerControl()
        {
            playerControl = true;
        }

        protected virtual void Start()
        {
            layerMask = shapeSO.shapeData.interact_Layermask;
        }

        


    }
}

