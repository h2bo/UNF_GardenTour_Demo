using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManagerScript : MonoBehaviour
{
    private float previousMouseX = 0f;
    private float previousMouseY = 0f;
    public GameObject theCamera;
    public GameObject activeList;
    public Material nodeMat;

    private bool increasing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoClick();
        HandleRotation();
        ModulateNodeMaterial();
    }

    private void ModulateNodeMaterial()
    {
        Color cc = nodeMat.color;
        Color newColor = new Color(cc.r, cc.g, cc.b, cc.a);

        if (increasing)
        {

            if (newColor.a >= 1)
                increasing = false;
            else
                newColor.a += (.5f * Time.smoothDeltaTime);
        }
        else
        {
            if (newColor.a <= 0f)
                increasing = true;
            else
                newColor.a -= (.5f * Time.smoothDeltaTime);
        }

        nodeMat.color = newColor;
    }


    private void HandleRotation()
    {
        float currMouseX = Input.mousePosition.x;
        float currMouseY = Input.mousePosition.y;

        float diffX = currMouseX - previousMouseX;
        float diffY = currMouseY - previousMouseY;

        theCamera.transform.Rotate(new Vector3((-1 * diffY), diffX, 0));
        Vector3 newRot = theCamera.transform.rotation.eulerAngles;

        newRot.z = 0;

        theCamera.transform.eulerAngles = newRot;

        previousMouseX = currMouseX;
        previousMouseY = currMouseY;
    }


    private void DoClick()
    {
        //if(OVRInput.GetDown(OVRInput.Axis1D...)....
        if (Input.GetMouseButtonDown(0))
        {
            // rightController.transform.position
            // rightController.transform.rotation
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                Material theStoredMaterial = hit.collider.gameObject.GetComponent<NodeLoadingScript>().myMaterial;
                RenderSettings.skybox = theStoredMaterial;

                GameObject theNodeList = hit.collider.gameObject.GetComponent<NodeLoadingScript>().myNodeList;
                activeList.SetActive(false);
                theNodeList.SetActive(true);
                activeList = theNodeList;
            }
        }
    }

}
