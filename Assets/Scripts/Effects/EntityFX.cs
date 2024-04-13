using Cinemachine;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{

    protected Player player;
    protected SpriteRenderer sr;

    [Header("Pop up text")]
    [SerializeField] private GameObject popupTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;


    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit fx")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject hitCriticalFx;


    private GameObject myHealthBar;


    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMat = sr.material;

        myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
    }

    public void CreatePopupText(string _text)
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(1f, 3f);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newtext = Instantiate(popupTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newtext.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }

    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();

        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    public void CreateHitFx(Transform _target, bool _critical)
    {

        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0,0,zRotation); 
        
        GameObject hitPrefab = hitFx;
        if (_critical)
        {
            hitPrefab = hitCriticalFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if(GetComponent<Entity>().facingDirection == -1)
            {
                yRotation = 180;
            }

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition,yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, .5f);
    }
}
