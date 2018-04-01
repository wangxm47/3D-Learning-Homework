using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mygame;

namespace Mygame
{
    public class Director : System.Object
    {
        private static Director _instance;
        public SceneController scene { get; set; }

        public static Director getInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }
    }
    public interface SceneController
    {
        void loadResources();
    }

    public interface UserAction
    {
        void moveBoat();
        void characterIsClicked(MyCharacterController chara);
        void restart();
    }

    public class Movebehavior : MonoBehaviour
    {
        float speed = 20;
        int status;
        Vector3 dest;
        Vector3 middle;

        public void Update()
        {
            if (status == 1)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, middle, speed * Time.deltaTime);
                if (this.transform.position == middle)
                {
                    status = 2;
                }
            }
            else if (status == 2)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, dest, speed * Time.deltaTime);
                if (this.transform.position == dest)
                {
                    status = 0;
                }
            }
        }

        public void reset()
        {
            status = 0;
        }

        public void set_dest(Vector3 vec)
        {
            dest = vec;
            middle = vec;

            if (transform.position.y == vec.y)
            {
                status = 2;
            }
            else if (transform.position.y > vec.y)
            {
                middle.y = transform.position.y;
            }
            else
            {
                middle.x = transform.position.x;
            }
            status = 1;
        }
    }


    public class MyCharacterController
    {
        GameObject character;
        Movebehavior move;
        public ClickGui click_gui;
        int cha_type;
        bool in_boat;
        CoastController coast;

        public MyCharacterController(string type)
        {
            if (type == "devil")
            {
                character = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                cha_type = 1;
            }
            else
            {
                character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                cha_type = 2;
            }
            move = character.AddComponent(typeof(Movebehavior)) as Movebehavior;
            click_gui = character.AddComponent(typeof(ClickGui)) as ClickGui;
            click_gui.setController(this);
        }
        public void setName(string name)
        {
            character.name = name;
        }

        public void setPosition(Vector3 pos)
        {
            character.transform.position = pos;
        }

        public void moveToPosition(Vector3 dest)
        {
            move.set_dest(dest);
        }

        public int getType()
        {   
            return cha_type;
        }

        public string getName()
        {
            return character.name;
        }

        public void getOnBoat(BoatController boatCtrl)
        {
            coast = null;
            character.transform.parent = boatCtrl.getGameobj().transform;
            in_boat = true;
        }

        public void getOnCoast(CoastController coastCtrl)
        {
            coast = coastCtrl;
            character.transform.parent = null;
            in_boat = false;
        }

        public bool isOnBoat()
        {
            return in_boat;
        }

        public CoastController getCoastController()
        {
            return coast;
        }

        public void reset()
        {
            move.reset();
            coast = (Director.getInstance().scene as FirstController).fromCoast;
            getOnCoast(coast);
            setPosition(coast.getEmptyPosition());
            coast.getOnCoast(this);           
        }
    }
    public class BoatController
    {
        readonly GameObject boat;
        readonly Movebehavior moveableScript;
        readonly Vector3 fromPosition = new Vector3(5, 1, 0);
        readonly Vector3 toPosition = new Vector3(-5, 1, 0);
        readonly Vector3[] from_positions;
        readonly Vector3[] to_positions;

        // change frequently
        int to_or_from; // to->-1; from->1
        MyCharacterController[] passenger = new MyCharacterController[2];

        public BoatController()
        {
            to_or_from = 1;

            from_positions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
            to_positions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };

            boat = Object.Instantiate(Resources.Load("Prefabs/boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
            boat.name = "boat";

            moveableScript = boat.AddComponent(typeof(Movebehavior)) as Movebehavior;
            boat.AddComponent(typeof(ClickGui));
        }


        public void Move()
        {
            if (to_or_from == -1)
            {
                moveableScript.set_dest(fromPosition);
                to_or_from = 1;
            }
            else
            {
                moveableScript.set_dest(toPosition);
                to_or_from = -1;
            }
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool isEmpty()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos;
            int emptyIndex = getEmptyIndex();
            if (to_or_from == -1)
            {
                pos = to_positions[emptyIndex];
            }
            else
            {
                pos = from_positions[emptyIndex];
            }
            return pos;
        }

        public void GetOnBoat(MyCharacterController characterCtrl)
        {
            int index = getEmptyIndex();
            passenger[index] = characterCtrl;
        }

        public MyCharacterController GetOffBoat(string passenger_name)
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null && passenger[i].getName() == passenger_name)
                {
                    MyCharacterController charactorCtrl = passenger[i];
                    passenger[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("Cant find passenger in boat: " + passenger_name);
            return null;
        }

        public GameObject getGameobj()
        {
            return boat;
        }

        public int get_to_or_from()
        { // to->-1; from->1
            return to_or_from;
        }

        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                    continue;
                if (passenger[i].getType() == 2)
                {   
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            moveableScript.reset();
            if (to_or_from == -1)
            {
                Move();
            }
            passenger = new MyCharacterController[2];
        }
    }

    public class CoastController
    {
        readonly GameObject coast;
        readonly Vector3 from_pos = new Vector3(9, 1, 0);
        readonly Vector3 to_pos = new Vector3(-9, 1, 0);
        readonly Vector3[] positions;
        readonly int to_or_from;    // to->-1, from->1

        // change frequently
        MyCharacterController[] passengerPlaner;

        public CoastController(string coast_where)
        {
            positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0),
                new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};

            passengerPlaner = new MyCharacterController[6];

            if (coast_where == "from")
            {
                coast = Object.Instantiate(Resources.Load("Prefabs/coast", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
                coast.name = "from";
                to_or_from = 1;
            }
            else
            {
                coast = Object.Instantiate(Resources.Load("Prefabs/coast", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
                coast.name = "to";
                to_or_from = -1;
            }
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos = positions[getEmptyIndex()];
            pos.x *= to_or_from;
            return pos;
        }

        public void getOnCoast(MyCharacterController characterCtrl)
        {
            int index = getEmptyIndex();
            passengerPlaner[index] = characterCtrl;
        }

        public MyCharacterController getOffCoast(string passenger_name)
        {   // 0->priest, 1->devil
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
                {
                    MyCharacterController charactorCtrl = passengerPlaner[i];
                    passengerPlaner[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("cant find passenger on coast: " + passenger_name);
            return null;
        }

        public int get_to_or_from()
        {
            return to_or_from;
        }

        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] == null)
                    continue;
                if (passengerPlaner[i].getType() == 2)
                {   
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            passengerPlaner = new MyCharacterController[6];
        }
    }
}
