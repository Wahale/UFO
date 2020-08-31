using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UFO : MonoBehaviour
{
    public Rigidbody turboOneRb;
    public Rigidbody turboTwoRb;

    public GameObject VFX;

    public AudioSource fireRock;

    public float pric = 0.8f;
    public float force = 20f;

    private bool youCan = true;

    public Slider leftSlider, rightSlider;
    public Text leftText, rightText, altimetr;

    private void Start()
    {
        leftSlider.maxValue = force;
        rightSlider.maxValue = force;

        youCan = true;
    }


    private void FixedUpdate()
    {
        Vector3 minForce = Vector3.up * force * pric;
        Vector3 maxForce = Vector3.up * force;

        Vector3 leftForce = Vector3.zero;
        Vector3 rightForce = Vector3.zero;

        fireRock.Pause();

        if (Input.GetKey(KeyCode.A)&& youCan)
        {
            leftForce = maxForce;
            rightForce = minForce;
            fireRock.Play();
        }
        else if (Input.GetKey(KeyCode.D)&& youCan)
        {
            leftForce = minForce;
            rightForce = maxForce;
            fireRock.Play();
        }
        else if (Input.GetKey(KeyCode.Space)&& youCan)
        {
            leftForce = maxForce;
            rightForce = maxForce;
            fireRock.Play();
        }

        turboOneRb.AddRelativeForce(leftForce);
        turboTwoRb.AddRelativeForce(rightForce);

        leftSlider.value = leftForce.y;
        rightSlider.value = rightForce.y;

        leftText.text = leftForce.y + " Wt";
        rightText.text = rightForce.y + " Wt";

        altimetr.text = Mathf.Ceil(gameObject.transform.position.y).ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            force = 0;
            StartCoroutine(Respawn());
            youCan = false;

            GetComponent<AudioSource>().Play();

            GameObject newVfx = Instantiate(VFX, new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y,
                gameObject.transform.position.z), Quaternion.identity) as GameObject;
            Destroy(newVfx, 1f);

            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = gameObject.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(15f, Vector3.up, 4f);
                child.SetParent(null);
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            LoadNextLevel();
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Загрузка следующего уровня
    /// </summary>
    void LoadNextLevel()
    {
        int LevelIndex = SceneManager.GetActiveScene().buildIndex;
        LevelIndex++;
        SceneManager.LoadScene(LevelIndex);
    }
}
