using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Target : MonoBehaviour
{
    public int pointValue;
    public ParticleSystem[] explosionParticle;

    private Rigidbody targetRb;
    private GameManager gameManager;

    private float minSpeed = 12;
    private float maxSpeed = 14;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -2;

    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(Vector3.up * Random.Range(minSpeed, maxSpeed), ForceMode.Impulse);
        targetRb.AddTorque(Random.Range(-maxTorque, maxTorque), Random.Range(-maxTorque, maxTorque),
        Random.Range(-maxTorque, maxTorque), ForceMode.Impulse);
        transform.position = new(Random.Range(-xRange, xRange), ySpawnPos);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && gameManager.isGameActive)
        {
            Debug.Log("Mouse was clicked.");
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    Destroy(gameObject);
                    ParticleSystem particle = explosionParticle[Random.Range(0, explosionParticle.Length)];
                    Instantiate(particle, transform.position, particle.transform.rotation);
                    gameManager.UpdateScore(pointValue);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit something");
        if (other.CompareTag("DestroyZone"))
        {
            Destroy(gameObject);
            if (!gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }
        }
    }
}
