using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaApps
{
    // List of known social media package names
    public static readonly Dictionary<string, string> socialMediaApps = new Dictionary<string, string>
    {
        { "com.instagram.android", "Instagram" },
        { "com.snapchat.android", "Snapchat" },
        { "com.twitter.android", "Twitter" },
        { "com.facebook.katana", "Facebook" },
        { "com.whatsapp", "WhatsApp" },
        { "com.zhiliaoapp.musically", "TikTok" },
        { "com.google.android.youtube", "YouTube" }
    };
}
