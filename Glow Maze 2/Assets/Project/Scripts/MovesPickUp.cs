using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovesPickUp : MonoBehaviour
{
    public int extraMoves;
    public TMP_Text extraMoveText;

    public ParticleSystem emitParticle;

    void Start()
    {
        extraMoveText.text = "+" + extraMoves.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag.Equals("Ball"))
        {
            BallMovement.Instance.ballAudio.PlayOneShot(BallMovement.Instance.movePickUpSound, 0.4f);
            emitParticle.Play();

            BallMovement.Instance.movesLeft += extraMoves;
            BallMovement.Instance.movesByPickup += extraMoves;

            LevelManager.Instance.addedMovesText.text = extraMoveText.text;
            LevelManager.Instance.addedMovesAnim.SetTrigger("ShowUI");

            Debug.Log(extraMoves + " EXTRA MOVES!");

            Destroy(gameObject, 0.2f);
        }
    }
}
