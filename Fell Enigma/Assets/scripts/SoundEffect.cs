using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

    public AudioClip se1; // first swing
    public AudioClip se2; // second swing
    public AudioClip se3; // pot usage
    public AudioClip se4; // heal
    public AudioClip se5; // time sound
    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        EventManager.StartListening("PlayBattleSound", PlayBattleSound);
        EventManager.StartListening("PlayPotSound", PlayPotSound);
        EventManager.StartListening("PlayHealSound", PlayHealSound);
        EventManager.StartListening("PlayTimeSound", PlayTimeSound);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Event Manager: Plays sound effect for Battle
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 23/7/17
     */ 
    void PlayBattleSound()
    {
        source.PlayOneShot(se1, 0.5f);
        StartCoroutine(playBattleSE());
    }


    /**
     * Creates the necessary delay for the 2 sounds to play consecutively
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 23/7/17
     */
    public IEnumerator playBattleSE()
    {
        yield return new WaitUntil(() => source.isPlaying);
        yield return new WaitForSeconds(0.05f);
        source.PlayOneShot(se2, 0.5f);
    }


    /**
     * Event Manager: Plays sound effect for consumable
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 23/7/17
     */
    void PlayPotSound()
    {
        source.PlayOneShot(se3, 0.5f);
    }


    /**
     * Event Manager: Plays sound effect for Heal
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 23/7/17
     */
    void PlayHealSound()
    {
        source.PlayOneShot(se4, 0.5f);
    }


    /**
     * Event Manager: Plays sound effect for End Turn
     * 
     * @author Wayne Neo
     * @version 1.0
     * @updated on 23/7/17
     */
    void PlayTimeSound()
    {
        source.PlayOneShot(se5, 0.5f);
    }
}
