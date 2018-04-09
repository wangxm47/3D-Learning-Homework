using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

public class ClickGui : MonoBehaviour
{
    UserAction action;
    MyCharacterController characterController;

    public void setController(MyCharacterController characterCtrl)
    {
        characterController = characterCtrl;
    }

    void Start()
    {
        action = Director.getInstance().scene as UserAction;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "boat")
        {
            action.moveBoat();
        }
        else
        {
            action.characterIsClicked(characterController);
        }
    }
}
