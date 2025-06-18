// AudioManager.cs
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern
    public static AudioManager instance;

    [Header("Audio Sources")]
    [Tooltip("Arka plan müziği için kullanılır.")]
    public AudioSource musicSource; 
    [Tooltip("Genel UI ve tek seferlik sesler için kullanılır.")]
    public AudioSource sfxSource; // Inspector'dan bu objenin üzerindeki AudioSource'u sürükleyin

    [Header("Audio Clips - SFX")]
    public AudioClip playerAttackSwoosh;
    public AudioClip playerHitEnemy;
    public AudioClip playerTakeDamage;
    public AudioClip playerDeath;
    public AudioClip goblinAttack;
    public AudioClip enemyDie;
    public AudioClip itemPickup;
    public AudioClip uiClick;

    [Header("Audio Clips - Music")] 
    public AudioClip backgroundMusic;

    void Awake()
    {
        // Singleton kurulumu
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // İsteğe bağlı: Sahneler arasında yok olmamasını istiyorsanız
        // DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // Oyun başladığında arka plan müziğini çal
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // Belirtilen AudioSource üzerinde tek seferlik bir ses çalan genel bir fonksiyon
    public void PlaySound(AudioClip clip, AudioSource source)
    {
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip);
        }
    }

    // 3D dünyada belirli bir pozisyonda ses çalan bir fonksiyon (düşman ölümü vb. için)
    public void PlaySoundAtPoint(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }

    // Doğrudan ana SFX kaynağında ses çalmak için kısa yol fonksiyonları
    public void PlayPlayerAttackSwoosh() => PlaySound(playerAttackSwoosh, sfxSource);
    public void PlayPlayerHitEnemy() => PlaySound(playerHitEnemy, sfxSource);
    public void PlayPlayerTakeDamage() => PlaySound(playerTakeDamage, sfxSource);
    public void PlayItemPickup() => PlaySound(itemPickup, sfxSource);
    public void PlayUIClick() => PlaySound(uiClick, sfxSource);
    public void PlayPlayerDeath()=>PlaySound(playerDeath, sfxSource);

    // Düşman ölümü gibi 3D pozisyon gerektiren sesler için
    public void PlayEnemyDie(Vector3 position) => PlaySoundAtPoint(enemyDie, position);
    public void PlayGoblinAttack(Vector3 position) => PlaySoundAtPoint(goblinAttack, position);
}