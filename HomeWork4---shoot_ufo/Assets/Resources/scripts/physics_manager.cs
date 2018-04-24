using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics_manager : SSActionManager
{
    public SceneController sceneController;
    public DiskFactory diskFactory;
    public physics_move DiskMove;
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
                DiskMove = physics_move.GetSSAction();
                this.RunAction(diskFactory.getDisk(sceneController.round), DiskMove);
                sceneController.num++;
                //print(sceneController.num);
                count = 0;
            }
            base.Update();
        }
    }
}