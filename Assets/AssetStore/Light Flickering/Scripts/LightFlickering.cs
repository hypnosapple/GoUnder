using System.Collections;
using UnityEngine;
using LightFlickeringSpace;

[RequireComponent(typeof(AudioSource))]
public class LightFlickering : MonoBehaviour
{
    [Tooltip("If enabled, the Flickering will start automatically on start.")]
    public bool onStart = true;
    [Tooltip("The target light source you want to flicker.")]
    public Light lightSource;  


    [Tooltip("Use the fade effect where the light won't suddenly turn off but rather get dimmed gradually until the Alpha is 0.")]
    public bool fadeEffect = false;               
    [Tooltip("The time of the fade effect."), Range(0f, 1f)] 
    public float fadeTime = 0.2f;
    [Tooltip("the value to fade the light to. The lower the number, the weaker the light will be."), Range(0, 1)]
    public float fadeTo = 0.5f;


    [Tooltip("If enabled, Flickering timings will be randomized, easing the job on you. The randomization will be based on minimum and maximum time values set below.")]
    public bool randomizeFlickerings = true;        
    public float minRandomizeTime = 0.08f;          
    public float maxRandomizeTime = 0.3f;   


    [Tooltip("Set the Flickering amount and timings manually.")]
    public float[] flickerings;                     
    [Tooltip("If true, the Flickering will loop to the beginning of the list when finished. If false, when the Flickering finishes the entire list. No more Flickering will occur.")]
    public bool loop = true;   

    
    [Tooltip("Set the light color of the flickers.")]
    public Lights[] lightings;     

    
    [Tooltip("Play a buzz sound when the the light is on.")]                   
    public bool playAudio = false;    
    public AudioClip buzzAudio;    


    [Tooltip("Do you want to change the material of the game object emitting the light during flicker? If so, then enable this.")]
    public bool changeMaterial;  
    [Tooltip("The Mesh Renderer that of the game object that you want to change it's material. For example: the bulb glass.")]                   
    public MeshRenderer bulbObject;                 
    [Tooltip("The material you want to change to. For example: a darker/dimmed glass material for teh bulb.")]
    public Material newMaterial;                              


    #region SYSTEM VARIABLES

    int index;                                      
    int arrayLength;                                
    int randomLightingsIndex;  
    int nextIndex;

    bool triggered;  
    bool fadeOutColor = false;                      
    bool fadeInColor = false;   
    bool addedCustomMat = false;    

    float setTime;  
    float t = 0;                                

    Material defaultMaterial;   
    AudioSource usedAudioSource;                      
    Color defaultColor; 
    Color lastUsedColor;        

    #endregion           

    #region UNITY METHODS
    
    void Start()
    {
        if (lightSource == null) {
            Debug.LogWarning("You need to set the Light Source in the inspector");
            return;
        }

        usedAudioSource = GetComponent<AudioSource>();
        usedAudioSource.loop = true;

        if (onStart) {
            Flicker();
        }
    }

    void Update() 
    {
        if (fadeInColor) {
            Color fadeInColorToUse = GetNextLightingColor();

            t += Time.deltaTime / fadeTime;
            lightSource.color = Color.Lerp(lastUsedColor, fadeInColorToUse, t);
            
            if (lightSource.color.Equals(fadeInColorToUse)) {
                fadeInColor = false;
                t = 0f;
                defaultColor = lightSource.color;

                OpenLightProperties();
            }
        }

        if (fadeOutColor) {
            Color temp = new Color(fadeTo, fadeTo, fadeTo, 1) * lastUsedColor;
            
            t += Time.deltaTime / fadeTime;
            lightSource.color = Color.Lerp(lastUsedColor, temp, t);
            
            if (lightSource.color.Equals(temp)) {
                fadeOutColor = false;
                t = 0f;

                CloseLightProperties();
            }
        }
    }

    #endregion

    #region SYSTEM METHODS

    // trigger the Flickering
    public void Flicker()
    {
        if (!randomizeFlickerings && flickerings.Length <= 0) {
            Debug.LogWarning("No flickering added in the array.");
            return;
        }

        if (triggered) {
            return;
        }

        triggered = true;
        index = 0;
        arrayLength = flickerings.Length;
        
        if (changeMaterial && bulbObject != null) {
            defaultMaterial = bulbObject.material;
        }
        
        defaultColor = lightSource.color;
        StartCoroutine(OpenLight());
    }

