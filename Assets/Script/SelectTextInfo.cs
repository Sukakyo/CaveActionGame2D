using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectTextInfo : MonoBehaviour
{
    [SerializeField]
    TextAsset node_csv;
    [SerializeField]
    TextAsset trans_csv;

    List<string[]> _nodeData = new List<string[]>();
    List<string[]> _transData = new List<string[]>();

    TreeNode _treeNode;
    List<TreeNode> _list = new List<TreeNode>();

    [SerializeField]
    GameObject buttonPrehab;

    private TreeNode _currentTree;
    private List<GameObject> _currentButton = new List<GameObject>();

    int treeNum = -1;

    [SerializeField]
    private TitleSelectManagement titleSelectManagement;

    [SerializeField]
    GameManagement gameManagement;


    private float time = 0;
    [SerializeField]
    private float max_time = 0.5f;
    private bool canSelect = true;

    public void Initialize()
    {
        _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", false);
        treeNum = 0;
        
        _currentButton[treeNum].GetComponent<Button>().Select();
        _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", true);
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameObject.activeSelf) 
        {
            if (gameManagement.state == GameManagement.GameState.play)
            {

                if (context.performed)
                {
                    Vector2 dir = context.ReadValue<Vector2>();

                    if (treeNum == -1)
                    {
                        treeNum = 0;
                        _currentButton[treeNum].GetComponent<Button>().Select();
                        _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", true);
                        GetComponent<AudioSource>().Play();
                    }
                    else
                    {
                        int first_num = treeNum;

                        if (dir.y > 0)
                        {
                            _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", false);
                            treeNum--;
                            Debug.Log(treeNum);
                            treeNum += _currentButton.Count;
                            treeNum = treeNum % _currentButton.Count;
                            _currentButton[treeNum].GetComponent<Button>().Select();
                            _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", true);
                        }
                        else if (dir.y < 0)
                        {
                            _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", false);
                            treeNum++;
                            treeNum = treeNum % _currentButton.Count;
                            _currentButton[treeNum].GetComponent<Button>().Select();
                            _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", true);
                        }


                        if (first_num != treeNum)
                        {
                            GetComponent<AudioSource>().Play();
                        }
                    }
                }
            }
        }
    }

    public void OnDecide(int num)
    {
        if (treeNum == num)
        {

                Debug.Log(treeNum);
            if (_currentTree.Trees[treeNum].node_type.Equals("0"))
            {
                ChangeTree();
                ResetButton();
                _currentButton[0].GetComponent<Button>().Select();
                _currentButton[0].GetComponent<Animator>().SetBool("Select",true);
            }
            else if (_currentTree.Trees[treeNum].node_type.Equals("1"))
            {
                _currentTree.Trees[treeNum].TransNode(gameManagement);
            }
            else if (_currentTree.Trees[treeNum].node_type.Equals("2"))
            {
                _currentTree.Trees[treeNum].TransNode(gameManagement);
            }
        }
        else
        {
            if (treeNum != -1)
            {
                _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", false);
            }
            treeNum = num;
            _currentButton[treeNum].GetComponent<Button>().Select();
            _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", true);
            GetComponent<AudioSource>().Play();
        }

    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFire();
        }
    }

    public void OnFire()
    {
        if (gameObject.activeSelf)
        {
            if (gameManagement.state == GameManagement.GameState.play)
            {
                if (canSelect)
                {
                    if (treeNum == -1)
                    {
                        treeNum = 0;
                        _currentButton[treeNum].GetComponent<Button>().Select();
                        _currentButton[treeNum].GetComponent<Animator>().SetBool("Select", true);
                    }
                    else
                    {
                        Debug.Log(_currentTree.Text);

                        if (_currentTree.Trees[treeNum].node_type.Equals("0"))
                        {
                            ChangeTree();
                            ResetButton();
                        }
                        else if (_currentTree.Trees[treeNum].node_type.Equals("1"))
                        {
                            
                            _currentTree.Trees[treeNum].TransNode(gameManagement);
                        }
                        else if (_currentTree.Trees[treeNum].node_type.Equals("2"))
                        {
                            
                            _currentTree.Trees[treeNum].TransNode(gameManagement);
                        }

                        canSelect = false;

                    }
                }
            }
        }
    }

    private void Start()
    {
        StringReader sr = new StringReader(node_csv.text);
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            line = line.Replace("\\n", "\n");
            //Debug.Log(line);
            string[] line_splits = line.Split(new char[] { ',' });

            _nodeData.Add(line_splits);
        }
        sr = new StringReader(trans_csv.text);
        while (sr.Peek() > -1) 
        {
            string line = sr.ReadLine();
            line = line.Replace("\\n", "\n");
            //Debug.Log(line);
            string[] line_splits = line.Split(new char[] { ',' });

            _transData.Add(line_splits);
        }

        _treeNode = new TextTreeNode();
        _treeNode.Label = _nodeData[1][0];
        _treeNode.Text = _nodeData[1][1];
        _treeNode.node_type = _nodeData[1][2];
        _list.Add(_treeNode);

        for (int i = 2; i < _nodeData.Count;i++)
        {
            if (_nodeData[i][2].Equals("0"))
            {
                TextTreeNode tr = new TextTreeNode();
                tr.Label = _nodeData[i][0];
                tr.Text = _nodeData[i][1];
                tr.node_type = _nodeData[i][2];
                _list.Add(tr);
            }
            else if (_nodeData[i][2].Equals("1"))
            {
                LoadSceneTreeNode tr = new LoadSceneTreeNode();
                tr.Label = _nodeData[i][0];
                tr.Text = _nodeData[i][1];
                tr.node_type = _nodeData[i][2];
                tr.bgm_num = int.Parse(_nodeData[i][3]);
                _list.Add(tr);
            }
            else if (_nodeData[i][2].Equals("2"))
            {
                ChangePanelNode tr = new ChangePanelNode();
                tr.Label = _nodeData[i][0];
                tr.Text = _nodeData[i][1];
                tr.node_type = _nodeData[i][2];
                tr.titleSelectManagement = titleSelectManagement;
                tr.panelNum = int.Parse(_nodeData[i][3]);
                _list.Add(tr);
            }

        }

        for (int i = 1; i < _transData.Count; i++)
        {
            string str1 = _transData[i][0];
            TreeNode tr2 = _list.Find(n => n.Label.Equals(_transData[i][1]));
            Debug.Log(str1+"=>"+tr2.Label);
            _treeNode.Insert(str1, tr2);
        }

        _currentTree = _treeNode;

        ResetButton();
    }

    private void Update()
    {
        if (!canSelect)
        {
            if(time > max_time)
            {
                canSelect = true;
                time = 0;

            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }

    public void ResetButton()
    {
        foreach (GameObject gb in _currentButton)
        {
            Destroy(gb);
        }
        _currentButton.Clear();
        /*foreach (TreeNode tn in currentTree.Trees)
        {
            GameObject gb = Instantiate(buttonPrehab,this.transform);
            
            gb.transform.Find("TMP").GetComponent<TextMeshProUGUI>().text = tn.Text;
            current_button.Add(gb);
            
        }*/
        for (int i = 0; i < _currentTree.Trees.Count; i++)
        {
            int num = i;
            GameObject gb = Instantiate(buttonPrehab, this.transform);

            gb.transform.Find("TMP").GetComponent<TextMeshProUGUI>().text = _currentTree.Trees[i].Text;
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { OnDecide(num); });
            gb.GetComponent<EventTrigger>().triggers.Add(entry);
            _currentButton.Add(gb);
        }
        //current_button[0].GetComponent<Animator>().SetBool("Select", true);
        
    }

    

    public void ChangeTree()
    {
        
        _currentTree = _currentTree.Trees[treeNum];
        treeNum = 0;
    }
    
}

