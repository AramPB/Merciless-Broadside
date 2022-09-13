using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class MinimapController : MonoBehaviour
{
    private int layerMask;
    private Camera cam;
    private Camera thisCam;

    [SerializeField]
    private GameObject water;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Base");
        cam = Camera.main;
        thisCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        Vector3 position = new Vector3();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity , layerMask))
        {
            position = hit.point;
        }
        position.y = transform.position.y;

        thisCam.orthographicSize = cam.transform.position.y;

        transform.position = position;

        transform.rotation = Quaternion.Euler(90f, cam.transform.eulerAngles.y, 0f);

        water.transform.position = new Vector3(position.x, 0, position.z);
    }
}
