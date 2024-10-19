using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LoginUI : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, forgotPasswordPanel, startupPanel, firstStartupPanel, notifPanel;
    public InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUsername, forgotPassWord, forgotCPassWord;
    public Toggle loginPasswordToggle, signupPasswordToggle, forgotPasswordToggle, signupCPasswordToggle, forgotCPasswordToggle;
    public Text notif_Title_Text, notif_Message_Text;

    private SceneManagerScript sceneManager;

    void Start()
    {
        // Get the SceneManagerScript component
        sceneManager = FindObjectOfType<SceneManagerScript>();

        OpenFirstStartupPanel(); // Starts the app on the first startup panel
        
        // Passwords hidden on startup
        TogglePasswordVisibility(loginPassword, false);
        TogglePasswordVisibility(signupPassword, false);
        TogglePasswordVisibility(signupCPassword, false);
        TogglePasswordVisibility(forgotPassWord, false);
        TogglePasswordVisibility(forgotCPassWord, false);
    }

    // Toggle visibility of password for any input field
    public void TogglePasswordVisibility(InputField passwordField, bool showPassword)
    {
        if (showPassword)
        {
            passwordField.contentType = InputField.ContentType.Standard; 
        }
        else
        {
            passwordField.contentType = InputField.ContentType.Password; 
        }
        passwordField.ForceLabelUpdate();
    }

    // Toggle logic methods
    public void OnLoginPasswordToggleChanged()
    {
        TogglePasswordVisibility(loginPassword, loginPasswordToggle.isOn);
    }

    public void OnSignupPasswordToggleChanged()
    {
        TogglePasswordVisibility(signupPassword, signupPasswordToggle.isOn);
        TogglePasswordVisibility(signupCPassword, signupPasswordToggle.isOn);
    }

    public void OnForgotPasswordToggleChanged()
    {
        TogglePasswordVisibility(forgotPassWord, forgotPasswordToggle.isOn);
        TogglePasswordVisibility(forgotCPassWord, forgotPasswordToggle.isOn);
    }


    // Opens the startup panel and hides the others
    public void OpenStartupPanel()
    {
        startupPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false); 
    }

    // Opens the first startup panel and hides the others
    public void OpenFirstStartupPanel()
    {
        firstStartupPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        notifPanel.SetActive(false);  
    }

    // Opens the login panel and hides the others
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  
    }

    // Opens the sign-up panel and hides the others
    public void OpenSignUpPanel()
    {
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  
    }

    // Opens the forgot password panel and hides the others
    public void OpenForgotPassPanel()
    {
        forgotPasswordPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  
    }

    // Check if any input field is empty and display an error message
    public void ValidateLoginFields()
    {
        HideNotification();
        
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            DisplayNotification("Error", "Please fill in all login fields.");
        }
        else if (!IsValidEmail(loginEmail.text))
        {
            DisplayNotification("Error", "Please enter a valid email address.");
        }
        else
        {
            // Call SceneManager's LoadScene method
            sceneManager.LoadScene("Main Menu");  
        }
    }

    // Validation Check for Register Panel
    public void ValidateSignUpFields()
    {
        if (string.IsNullOrEmpty(signupEmail.text) || 
            string.IsNullOrEmpty(signupPassword.text) || 
            string.IsNullOrEmpty(signupCPassword.text) || 
            string.IsNullOrEmpty(signupUsername.text))
        {
            DisplayNotification("Error", "Please fill in all sign-up fields."); // Checks if user has filled out all sign in fields
        }
        else if (!IsValidEmail(signupEmail.text))
        {
            DisplayNotification("Error", "Please enter a valid email address."); // Checks if user has put a valid email
        }
        else if (!IsValidPassword(signupPassword.text)) // Password strength check
        {
            DisplayNotification("Error", "Password must be at least 6 characters long, contain at least 1 uppercase letter, 1 lowercase letter, and 1 symbol."); 
        }
        else if (signupPassword.text != signupCPassword.text)
        {
            DisplayNotification("Error", "Passwords do not match."); // If passwords don't match
        }
        else if (!IsUsernameLengthValid(signupUsername.text)) 
        {
            DisplayNotification("Error", "Username must be between 3 and 15 characters long."); // Checks if username is too short or long
        }
        else if (!IsUsernameCharactersValid(signupUsername.text)) 
        {
            DisplayNotification("Error", "Username can only contain letters, underscores, and full stops."); // Checks if the username has invalid characters  
        }
        else
        {
            // Call SceneManager's LoadScene method
            sceneManager.LoadScene("PickStarter");  // Goes to the select starter screen
        }
    }

    // Validation check for Forgot Password panel
    public void ValidateForgotPasswordFields()
    {
        if (string.IsNullOrEmpty(forgotPassWord.text) || string.IsNullOrEmpty(forgotCPassWord.text))
        {
            DisplayNotification("Error", "Please fill in all password fields.");
        }
        else if (forgotPassWord.text != forgotCPassWord.text)
        {
            DisplayNotification("Error", "Passwords do not match.");
        }
        else
        {
            OpenLoginPanel();
        }
    }

    // Validate if the username length is between 3 and 15 characters
    private bool IsUsernameLengthValid(string username)
    {
        return username.Length >= 3 && username.Length <= 15;
    }

    // Validate if the username contains only letters, full stops, and underscores
    private bool IsUsernameCharactersValid(string username)
    {
        string usernamePattern = @"^[a-zA-Z._]+$";
        return Regex.IsMatch(username, usernamePattern);
    }

    private bool IsValidPassword(string password)
    {
        // Password must be at least 6 characters long, contain at least one uppercase letter, 
        // one lowercase letter, one digit or symbol.
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9\W]).{6,}$";
        return Regex.IsMatch(password, passwordPattern);
    }

    // Validates email format
    private bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Displays a notification with a title and message
    public void DisplayNotification(string title, string message)
    {
        notifPanel.SetActive(true);
        notif_Title_Text.text = title;
        notif_Message_Text.text = message;
    }

    // Hides notification panel
    public void HideNotification()
    {
        notifPanel.SetActive(false);
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";
    }

    
}