public abstract class TreeNode
{
    
    protected string label;

    public string Label { get { return label; } set { label = value; } }

    protected List<TreeNode> trees = new List<TreeNode>();

    public List<TreeNode> Trees { get { return trees; } }

    protected string text;
    public string Text { get { return text; } set { text = value; } }

    public string node_type;


    public TreeNode Check(string str)
    {
        if (this.label.Equals(str))
        {
            return this;
        }
        foreach (TreeNode textTree in trees)
        {
            TreeNode true_tree = textTree.Check(str);
            if (!(true_tree.Equals(null)))
            {
                return true_tree;
            }
        }
        return null;
    }

    public void Insert(string str, TreeNode insert_tree)
    {

        TreeNode buffer = Check(str);
        if (!buffer.Equals(null)) {
            buffer.trees.Add(insert_tree);
        }
        else
        {

        }
    }

    public abstract TreeNode TransNode(GameManagement gameManagement);
}

public class TextTreeNode : TreeNode
{
    public override TreeNode TransNode(GameManagement gameManagement)
    {
        return this;
    }
}

public abstract class MethodTreeNode : TreeNode
{
    

    public override TreeNode TransNode(GameManagement gameManagement)
    {
        Method(gameManagement);
        return null;
    }

    protected abstract void Method(GameManagement gameManagement);
}

public class LoadSceneTreeNode : MethodTreeNode
{
    public int bgm_num = 0;

    protected override void Method(GameManagement gameManagement)
    {
        gameManagement.OnSceneLoadInstance(this.label,LoadSceneMode.Single,true,bgm_num);
    }
}

public class ChangePanelNode : MethodTreeNode
{
    public TitleSelectManagement titleSelectManagement;
    public int panelNum;

    protected override void Method(GameManagement gameManagement)
    {
        titleSelectManagement.Select(panelNum);
    }
}
