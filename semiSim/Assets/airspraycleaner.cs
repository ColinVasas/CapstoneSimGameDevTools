using UnityEngine;

public class airspraycleaner : MonoBehaviour
{
    public ParticleSystem particleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Dirty"))
        {
            other.tag = "CleanWafer";
        }
    }
}
