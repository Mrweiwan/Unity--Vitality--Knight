using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class RoomGenerator : MonoBehaviour
    {
        public enum Direction { up, down, left, right };
        public Direction direction;

        [Header("房间信息")]
        public GameObject roomPrefab;
        public int roomNumber;//房间数量
        public Color startColor, endColor;//初始房间与最后房间的颜色
        private GameObject endRoom;//最后的房间
        public GameObject portal;//传送门

        [Header("位置控制")]
        public Transform generatorPoint;//生成房间位置
        public float xOffset;//x轴偏移量
        public float yOffset;//y轴偏移量
        public LayerMask roomLayer;

        public int maxStep;//最大距离

        public GameObject treasure;//宝箱

        public List<Room> rooms = new List<Room>();

        List<GameObject> farRooms = new List<GameObject>();//最远的房间

        List<GameObject> lessFarRooms = new List<GameObject>();//第二远的房间

        List<GameObject> oneWayRooms = new List<GameObject>();//只有一条通道的房间

        public WallType wallType;//墙

        public Transform walls;
        void Start()
        {
            for (int i = 0; i < roomNumber; i++)
            {
                var newRoom = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity);
                if (i==0)
                {
                    newRoom.GetComponent<Room>().isFistRoom = true;
                }
                newRoom.transform.SetParent(transform);
                rooms.Add(newRoom.GetComponent<Room>());
                //改变generatorPoint位置
                ChangePointPos();
            }
            rooms[0].GetComponent<SpriteRenderer>().color = startColor;

            //获取最终房间并将最终房间的颜色进行更改
            endRoom = rooms[0].gameObject;
            foreach (var room in rooms)
            {
                SetUpRoom(room, room.transform.position);
                
            }
            
            FindEndRoom();
            endRoom.GetComponent<SpriteRenderer>().color = endColor;
            endRoom.GetComponent<Room>().isEndRoom = true;
            Instantiate(portal, endRoom.transform.position, Quaternion.identity, endRoom.transform);
            foreach (var room in rooms) 
            {
                if (room.isFistRoom == false && room.isEndRoom == false) 
                {
                    float x = Random.Range(room.transform.position.x - 6, room.transform.position.x + 6);//规定x轴方向上的范围
                    float y = Random.Range(room.transform.position.y - 2, room.transform.position.y + 2);//规定y轴方向上的范围
                    Vector2 newPoint = new Vector2(x, y);
                    var treasureDemo =  Instantiate(treasure, newPoint, Quaternion.identity, room.transform);
                    treasureDemo.SetActive(false);
                }
            }
        }


        void ChangePointPos()
        {
            do
            {
                direction = (Direction)Random.Range(0, 4);
                switch (direction)
                {
                    case Direction.up:
                        generatorPoint.position += new Vector3(0, yOffset, 0);
                        break;
                    case Direction.down:
                        generatorPoint.position += new Vector3(0, -yOffset, 0);
                        break;
                    case Direction.left:
                        generatorPoint.position += new Vector3(-xOffset, 0, 0);
                        break;
                    case Direction.right:
                        generatorPoint.position += new Vector3(xOffset, 0, 0);
                        break;
                    default:
                        break;
                }
            } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer));

        }
        /// <summary>
        /// 判断上下左右是否有房间
        /// </summary>
        public void SetUpRoom(Room newRoom,Vector3 roomPosition) 
        {
            newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0),0.2f,roomLayer);
            newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0),0.2f,roomLayer);
            newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0),0.2f,roomLayer);
            newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0),0.2f,roomLayer);

            newRoom.UpdateRoom(xOffset,yOffset);
            switch (newRoom.doorNumber)
            {
                case 1:
                    if (newRoom.roomUp)
                        Instantiate(wallType.singleUp, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomDown)
                        Instantiate(wallType.singleBottom, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomLeft)
                        Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomRight)
                        Instantiate(wallType.singleRight, roomPosition, Quaternion.identity, walls);
                    break;
                case 2:
                    if (newRoom.roomLeft && newRoom.roomUp)
                        Instantiate(wallType.doubleLU, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomLeft && newRoom.roomRight)
                        Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomLeft && newRoom.roomDown)
                        Instantiate(wallType.doubleLB, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomDown && newRoom.roomUp)
                        Instantiate(wallType.doubleUB, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomRight && newRoom.roomUp)
                        Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomRight && newRoom.roomDown)
                        Instantiate(wallType.doubelRB, roomPosition, Quaternion.identity, walls);
                    break;
                case 3:
                    if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomDown)
                        Instantiate(wallType.tripleLUB, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomRight && newRoom.roomUp && newRoom.roomDown)
                        Instantiate(wallType.tripleURB, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown)
                        Instantiate(wallType.tripleLRB, roomPosition, Quaternion.identity, walls);
                    if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomUp)
                        Instantiate(wallType.tripleLUR, roomPosition, Quaternion.identity, walls);
                    break;
                case 4:
                    if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomDown && newRoom.roomRight)
                        Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity, walls);
                    break;
            }
        }

        public void FindEndRoom() 
        {
            //获取最大步数
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].stepToStart > maxStep)
                {
                    maxStep = rooms[i].stepToStart;
                }
            }

            //获取到达步数最大的房间和次大的房间
            foreach (var room in rooms)
            {
                if (room.stepToStart == maxStep)
                {
                    farRooms.Add(room.gameObject);
                }
                if (room.stepToStart == maxStep -1)
                {
                    lessFarRooms.Add(room.gameObject);
                }
            }

            for (int i = 0; i < farRooms.Count; i++)
            {
                if (farRooms[i].GetComponent<Room>().doorNumber == 1)
                {
                    oneWayRooms.Add(farRooms[i].gameObject);
                }
            }
            for (int i = 0; i < lessFarRooms.Count; i++)
            {
                if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1)
                {
                    oneWayRooms.Add(lessFarRooms[i].gameObject);
                }
            }

            if (oneWayRooms.Count!=0)
            {
                endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
            }
            else
            {
                endRoom = farRooms[Random.Range(0, farRooms.Count)];
            }
        }
    }
    [System.Serializable]
    public class WallType
    {
        public GameObject singleLeft, singleRight, singleUp, singleBottom,
                          doubleLU, doubleLR, doubleLB, doubleUR, doubleUB, doubelRB,
                          tripleLUR, tripleLUB, tripleURB, tripleLRB,
                          fourDoors;
    }

}


