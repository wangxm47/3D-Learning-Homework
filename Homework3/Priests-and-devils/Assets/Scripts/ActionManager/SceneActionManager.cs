using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class SceneActionManager : ActionManager
    {
        public void moveBoat(BoatController boat)
        {
            MoveAction action = MoveAction.getAction(boat.getDestination(), boat.speed);
            this.addAction(boat.getGameobj(), action, this);
        }

        public void moveCharacter(MyCharacterController character, Vector3 destination)
        {
            Vector3 currentPos = character.getPos();
            Vector3 middlePos = currentPos;
            if (destination.y > currentPos.y)
            {       //from low(boat) to high(coast)
                middlePos.y = destination.y;
            }
            else
            {   //from high(coast) to low(boat)
                middlePos.x = destination.x;
            }
            ObjAction action1 = MoveAction.getAction(middlePos, character.speed);
            ObjAction action2 = MoveAction.getAction(destination, character.speed);
            ObjAction seqAction = SequenceAction.getAction(new List<ObjAction> { action1, action2 });
            this.addAction(character.GetGameObject(), seqAction, this);
        }
    }
}
