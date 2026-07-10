using UnityEngine;

public class PropellerControllerX : MonoBehaviour
{
    public float rotationSpeed;

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
