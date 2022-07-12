using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    public GameObject pointer;
    public float flySpeed = 5f;
    public float yawSpeed = 120;

    public GameObject partPrefab;
    public List<GameObject> bodyParts = new List<GameObject>();
    public List<Vector3> pastPosition = new List<Vector3>();
    int gap = 10;
    public float bodySpeed = 5;

    float yaw;
    void Start()
    {
        GrowPlane();
        GrowPlane();
        GrowPlane();
    }

    // Update is called once per frame
    void Update()
    {
        pointer.transform.position = transform.position;
        transform.position +=  transform.forward * flySpeed * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        yaw += horizontal * yawSpeed * Time.deltaTime;
        float pitch = Mathf.Lerp(0, 30, Mathf.Abs(vertical)) * Mathf.Sign(vertical);
        float roll = Mathf.Lerp(0, 40, Mathf.Abs(horizontal)) * -Mathf.Sign(horizontal);

        transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);

        pastPosition.Insert(0, transform.position);
        int index = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = pastPosition[Mathf.Min(index * gap, pastPosition.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }
    }

    void GrowPlane()
    {
        GameObject part = Instantiate(partPrefab);
        bodyParts.Add(part);
    }
}
