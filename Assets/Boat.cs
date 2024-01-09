using UnityEngine;

public class Boat : MonoBehaviour
{
    private Rigidbody2D rb;
    public float constantAcceleration = 0.2f;  // Constant acceleration when a key is pressed
    public float maxDeceleration = 1f;  // Maximum deceleration when stamina is 0
    private float currentDeceleration;
    public float maxSpeed = 5f;  // Theoretical maximum speed
    private float currentSpeed = 0f;
    private bool lastKeyPressedLeft = false;

    // Stamina variables
    public float maxStamina = 100f;
    private float currentStamina;

    // Stamina depletion and refill rates
    public float staminaDepletionRate = 10f;
    public float baseRefillRate = 5f;

    public delegate void BoatStateHandler(float speed, float stamina);
    public static event BoatStateHandler OnBoatStateUpdated;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleInput();
        RefillStamina();
        Debug.Log("Invoking OnBoatStateUpdated with Speed: " + currentSpeed + ", Stamina: " + currentStamina);
        OnBoatStateUpdated?.Invoke(currentSpeed, currentStamina);
    }

    void FixedUpdate()
    {
        currentDeceleration = CalculateDeceleration(currentStamina);
        if (currentSpeed > 0 && !Input.anyKey)
        {
            currentSpeed -= currentDeceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    void HandleInput()
    {
        if (currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !lastKeyPressedLeft)
            {
                IncreaseSpeed();
                lastKeyPressedLeft = true;
                DecreaseStamina();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && lastKeyPressedLeft)
            {
                IncreaseSpeed();
                lastKeyPressedLeft = false;
                DecreaseStamina();
            }
        }
    }

    void IncreaseSpeed()
    {
        // Increase the boat's speed based on a constant acceleration
        currentSpeed = Mathf.Min(currentSpeed + constantAcceleration, maxSpeed);
    }

    void DecreaseStamina()
    {
        currentStamina -= staminaDepletionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    void RefillStamina()
    {
        currentStamina += baseRefillRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }
    float CalculateDeceleration(float stamina)
    {
        // Example of a smoother deceleration curve
        // Adjust the curve to fit the desired gameplay feel
        float normalizedStamina = stamina / maxStamina;
        return maxDeceleration * (1 - Mathf.Pow(normalizedStamina, 1.5f));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            // Direct method call approach
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.FinishTimer();
            }
        }
    }

    
}
