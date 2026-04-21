using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController_Ship : MonoBehaviour
{
    [Header("Mesh для отключения")]
    [SerializeField] private MeshRenderer targetMeshRenderer; // Теперь можно указать в инспекторе
    [SerializeField] private CManager_DestroyedShip destroyedShip;

    public float WaitTime_ExplosionPlayer = 1;
    public float WaitTime_ExplosionShip = 0.2f;
    public float WaitTime_Sinking = 0.5f;

    public Transform Particle_Player;
    public Transform Parent_Particles;
    public Transform Parent_Crew;

    private void Awake()
    {
        // Если в инспекторе не указали — пробуем найти на себе
        if (targetMeshRenderer == null)
            targetMeshRenderer = GetComponent<MeshRenderer>();

        if (targetMeshRenderer != null)
            targetMeshRenderer.enabled = true;
        else
            Debug.LogError("MeshRenderer для отключения не назначен!", this);

        if (destroyedShip != null)
            destroyedShip.gameObject.SetActive(false);
    }

    public void StartDestruction()
    {
        StartCoroutine(Coroutine_PlayerExplosionParticle());
    }

    private IEnumerator Coroutine_PlayerExplosionParticle()
    {
        yield return new WaitForSeconds(WaitTime_ExplosionPlayer);
        CManager_Sound.Instance.Play("Environment_Explosion_01");
        Particle_Player.gameObject.SetActive(true);
        StartCoroutine(Coroutine_ShipExplosionParticles());
    }

    private IEnumerator Coroutine_ShipExplosionParticles()
    {
        yield return new WaitForSeconds(WaitTime_ExplosionShip);
        CManager_Sound.Instance.Play("Environment_Explosion_01");
        Parent_Particles.gameObject.SetActive(true);
        StartCoroutine(Coroutine_MeshSwap());
    }

    private IEnumerator Coroutine_MeshSwap()
    {
        yield return new WaitForSeconds(WaitTime_Sinking);

        if (targetMeshRenderer != null)
            targetMeshRenderer.enabled = false;

        if (destroyedShip != null)
            destroyedShip.gameObject.SetActive(true);

        //StartCoroutine(Coroutine_SinkCrew());
    }

    //private IEnumerator Coroutine_SinkCrew()
    //{
    //    float sinkDuration = 2f;
    //    float elapsed = 0f;

    //    List<Transform> crewMembers = new List<Transform>();
    //    List<Vector3> startPositions = new List<Vector3>();
    //    List<Vector3> endPositions = new List<Vector3>();

    //    foreach (Transform crew in Parent_Crew)
    //    {
    //        crewMembers.Add(crew);
    //        startPositions.Add(crew.position);
    //        endPositions.Add(crew.position + Vector3.down * 5f);
    //    }

    //    while (elapsed < sinkDuration)
    //    {
    //        elapsed += Time.deltaTime;

    //        for (int i = 0; i < crewMembers.Count; i++)
    //        {
    //            crewMembers[i].position = Vector3.Lerp(startPositions[i], endPositions[i], elapsed / sinkDuration);
    //        }

    //        yield return null;
    //    }

    //    Parent_Crew.gameObject.SetActive(false);
    //}

    public void StartCrewVictoryAnimations()
    {
        foreach (Transform crew in Parent_Crew)
        {
            crew.GetComponent<Animator>().SetTrigger("OnVictory");
        }
    }

    public void StartCrewDefeatAnimations()
    {
        foreach (Transform crew in Parent_Crew)
        {
            crew.GetComponent<Animator>().SetTrigger("OnDefeat");
        }
    }
}