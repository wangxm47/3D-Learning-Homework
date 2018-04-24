using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : SSActionManager
{
    public SceneController sceneController;
    public DiskFactory diskFactory;
    public Move DiskMove;
    int count = 0;
    // Use this for initialization
    protected void Start()
    {
        sceneController = (SceneController)SSDirector.getInstance().currentScenceController;
        diskFactory = sceneController.factory;
        sceneController.actionManager = this;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (sceneController.round <= 3 && sceneController.game == 1)
        {
            count++;
            if (count == 60)
            {
                DiskMove = Move.GetSSAction();
                this.RunAction(diskFactory.getDisk(sceneController.round), DiskMove);
                sceneController.num++;
                //print(sceneController.num);
                count = 0;
            }
            base.Update();
        }
    }
}
