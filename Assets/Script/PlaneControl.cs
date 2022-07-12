using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    public GameObject pointer;
    public float flySpeed = 5f;
    public float yawSpeed = 120;

    public float BodySpeed = 5;
    public int Gap = 10;
    public GameObject BodyPrefab;
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();
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
        transform.position += transform.forward * flySpeed * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        yaw += horizontal * yawSpeed * Time.deltaTime;
        float pitch = Mathf.Lerp(0, 30, Mathf.Abs(vertical)) * Mathf.Sign(vertical);
        float roll = Mathf.Lerp(0, 40, Mathf.Abs(horizontal)) * -Mathf.Sign(horizontal);

        transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);

        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }
    }

    void GrowPlane()
    {
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }
}
