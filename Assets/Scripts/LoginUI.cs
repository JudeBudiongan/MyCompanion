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
        yield return new WaitUntil(() => LoginTask.IsCompleted);

        try
        {
            if (LoginTask.Exception != null)
            {
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
                        Debug.LogError($"Login failed: {LoginTask.Exception.Message}");
                        break;
                }

                DisplayNotification("Error", message);
                yield break;
            }

            Debug.Log("Login successful!");
            OnLoginOrRegisterSuccess(false); // Pass 'false' to indicate login
        }
        catch (Exception e)
        {
            Debug.LogError($"An error occurred during login: {e.Message}");
            DisplayNotification("Error", "An unexpected error occurred. Please try again.");
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(() => RegisterTask.IsCompleted);

        if (RegisterTask.Exception == null)
        {
            Userauth = RegisterTask.Result.User;
            Debug.Log($"Successfully registered: {Userauth.Email}");

            UserProfile profile = new UserProfile { DisplayName = _username };
            var ProfileTask = Userauth.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(() => ProfileTask.IsCompleted);

            if (ProfileTask.Exception == null)
            {
                RegisterUser(_email, _username, _password);
                OnLoginOrRegisterSuccess(true); // Pass 'true' to indicate registration
            }
        }
        else
        {
            Debug.LogError($"Registration failed: {RegisterTask.Exception.Message}");
            DisplayNotification("Error", "Registration failed. Please try again.");
        }
    }

    private void RegisterUser(string email, string username, string password)
{
    User newUser = new User { Email = email, Username = username, Password = password };
    string json = JsonUtility.ToJson(newUser);
    string userId = databaseReference.Child("users").Push().Key; // Generate a unique user ID
    databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
    {
        if (task.IsCompleted)
        {
            Debug.Log("User registered successfully.");
        }
        else
        {
            Debug.LogError("Failed to register user: " + task.Exception);
        }
    });
}


    private void OnLoginOrRegisterSuccess(bool isRegistration)
{
    if (isRegistration)
    {
        // Set a temporary flag to indicate data should be saved after picking starter
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ShouldSaveData", 1);
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs ShouldSaveData set to 1");

        // Load the "Pick Starter" scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PickStarter");
    }
    else
    {
        // If it's just a login, navigate directly to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}


    private IEnumerator ResetPassword(string email)
    {
        var resetTask = auth.SendPasswordResetEmailAsync(email);
        yield return new WaitUntil(() => resetTask.IsCompleted);

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
        string email = forgotPassWord.text;

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

        TogglePasswordVisibility(loginPassword, false);
        TogglePasswordVisibility(signupPassword, false);
        TogglePasswordVisibility(signupCPassword, false);
    }

    public void TogglePasswordVisibility(InputField passwordField, bool showPassword)
    {
        passwordField.contentType = showPassword ? InputField.ContentType.Standard : InputField.ContentType.Password;
        passwordField.ForceLabelUpdate();
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

    public void OpenLoginPanel() => OpenPanel(loginPanel);
    public void OpenSignUpPanel() => OpenPanel(signupPanel);
    public void OpenStartupPanel() => OpenPanel(startupPanel);
    public void OpenFirstStartupPanel() => OpenPanel(firstStartupPanel);
    public void OpenForgotPassPanel() => OpenPanel(forgotPasswordPanel);
}
