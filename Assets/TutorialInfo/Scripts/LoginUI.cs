using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour

{
    public GameObject loginPanel, signupPanel, forgotPasswordPanel, startupPanel, firstStartupPanel; //notifpanel
    //public InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUsername, forgotPassEmail;
   // public Text notif_Title_Text, notif_Message_Text, profileUserName_Text, profileUserEmail_Text;

   // public LoginUI(Text notif_Title_Text, Text notif_Message_Text)
    //{
     //   this.notif_Title_Text = notif_Title_Text;
    //    this.notif_Message_Text = notif_Message_Text;
    //}
 
    // Opens the startup panel and hides the others
    public void OpenStartupPanel()
    {
        startupPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        
    }

    // Opens the first startup panel and hides the others
    public void OpenFirstStartupPanel()
    {
        firstStartupPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        
    }

    // Opens the login panel and hides the others
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        
    }

    // Opens the sign-up panel and hides the others
    public void OpenSignUpPanel()
    {
        signupPanel.SetActive(true);
        loginPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        
    }

    // Opens the forgot password panel and hides the others
    public void OpenForgotPassPanel()
    {
        forgotPasswordPanel.SetActive(true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        startupPanel.SetActive(false);
        firstStartupPanel.SetActive(false);
        
    }

     void Start()
    {
        OpenFirstStartupPanel(); // Starts the app on the first startup panel
    }
}
