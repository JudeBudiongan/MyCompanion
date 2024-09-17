using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, forgotPasswordPanel, startupPanel, firstStartupPanel, notifPanel;
    public InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUsername, forgotPassWord, forgotCPassWord;
    public Text notif_Title_Text, notif_Message_Text;

    private SceneManagerScript sceneManager;

    // Opens the startup panel and hides the others
    public void OpenStartupPanel()
    {
        startupPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  // Hide notification panel
    }

    // Opens the first startup panel and hides the others
    public void OpenFirstStartupPanel()
    {
        firstStartupPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        notifPanel.SetActive(false);  // Hide notification panel
    }

    // Opens the login panel and hides the others
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  // Hide notification panel
    }

    // Opens the sign-up panel and hides the others
    public void OpenSignUpPanel()
    {
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  // Hide notification panel
    }

    // Opens the forgot password panel and hides the others
    public void OpenForgotPassPanel()
    {
        forgotPasswordPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);  // Hide notification panel
    }

    // Check if any input field is empty and display an error message
    public void ValidateLoginFields()
    {
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

    public void ValidateSignUpFields()
    {
        if (string.IsNullOrEmpty(signupEmail.text) || 
            string.IsNullOrEmpty(signupPassword.text) || 
            string.IsNullOrEmpty(signupCPassword.text) || 
            string.IsNullOrEmpty(signupUsername.text))
        {
            DisplayNotification("Error", "Please fill in all sign-up fields.");
        }
        else if (!IsValidEmail(signupEmail.text))
        {
            DisplayNotification("Error", "Please enter a valid email address.");
        }
        else if (signupPassword.text != signupCPassword.text)
        {
            DisplayNotification("Error", "Passwords do not match.");
        }
        else
        {
            // Call SceneManager's LoadScene method
            sceneManager.LoadScene("PickStarter");  
        }
    }

    public void ValidateForgotPasswordFields()
    {
        if (string.IsNullOrEmpty(forgotPassWord.text) || string.IsNullOrEmpty(forgotCPassWord.text))
        {
            DisplayNotification("Error", "Please fill in all forgot password fields.");
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

    // Displays a notification with a title and message
    public void DisplayNotification(string title, string message)
    {
        notifPanel.SetActive(true);
        notif_Title_Text.text = title;
        notif_Message_Text.text = message;
    }

    public void HideNotification()
    {
        notifPanel.SetActive(false);
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";
    }

   private bool IsValidEmail(string email)
    {
        
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
    }
    void Start()
    {
        // Get the SceneManagerScript component
        sceneManager = FindObjectOfType<SceneManagerScript>();

        OpenFirstStartupPanel(); // Starts the app on the first startup panel
    }
}
