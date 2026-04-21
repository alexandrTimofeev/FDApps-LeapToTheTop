using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CManager_DestroyedShip : MonoBehaviour
{
    [Header("Timing")]
    public float WaitTimeForDestruction = 0.2f;

    [Header("Animation")]
    public PlayableDirector Director;

    [Header("Physics")]
    public Vector3 Gravity_Sinking = new Vector3(0, -3f, 0);
    public float ExplosionForce = 5f;
    public Transform ExplosionOrigin;

    [Header("Ship Root")]
    public Transform ShipRoot;

    [Header("Particles")]
    public List<ParticleSystem> ExplosionParticles = new List<ParticleSystem>();

    private void OnEnable()
    {
        StartCoroutine(Coroutine_ExplodeAfterDelay());
    }

    private IEnumerator Coroutine_ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(WaitTimeForDestruction);
        Explode();
    }

    private void Explode()
    {
        Physics.gravity = Gravity_Sinking;

        if (Director != null)
            Director.Play(Director.playableAsset, DirectorWrapMode.Hold);

        ApplyExplosionForce(ShipRoot, ExplosionOrigin, ExplosionForce);

        foreach (ParticleSystem ps in ExplosionParticles)
        {
            if (ps != null)
                ps.Play();
        }
    }

    private void ApplyExplosionForce(Transform parent, Transform origin, float force)
    {
        if (parent == null || origin == null) return;

        foreach (Rigidbody rb in parent.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(force, origin.position, 5f, 1f, ForceMode.Impulse);
        }
    }
}