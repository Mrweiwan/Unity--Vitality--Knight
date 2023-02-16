using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueShooting
{
    public class RoomGenerator : MonoBehaviour
    {
        public enum Direction { up, down, left, right };
        public Direction direction;

        [Header("������Ϣ")]
        public GameObject roomPrefab;
        public int roomNumber;//��������
        public Color startColor, endColor;//��ʼ��������󷿼����ɫ
        private GameObject endRoom;//���ķ���
        public GameObject portal;//������

        [Header("λ�ÿ���")]
        public Transform generatorPoint;//���ɷ���λ��
        public float xOffset;//x��ƫ����
        public float yOffset;//y��ƫ����
        public LayerMask roomLayer;

        public int maxStep;//������

        public GameObject treasure;//����

        public List<Room> rooms = new List<Room>();

        List<GameObject> farRooms = new List<GameObject>();//��Զ�ķ���

        List<GameObject> lessFarRooms = new List<GameObject>();//�ڶ�Զ�ķ���

        List<GameObject> oneWayRooms = new List<GameObject>();//ֻ��һ��ͨ���ķ���

        public WallType wallType;//ǽ

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
                //�ı�generatorPointλ��
                ChangePointPos();
            }
            rooms[0].GetComponent<SpriteRenderer>().color = startColor;

            //��ȡ���շ��䲢�����շ������ɫ���и���
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
                    float x = Random.Range(room.transform.position.x - 6, room.transform.position.x + 6);//�涨x�᷽���ϵķ�Χ
                    float y = Random.Range(room.transform.position.y - 2, room.transform.position.y + 2);//�涨y�᷽���ϵķ�Χ
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
        /// �ж����������Ƿ��з���
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
            //��ȡ�����
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].stepToStart > maxStep)
                {
                    maxStep = rooms[i].stepToStart;
                }
            }

            //��ȡ���ﲽ�����ķ���ʹδ�ķ���
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


