﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class SequenceAction : ObjAction , ActionCallback
    {
        public List<ObjAction> sequence;
        public int repeat = 1;
        public int currentActionIndex = 0;

        public static SequenceAction getAction(List<ObjAction> sequence,int repeat=1, int currentActionIndex=0)
        {
            SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
            action.sequence = sequence;
            action.repeat = repeat;
            action.currentActionIndex = currentActionIndex;
            return action;
        }

        public override void Update()
        {
            if (sequence.Count == 0) return;
            if (currentActionIndex < sequence.Count)
            {
                sequence[currentActionIndex].Update();
            }
        }

        public void ActionDone(ObjAction source)
        {
            source.destroy = false;
            this.currentActionIndex++;
            if (this.currentActionIndex >= sequence.Count)
            {
                this.currentActionIndex = 0;
                if (repeat > 0) repeat--;
                if (repeat == 0)
                {
                    this.destroy = true;
                    this.callback.ActionDone(this);
                }
            }
        }

        public override void Start()
        {
            foreach (ObjAction action in sequence)
            {
                action.gameObject = this.gameObject;
                action.transform = this.transform;
                action.callback = this;
                action.Start();
            }
        }

        void  OnDestroy()
        {
            foreach (ObjAction action in sequence)
            {
                DestroyObject(action);
            }
        }
    }
}
