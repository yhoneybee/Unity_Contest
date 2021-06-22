using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int BlockValue;
    public bool isUnblock;
    public bool isPortal;
    public bool isEnter = false;
    public bool isExit = false;
    public int myBlockNumber;
    public Image img { get; set; }
    public Text BlockValueTxt { get; set; }
    RectTransform rt;

    private void Awake()
    {
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
        BlockValueTxt = GetComponentInChildren<Text>();
    }

    void Start()
    {
        GameManager.instance.Blocks.Add(gameObject);
        BlockValueTxt.text = BlockValue.ToString();
    }

    void Update()
    {
        if (isPortal)
        {
            if (isEnter)
                img.color = new Color(0.1f, 0.3f, 0.5f);
            if (isExit)
                img.color = new Color(0.1f, 0.5f, 0.5f);

            if (isUnblock)
            {
                isUnblock = false;
                int rand = Random.Range(0, 25);
                Debug.Log($"{myBlockNumber} ---> 장애물 위치 재탐색 ---> index : {rand}");
                GameManager.instance.Blocks[rand].GetComponent<Block>().isUnblock = true;
            }
            BlockValue = -1;
        }
        else if (isUnblock)
        {
            BlockValue = 0;
            img.color = new Color(0.9f, 0.1f, 0.1f);
            BlockValueTxt.text = "X";

            if (myBlockNumber == 0)
            {
                if (GameManager.instance.Blocks[myBlockNumber + 1].GetComponent<Block>().BlockValue <= 0 &&
                 GameManager.instance.Blocks[myBlockNumber + Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber == Algorithm.Instance.cell_size.x - 1)
            {
                if (GameManager.instance.Blocks[myBlockNumber - 1].GetComponent<Block>().BlockValue <= 0 &&
                 GameManager.instance.Blocks[myBlockNumber + Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber == Algorithm.Instance.cell_size.x * (Algorithm.Instance.cell_size.x - 1))
            {
                if (GameManager.instance.Blocks[myBlockNumber + 1].GetComponent<Block>().BlockValue <= 0 &&
                 GameManager.instance.Blocks[myBlockNumber - Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber == Algorithm.Instance.cell_size.x * (Algorithm.Instance.cell_size.x) - 1)
            {
                if (GameManager.instance.Blocks[myBlockNumber - 1].GetComponent<Block>().BlockValue <= 0 &&
                 GameManager.instance.Blocks[myBlockNumber - Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber % 5 == 0)
            {
                if (GameManager.instance.Blocks[myBlockNumber + 1].GetComponent<Block>().BlockValue <= 0 &&
                     GameManager.instance.Blocks[myBlockNumber - Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0 &&
                     GameManager.instance.Blocks[myBlockNumber + Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber % 4 == 0)
            {
                if (GameManager.instance.Blocks[myBlockNumber - 1].GetComponent<Block>().BlockValue <= 0 &&
                     GameManager.instance.Blocks[myBlockNumber - Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0 &&
                     GameManager.instance.Blocks[myBlockNumber + Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber > 0 && myBlockNumber < Algorithm.Instance.cell_size.x - 1)
            {
                if (GameManager.instance.Blocks[myBlockNumber - 1].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber + 1].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber + Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber > Algorithm.Instance.cell_size.x * (Algorithm.Instance.cell_size.x - 1) && myBlockNumber < Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.x - 1)
            {
                if (GameManager.instance.Blocks[myBlockNumber - 1].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber + 1].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber - Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }

            else if (myBlockNumber > 0 && myBlockNumber < Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.x - 1)
            {
                if (GameManager.instance.Blocks[myBlockNumber - 1].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber + 1].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber + Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0 &&
                    GameManager.instance.Blocks[myBlockNumber - Algorithm.Instance.cell_size.x].GetComponent<Block>().BlockValue <= 0)
                {
                    isUnblock = false;
                    img.color = new Color(0.8f, 0.4f, 0.8f);
                    Instantiate(GameManager.instance.ExplosionPrefab, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                }
            }
        }
        else
        {
            if (GameManager.instance.isClick == false)
            {
                img.color = new Color(1, 1, 1);
            }
            else
            {
                if (img.color == new Color(0.8f, 0.8f, 0.1f))
                {
                    if (GameManager.instance.BlockPosition.Last() != gameObject)
                    {
                        img.color = new Color(0.9f, 0.9f, 0.9f);
                    }
                }
            }
            BlockValueTxt.text = BlockValue.ToString();

            if (BlockValue == 0)
            {
                img.color = new Color(0.8f, 0.4f, 0.8f);
            }
        }
    }

    public void PointerUp()
    {
        if (GameManager.instance.isClick)
        {
            GameManager.instance.isClick = false;

            if (BlockValue > 0)
            {
                if (GameManager.instance.BlockPosition.Last() != gameObject)
                {
                    GameManager.instance.DragCount++;
                    foreach (var GameObj in GameManager.instance.BlockPosition)
                    {
                        if (GameObj.GetComponent<Block>().BlockValue > 0)
                        {
                            GameObj.GetComponent<Block>().BlockValue--;
                        }
                    }
                }
                else
                {
                    GameManager.instance.UndoList.Clear();
                }
            }

            GameManager.instance.drag_end_callback();

            GameManager.instance.BlockPosition.Clear();
        }
    }

    public void PointerDown()
    {
        GameManager.instance.UndoList.Clear();
        if (BlockValue > 0)
        {
            GameManager.instance.isClick = true;
            img.color = new Color(0.8f, 0.8f, 0.1f);

            GameManager.instance.BlockPosition.Add(gameObject);
            GameManager.instance.UndoList.Add(gameObject);
        }
    }

    public void Drag()
    {
        if (GameManager.instance.isClick == true)
        {
            if (BlockValue == 0)
                return;
            Vector2 LastAPosition = GameManager.instance.BlockPosition.Last().GetComponent<RectTransform>().anchoredPosition;

            if (Mathf.Abs(LastAPosition.x - gameObject.GetComponent<RectTransform>().anchoredPosition.x) > 60
                || Mathf.Abs(LastAPosition.y - gameObject.GetComponent<RectTransform>().anchoredPosition.y) > 60)
            {
                return;
            }

            if (LastAPosition.x != gameObject.GetComponent<RectTransform>().anchoredPosition.x &&
                LastAPosition.y != gameObject.GetComponent<RectTransform>().anchoredPosition.y)
            {
                return;
            }

            foreach (var GameObj in GameManager.instance.BlockPosition)
            {
                if (GameObj == gameObject)
                {
                    GameManager.instance.SameBlock = true;
                    break;
                }
                else
                {
                    GameManager.instance.SameBlock = false;
                }
            }
            if (GameManager.instance.SameBlock == false)
            {
                GameManager.instance.BlockPosition.Add(gameObject);
                GameManager.instance.UndoList.Add(gameObject);
                img.color = new Color(0.9f, 0.9f, 0.9f);
            }
            if (GameManager.instance.BlockPosition.Last() == gameObject)
            {
                GameManager.instance.draging_callback(gameObject);
                img.color = new Color(0.8f, 0.8f, 0.1f);
            }
        }
    }
}
