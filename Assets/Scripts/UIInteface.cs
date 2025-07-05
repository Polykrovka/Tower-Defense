using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class UIInteface:MonoBehaviour
{
    public GameObject RocketLauncherTurret;
    public GameObject GatlingTurret;
    public GameObject flamerTurret;
    public GameObject turretMenu;

    GameObject itemPrefab;
    GameObject focusObj;

    public void CreteRocketLauncher()
    {
        itemPrefab = RocketLauncherTurret;
        CreateItemForButton();
    }

    public void CreateGatling()
    {
        itemPrefab = GatlingTurret;
        CreateItemForButton();
    }

    public void CreateFlamer()
    {
        itemPrefab = flamerTurret;
        CreateItemForButton();
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
    }

    void CreateItemForButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetInputPosition());
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            focusObj = Instantiate(itemPrefab, hit.point, itemPrefab.transform.rotation);
            focusObj.GetComponent<Collider>().enabled = false;
        }
    }
    void Update()
    {
        bool inputBegan = IsInputBegan();
        bool inputHeld = IsInputHeld();
        bool inputEnded = IsInputEnded();
        Vector3 inputPosition = GetInputPosition();

        if(inputBegan)
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject.CompareTag("Turret"))
            {
                turretMenu.transform.position = inputPosition;
                turretMenu.SetActive(true);
            }
        }
        else if(inputHeld && focusObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                focusObj.transform.position = hit.point;
            }
        }
        else if(inputEnded && focusObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject.CompareTag("Platform") &&
                hit.normal.Equals(new Vector3(0, 1, 0)))
            {
                hit.collider.gameObject.tag = "Occupied";
                focusObj.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, focusObj.transform.position.y, hit.collider.gameObject.transform.position.z);
                focusObj.GetComponent<Collider>().enabled = true;
            }
            else
            {
                Destroy(focusObj);
            }

            focusObj = null;
        }
    }
    bool IsInputBegan()
    {
        if(Input.touchCount == 1)
            return Input.GetTouch(0).phase == TouchPhase.Began;
        return Input.GetMouseButtonDown(0);
    }

    bool IsInputHeld()
    {
        if(Input.touchCount == 1)
            return Input.GetTouch(0).phase == TouchPhase.Moved;
        return Input.GetMouseButton(0);
    }

    bool IsInputEnded()
    {
        if(Input.touchCount == 1)
        {
            var phase = Input.GetTouch(0).phase;
            return phase == TouchPhase.Ended || phase == TouchPhase.Canceled;
        }
        return Input.GetMouseButtonUp(0);
    }

    Vector3 GetInputPosition()
    {
        if(Input.touchCount == 1)
            return Input.GetTouch(0).position;
        return Input.mousePosition;
    }
}