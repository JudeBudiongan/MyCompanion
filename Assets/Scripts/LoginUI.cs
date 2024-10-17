using System.Text.RegularExpressions;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoginUI : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, forgotPasswordPanel, startupPanel, firstStartupPanel, notifPanel;
    public InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUsername, forgotPassWord;
    public Toggle loginPasswordToggle, signupPasswordToggle, forgotPasswordToggle, signupCPasswordToggle, forgotCPasswordToggle;
    public Text notif_Title_Text, notif_Message_Text;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser Userauth;
    private DatabaseReference databaseReference;

    private SceneManagerScript sceneManager;

    [System.Serializable]
    public class User
    {
        public string Email;
        public string Username;
        public string Password; // Store this securely (e.g., hashed) in a real application
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth and Database");
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;

    }

    public void LoginButton()
    {
        StartCoroutine(Login(loginEmail.text, loginPassword.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(signupEmail.text, signupPassword.text, signupUsername.text));
    }

   private IEnumerator Login(string _email, string _password)
{
    var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
    
    // Wait until the task is completed
    yield return new WaitUntil(() => LoginTask.IsCompleted);

    try
    {
        // Check for exceptions after yielding
        if (LoginTask.Exception != null)
        {
            // Extract the Firebase error
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login failed. Please try again.";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Please enter your email.";
                    break;
                case AuthError.MissingPassword:
                    message = "Please enter your password.";
                    break;
                case AuthError.InvalidEmail:
                    message = "The email address is invalid.";
                    break;
                case AuthError.UserNotFound:
                    message = "No account found with this email.";
                    break;
                case AuthError.WrongPassword:
                    message = "Incorrect password. Please try again.";
                    break;
                default:
                    // Log the exception details for debugging
                    Debug.LogError($"Login failed: {LoginTask.Exception.Message}");
                    break;
            }

            // Display the notification to the user
            DisplayNotification("Error", message);
            yield break; // Stop the coroutine if login fails
        }

        // If login is successful, proceed
        Debug.Log("Login successful!");
        sceneManager.LoadScene("Main Menu");
        // Proceed to the main menu or next step
    }
    catch (Exception e)
    {
        // Catch any unexpected exceptions
        Debug.LogError($"An error occurred during login: {e.Message}");
        DisplayNotification("Error", "An unexpected error occurred. Please try again.");
    }
}



    private IEnumerator Register(string _email, string _password, string _username)
    {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(() => RegisterTask.IsCompleted);

        Userauth = RegisterTask.Result.User;
        Debug.Log($"Successfully registered: {Userauth.Email}");

        UserProfile profile = new UserProfile { DisplayName = _username };
        var ProfileTask = Userauth.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(() => ProfileTask.IsCompleted);

        if (ProfileTask.Exception == null)
        {
            RegisterUser(_email, _username, _password);
            sceneManager.LoadScene("PickStarter");
        }
        
    }

    private void RegisterUser(string email, string username, string password)
    {
        User newUser = new User { Email = email, Username = username, Password = password };
        string json = JsonUtility.ToJson(newUser);
        string userId = databaseReference.Child("users").Push().Key;
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            DisplayNotification(task.IsCompleted ? "Success" : "Error", task.IsCompleted ? "User registered successfully!" : "Registration failed. Please try again.");
            if (task.IsCompleted)
                OpenLoginPanel();
        });
    }

    public void OnForgotPasswordButtonClicked()
    {
        string email = forgotPassWord.text; // Replace with your actual input field
        StartCoroutine(ResetPassword(email));
    }


    private IEnumerator ResetPassword(string email)
{
    // Start the Firebase password reset task
    var resetTask = auth.SendPasswordResetEmailAsync(email);
    yield return new WaitUntil(() => resetTask.IsCompleted);

    // Check for errors
    if (resetTask.Exception != null)
    {
        Debug.LogError($"Password reset failed: {resetTask.Exception.Message}");
        DisplayNotification("Error", "Failed to send reset link. Please check your email.");
    }
    else
    {
        DisplayNotification("Success", "If this email is registered, a password reset link has been sent.");
    }
}

public void OnResetPasswordButtonClicked()
{
    string email = forgotPassWord.text; // Replace with your actual email input field

    if (!string.IsNullOrEmpty(email))
    {
        StartCoroutine(ResetPassword(email));
    }
    else
    {
        DisplayNotification("Error", "Please enter your email address.");
    }
}




    private void DisplayNotification(string title, string message)
    {
        notif_Title_Text.text = title;
        notif_Message_Text.text = message;
        notifPanel.SetActive(true);
    }

    // Method to hide the notification panel
    public void HideNotification()
    {
        notifPanel.SetActive(false);
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";
    }

   
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        sceneManager = FindObjectOfType<SceneManagerScript>();
        OpenFirstStartupPanel();
        
          // Passwords hidden on startup
        TogglePasswordVisibility(loginPassword, false);
        TogglePasswordVisibility(signupPassword, false);
        TogglePasswordVisibility(signupCPassword, false);
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
    }




    

    
    public void OpenPanel(GameObject panelToOpen)
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        notifPanel.SetActive(false);

        panelToOpen.SetActive(true);
    }

    public void OpenLoginPanel() => OpenPanel(loginPanel);
    public void OpenSignUpPanel() => OpenPanel(signupPanel);
    public void OpenStartupPanel() => OpenPanel(startupPanel);
    public void OpenFirstStartupPanel() => OpenPanel(firstStartupPanel);
    public void OpenForgotPassPanel() => OpenPanel(forgotPasswordPanel);

   

    private bool IsUsernameLengthValid(string username) => username.Length >= 3 && username.Length <= 15;
    private bool IsUsernameCharactersValid(string username) => Regex.IsMatch(username, @"^[a-zA-Z._]+$");
    private bool IsValidPassword(string password) => Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$");
    private bool IsValidEmail(string email) => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    
}
