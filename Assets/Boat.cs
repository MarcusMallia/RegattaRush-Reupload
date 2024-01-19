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
    private bool canMove = false;
    public float passiveDeceleration = 0.1f;
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
        if (canMove)
        {
            HandleInput();
            RefillStamina();

            // Calculate deceleration for logging
            float currentDeceleration = CalculateDeceleration(currentStamina);

            // Log current speed, stamina, and deceleration
            Debug.Log("Speed: " + currentSpeed + ", Stamina: " + currentStamina + ", Deceleration: " + currentDeceleration);

            OnBoatStateUpdated?.Invoke(currentSpeed, currentStamina);
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
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
        
        float currentDynamicDeceleration = CalculateDeceleration(currentStamina);
    
        if (!Input.anyKey)
        {
            ApplyPassiveDeceleration();
        }
        else
        {
            // Apply dynamic deceleration during player input
            currentSpeed -= currentDynamicDeceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Max(currentSpeed, 0);  // Ensure speed doesn't go negative
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }
    void ApplyPassiveDeceleration()
    {
        // Apply passive deceleration when there is no player input
        currentSpeed -= passiveDeceleration * Time.deltaTime;
    }
    
    public void EnableMovement()
    {
        canMove = true;
    }
    
    public void DisableMovement()
    {
        canMove = false;
    }

    void HandleInput()
    {
        float resistance = CalculateResistance(currentStamina);

        // Apply resistance even during acceleration
        float effectiveAcceleration = constantAcceleration * (1 - resistance);
        
        if (currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = true;
                DecreaseStamina();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && lastKeyPressedLeft)
            {
                IncreaseSpeed(effectiveAcceleration);
                lastKeyPressedLeft = false;
                DecreaseStamina();
            }
        }
    }
    float CalculateResistance(float currentStamina)
    {
        // Example: Inversely proportional to stamina
        return (1 - currentStamina / maxStamina) * maxDeceleration;
    }
    void IncreaseSpeed(float acceleration)
    {
        currentSpeed += acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void DecreaseStamina()
    {
        currentStamina -= staminaDepletionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    void RefillStamina()
    {
        float speedFactor = 1 - (currentSpeed / maxSpeed);  // Inverse relation to speed
        float modifiedRefillRate = baseRefillRate * speedFactor;

        currentStamina += modifiedRefillRate * Time.deltaTime;
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