using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoSelector : MonoBehaviour
{
    [Header("Logo for different languages")]
    public Image LogoImage;
    public List<Sprite> textures;
    private void Awake()
    {
        //EventHandler.instance.OnLanguageChange += CheckLogoChange;
    }
    void Start()
    {
        CheckLogoChange();
    }
    void CheckLogoChange()
    {
        string lang = PlayerPrefs.GetString("LanguageChar");
        if (lang == "en")
        {
            LogoImage.sprite = textures[0];
        }
        else if (lang == "id")
        {
            LogoImage.sprite = textures[1];
        }
        else if (lang == "de")
        {
            LogoImage.sprite = textures[2];
        }
        else 
        {
            LogoImage.sprite = textures[0];
        }
    }
}
