using UnityEngine;

public class UIInteface : MonoBehaviour
{
    public GameObject turret;
    GameObject focusObj;

    void Update()
    {
        bool inputBegan = IsInputBegan();
        bool inputHeld = IsInputHeld();
        bool inputEnded = IsInputEnded();
        Vector3 inputPosition = GetInputPosition();

        if(inputBegan)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                focusObj = Instantiate(turret, hit.point, turret.transform.rotation);
                focusObj.GetComponent<Collider>().enabled = false;
            }
        }
        else if(inputHeld && focusObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                focusObj.transform.position = hit.point + new Vector3(0, 1, 0);
            }
        }
        else if(inputEnded && focusObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject.CompareTag("Platform") &&
                hit.normal.Equals(new Vector3(0,1,0)))
            {
                hit.collider.gameObject.tag = "Occupied";
                focusObj.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, focusObj.transform.position.y, hit.collider.gameObject.transform.position.z);
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