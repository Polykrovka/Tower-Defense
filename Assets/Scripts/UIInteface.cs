using UnityEngine;

public class UIInteface : MonoBehaviour
{
    public GameObject turret;
    GameObject focusObj;
    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray, out hit))
            {
                return;
            }

            focusObj = Instantiate(turret, hit.point, turret.transform.rotation);
            focusObj.GetComponent<Collider>().enabled = false;

        }
        else if(Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray, out hit) || !focusObj)
            {
                return;
            }

            focusObj.transform.position = hit.point + new Vector3(0, 1, 0);
        }
        else if(focusObj && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit) &&
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
}
