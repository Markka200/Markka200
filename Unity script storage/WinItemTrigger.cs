using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    public SoundEffect Audio;
    public GameObject CollectedDiamond;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == CollectedDiamond)  // jos kerätty timantti koskee triggeriin niin pelaaja voittaa // timantti on todistetusti active koska se voi koskea triggeriin
        {
            UIManager.Instance.Victory();
            AudioManager.Instance.PlayClipOnce(Audio, this.gameObject);
        }
    }
}
