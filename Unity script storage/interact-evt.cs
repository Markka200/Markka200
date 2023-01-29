using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class interact : MonoBehaviour
{
    public SoundEffect Audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public UnityEvent evt;

    public void Interact()
    {
        evt.Invoke();
        AudioManager.Instance.PlayClipOnce(Audio, this.gameObject);
    }
    
    void Update()
    {
        
    }
}
