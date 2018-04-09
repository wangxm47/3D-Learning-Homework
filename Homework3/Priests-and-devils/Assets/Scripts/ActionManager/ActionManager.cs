using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class ActionManager : MonoBehaviour, ActionCallback
    {
        Dictionary<int, ObjAction> actions = new Dictionary<int, ObjAction>();
        List<ObjAction> ToAdd = new List<ObjAction>();
        List<int> ToDelete = new List<int>();
        public void ActionDone(ObjAction source){}

        void Update()
        {
            foreach(ObjAction action in ToAdd)
            {
                actions[action.GetInstanceID()] = action;
            }
            ToAdd.Clear();

            foreach (KeyValuePair<int, ObjAction> kv in actions)
            {
                ObjAction action = kv.Value;
                if (action.destroy)
                {
                    ToDelete.Add(action.GetInstanceID());
                }
                else if (action.enable)
                {
                    action.Update();
                }
            }

            foreach (int key in ToDelete)
            {
                ObjAction action = actions[key];
                actions.Remove(key);
                DestroyObject(action);
            }
            ToDelete.Clear();
        }

        public void addAction(GameObject gameObject, ObjAction action, ActionCallback callback)
        {
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = callback;
            ToAdd.Add(action);
            action.Start();
        }
    }
}
