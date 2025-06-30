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
        if(Input.GetMouseButtonDown(0))
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
        else if(Input.GetMouseButton(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!Physics.Raycast(ray, out hit) || focusObj == null)
            {
                return;
            }

            focusObj.transform.position = hit.point + new Vector3(0, 1, 0);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit) &&
                hit.collider.gameObject.CompareTag("Platform"))
            {
                hit.collider.gameObject.tag = "Occupied";
            }
            else
            {
                Destroy(focusObj);
            }

        }
    }
}
