using UnityEngine;
using UnityEngine.Pool;
public class ReturnToPool : MonoBehaviour
{
    public ParticleSystem explosion;
    public AudioSource explosionSound;
    public IObjectPool<ParticleSystem> pool;

    private void Start()
    {
        var main = explosion.main;
        main.stopAction = ParticleSystemStopAction.Callback;
        pool = LevelManager.deathParticalePool;
    }

    private void OnEnable()
    {

        explosionSound.Play();
    }


    void OnParticleSystemStopped()
    {
        pool.Release(explosion);
    }
}
