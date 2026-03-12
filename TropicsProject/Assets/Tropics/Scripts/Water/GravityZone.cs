using RoR2;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
public class GravityZone : MonoBehaviour
{
    private List<CharacterBody> characterBodies = new List<CharacterBody>();
    public float gravityCoefficient;

    // this script was based on the anti gravity script used in Fogbound Lagoon. Credit to John Lagoon
    // it is largely the same but applies velocity if the character is in a volume, rather than if they are below a specified elevation

    // add character body to the list of bodies to be affected by water after it enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        CharacterBody body = other.GetComponent<CharacterBody>();
        if ((bool)body && (bool)body.characterMotor && !characterBodies.Contains(body) && body.characterMotor.useGravity && body.characterMotor.hasEffectiveAuthority)
        {
            characterBodies.Add(body);
        }
    }

    // remove character body from the list of bodies to be affected by water after it leaves the trigger
    public void OnTriggerExit(Collider other)
    {
        CharacterBody body = other.GetComponent<CharacterBody>();
        if (characterBodies.Contains(body))
        {
            characterBodies.Remove(body);
        }
    }

    // apply anti-gravity
    void FixedUpdate()
    {
        foreach (CharacterBody body in characterBodies)
        {

            CharacterMotor characterMotor = body.GetComponent<CharacterMotor>();

            if (characterMotor && !characterMotor.isGrounded && characterBodies.Contains(body))
            {
                characterMotor.velocity.y += -Physics.gravity.y * gravityCoefficient * Time.fixedDeltaTime;
            }
        }
    }

}