    // stops the flickerings
    public void StopFlickering()
    {
        StopAllCoroutines();

        index = 0;
        triggered = false;
    }

    // enable the light and open the audio
    IEnumerator OpenLight()
    {
        float time;
        addedCustomMat = false;

        // check if randomizer is checked and randomize flickerings if so
        if (randomizeFlickerings) {
            float rnd = Random.Range(minRandomizeTime, maxRandomizeTime);
            time = rnd;
            setTime = rnd;
            loop = true;

            if (lightings.Length != 0) {
                if (lightings.Length < index + 1) {
                    index = 0;
                }
                
                if (!fadeEffect) {
                    lightSource.color = lightings[index].lightColor;
                }

                defaultColor = lightings[index].lightColor;

                if (bulbObject != null && lightings[index].bulbMaterial != null) {
                    addedCustomMat = true;
                }
            }
        }
        else {
            time = flickerings[index];

            if (lightings.Length >= index + 1) { 
                if (!fadeEffect) {
                    lightSource.color = lightings[index].lightColor;
                }
                
                defaultColor = lightings[index].lightColor;

                if (bulbObject != null && lightings[index].bulbMaterial != null) {
                    addedCustomMat = true;
                }
            }

            setTime = time;
        }

        
        yield return new WaitForSeconds(time);
        

        if (fadeEffect) {
            lastUsedColor = lightSource.color;
            fadeInColor = true;
            OpenLightProperties(true);
        } 
        else { 
            lightSource.enabled = true; 
            OpenLightProperties();
        }
    }

    // close the light and stop audio
    IEnumerator FlickerTimer()
    {
        yield return new WaitForSeconds(setTime);
        
        if (!fadeEffect) {
            lightSource.enabled = false;
            CloseLightProperties();
        } 
        else {
            lastUsedColor = lightSource.color;
            fadeOutColor = true;
        }
    }

    void OpenLightProperties(bool dontCall = false)
    {
        if (changeMaterial && bulbObject != null && !addedCustomMat) {
            bulbObject.material = defaultMaterial;
        }

        if (addedCustomMat) {
            if (fadeEffect) {
                if (index + 1 <= lightings.Length - 1) {
                    bulbObject.material = lightings[index+1].bulbMaterial;
                }
                else {
                    bulbObject.material = lightings[0].bulbMaterial;
                }
            }
            else {
                bulbObject.material = lightings[index].bulbMaterial;
            }
        }

        // play light audio
        PlayAudio();
        lightSource.enabled = true;

        if (!dontCall) {
            StartCoroutine(FlickerTimer());
        }
    }

    void CloseLightProperties()
    {
        StopAudio();


        if (changeMaterial && bulbObject != null && newMaterial != null) {
            bulbObject.material = newMaterial;
        }

        
        if ((index + 1) < arrayLength) {
            index++;
            StartCoroutine(OpenLight());
        }
        else {
            if (randomizeFlickerings) {
                index++;
                StartCoroutine(OpenLight());
            }
            else {
                if (loop) {
                    index = 0;
                    StartCoroutine(OpenLight());
                } 
            }
        }
    }

    void PlayAudio()
    {
        if (!playAudio) return;
        
        if (buzzAudio == null) {
            Debug.LogWarning("There is no AudioClip added in the Buzz Audio property.");
            return;
        }

        if (usedAudioSource == null) return;

        usedAudioSource.Stop();
        usedAudioSource.clip = buzzAudio;
        usedAudioSource.Play();
    }

    void StopAudio()
    {   
        if (playAudio && buzzAudio == null) {
            Debug.LogWarning("There is no AudioClip added in the Buzz Audio property.");
            return;
        }

        if (usedAudioSource == null) return;

        usedAudioSource.Stop();
    }

    Color GetNextLightingColor()
    {
        Color fadeInColorToUse;

        if (lightings.Length > 0) {
            if (index + 1 <= lightings.Length - 1) {
                return fadeInColorToUse = lightings[index+1].lightColor;
            }
            
            return fadeInColorToUse = lightings[0].lightColor;
        }
        
        return fadeInColorToUse = defaultColor;
    }

    #endregion
}
