using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickUp : MonoBehaviour
{
    public ParticleSystem emitParticle;

    void Update()
    {
        transform.Rotate(Vector3.up, 18f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag.Equals("Ball"))
        {
            emitParticle.Play();

            SettingsMenu.Instance.diamondsCount++;
            BallMovement.Instance.ballAudio.PlayOneShot(BallMovement.Instance.diamondSound, 0.4f);

            LevelManager.Instance.diamondstoRemove++;

            Debug.Log("DIAMOND COLLECTED!");

            Destroy(gameObject, 0.2f);
        }
    }
}
