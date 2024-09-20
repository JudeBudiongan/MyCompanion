using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PasswordStrengthMeter : MonoBehaviour
{
    public InputField passwordInputField; // The input field where the user types their password
    public Slider strengthSlider; // Slider to visually show the strength of the password
    public Text strengthText; // Text to display feedback on password strength
    public Image fillImage; // Image component for the Slider's fill area to change color
    public Image handleImage; // The Image component for the Slider's handle
    public Sprite[] handleSprites; // Array of sprites for different handle images based on strength level

    private void Start()
    {
        // Add a listener to the password input field to call the UpdateStrength method when the text changes
        passwordInputField.onValueChanged.AddListener(UpdateStrength);
    }

    private void UpdateStrength(string password)
    {
        // Calculate the strength of the password
        float strength = CalculatePasswordStrength(password);

        // Update the strength slider value
        strengthSlider.value = strength;

        // Update the strength text based on the calculated strength
        strengthText.text = GetStrengthDescription(strength);

        // Change the color of the slider fill based on the strength
        UpdateSliderColor(strength);

        // Change the handle image based on the strength level
        UpdateHandleImage(strength);
    }

    private float CalculatePasswordStrength(string password)
    {
        int score = 0;

        // Strict conditions for password length (minimum 6 characters for any score)
        if (password.Length >= 6) score += 1;
        if (password.Length >= 8) score += 1;
        if (password.Length >= 12) score += 1;

        // Add points if the password meets basic security requirements (uppercase, lowercase, symbol/digit)
        if (Regex.IsMatch(password, @"[a-z]")) score += 1; // Contains lowercase
        if (Regex.IsMatch(password, @"[A-Z]")) score += 1; // Contains uppercase
        if (Regex.IsMatch(password, @"[0-9]")) score += 1; // Contains digits
        if (Regex.IsMatch(password, @"[\W_]")) score += 1; // Contains symbols or underscores

        // Final validation for password strength based on all criteria
        if (IsValidPassword(password))
        {
            score += 1; // If the password meets the final strong password criteria, give it a final score boost
        }

        // Return strength as a value between 0 and 1 for the slider
        return Mathf.Clamp01(score / 6f); // Updated to reflect stricter scoring (max score is now 6)
    }

    private bool IsValidPassword(string password)
    {
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9\W]).{6,}$";
        return Regex.IsMatch(password, passwordPattern);
    }

    private string GetStrengthDescription(float strength)
    {
        if (strength < 0.25f) return "Weak";
        if (strength < 0.50f) return "Moderate";
        if (strength < 0.75f) return "Strong";
        return "Very Strong";
    }

    private void UpdateSliderColor(float strength)
    {
        if (strength < 0.25f)
        {
            fillImage.color = Color.red; // Weak
        }
        else if (strength < 0.50f)
        {
            fillImage.color = Color.yellow; // Moderate
        }
        else if (strength < 0.75f)
        {
            fillImage.color = new Color(1f, 0.64f, 0f); // Orange for Strong
        }
        else
        {
            fillImage.color = Color.green; // Very Strong
        }
    }

    private void UpdateHandleImage(float strength)
    {
        int index = Mathf.FloorToInt(strength * (handleSprites.Length - 1)); // Determine the index based on strength
        handleImage.sprite = handleSprites[index]; // Set the handle image
    }
}
