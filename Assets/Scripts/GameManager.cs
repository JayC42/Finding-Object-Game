using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HiddenObjectData
{
    public string name;
    public GameObject hiddenObj;
    public bool makeHidden = false;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    private Item itemBeingPickedUp; 
    //[SerializeField] private float timeLimit = 0;                       //Total time for single game player gets
    [SerializeField] private int maxHiddenObjectToBeFound = 6;            //maximum hidden objects available in the scene
    //[SerializeField] private ObjectHolder objectHolderPrefab;           //ObjectHolderPrefab contains list of all the hiddenObjects available in it

    //[HideInInspector] public GameStatus gameStatus = GameStatus.NEXT;   //enum to keep track of game status
    [SerializeField] private List<HiddenObjectData> hiddenObjectList;
    private List<HiddenObjectData> activeHiddenObjectList;              //list hidden objects which are marked as hidden from the above list
    private int totalHiddenObjectsFound = 0;                            //int to keep track of hidden objects found
    //private float currentTime;                                          //float to keep track of time remaining
    //private TimeSpan time;                                              //variable to help convert currentTime into time format
    //private RaycastHit2D hit;
    //private Vector3 pos;                                                //hold Mouse Tap position converted to WorldPoint

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        activeHiddenObjectList = new List<HiddenObjectData>();          //we initialize the list
        AssignHiddenObjects();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectHiddenObject(); 
        }
    }
    void AssignHiddenObjects()  //Method select objects from the hiddenobjects list which should be hidden
    {
        totalHiddenObjectsFound = 0; 
        activeHiddenObjectList.Clear();                        // Clear the list to remove any residual objects still in it 
        for (int i = 0; i < hiddenObjectList.Count; i++)       //loop through all the hiddenObjects in the hiddenObjectList
        {
            //deacivate collider, as we only want selected hidden objects to have collider active
            hiddenObjectList[i].hiddenObj.GetComponent<Collider>().enabled = false;
        }

        int k = 0; //int to keep count

        while (k < maxHiddenObjectToBeFound) //we check while k is less that maxHiddenObjectToFound, keep looping
        {
            //we randomly select any number between 0 to hiddenObjectList.Count
            int randomVal = Random.Range(0, hiddenObjectList.Count);
            
            //then we check is the makeHidden bool of that hiddenObject is false
            if (!hiddenObjectList[randomVal].makeHidden)
            {
                //We are setting the object name similar to index, because we are going to use index to identify the tapped object
                //and this index will help us to deactivate the hidden object icon from the UI
                //hiddenObjectList[randomVal].hiddenObj.name = "" + k;                            
                UIManager.instance.SetText(hiddenObjectList[randomVal].hiddenObj.name);         //set their name on to-find-list-UI
                hiddenObjectList[randomVal].makeHidden = true;                                  //if false, then we set it to true                                              
                hiddenObjectList[randomVal].hiddenObj.GetComponent<Collider>().enabled = true;  //activate its collider, so we can detect it on tap
                activeHiddenObjectList.Add(hiddenObjectList[randomVal]);                        //add the hidden object to the activeHiddenObjectList
                k++;                                                                            //and increase the count
            }
        }
    }
    void SelectHiddenObject()
    {
        Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 2f, layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<Item>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
                hitItem.gameObject.SetActive(false);
                
                for (int i = 0; i < activeHiddenObjectList.Count; i++)
                {
                    if (activeHiddenObjectList[i].hiddenObj.name == hitItem.gameObject.name)
                    {
                        activeHiddenObjectList.RemoveAt(i);
                        break;
                    }

                    totalHiddenObjectsFound++; 

                    if (totalHiddenObjectsFound >= maxHiddenObjectToBeFound)
                    {
                        Debug.Log("All Items Found!");
                    }
                }
                //Debug.Log("Found: " + hitItem.gameObject.name);
                
            }
        }
        else
        {
            itemBeingPickedUp = null;
        }

    }

}
