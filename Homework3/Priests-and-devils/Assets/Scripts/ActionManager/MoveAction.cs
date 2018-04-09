using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class MoveAction : ObjAction
    {
        public Vector3 target;
        public float speed;

        private MoveAction() { }
        public static MoveAction getAction(Vector3 target, float speed)
        {
            MoveAction action = ScriptableObject.CreateInstance<MoveAction>();
            action.target = target;
            action.speed = speed;
            return action;
        }
        public override void Start() { }
        public override void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                this.destroy = true;
                this.callback.ActionDone(this);
            }
        }
    }
}
