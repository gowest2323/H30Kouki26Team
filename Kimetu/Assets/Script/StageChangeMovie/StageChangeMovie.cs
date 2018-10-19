using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Playables;



public class StageChangeMovie : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private Transform playerStartPosition;
    [SerializeField]
    private Transform cameraStartPosition;
    [SerializeField]
    private Transform doorPosition;
    [SerializeField]
    private float speed;
    private GameObject player;
    private Camera mainCamera;
    private Rigidbody playerRigid;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagName.Player.String());
        playerRigid = player.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    public void StartMovie()
    {
        player = GameObject.FindGameObjectWithTag(TagName.Player.String());
        playerRigid = player.GetComponent<Rigidbody>();
        StartCoroutine(Movie());
    }

    private IEnumerator Movie()
    {
        yield return StartCoroutine(PlayerPreparation());
        yield return StartCoroutine(CameraPreparation());
        playableDirector.Play();
    }

    /// <summary>
    /// プレイヤーのムービー準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerPreparation()
    {
        player.transform.LookAt(playerStartPosition);
        Vector3 pos = playerRigid.position;
        Vector3 direction = playerStartPosition.position - playerRigid.position;
        float distance = direction.magnitude;
        direction.Normalize();
        while (distance > 0.1f)
        {
            playerRigid.MovePosition(player.transform.position + player.transform.forward * speed * Time.deltaTime);
            distance = (playerStartPosition.position - playerRigid.position).magnitude;
            yield return null;
        }
        player.transform.position = playerStartPosition.position;
        Vector3 target = doorPosition.position - player.transform.position;
        target.y = player.transform.position.y;
        player.transform.rotation = Quaternion.LookRotation(target, Vector3.up);
    }

    private IEnumerator CameraPreparation()
    {
        Vector3 position = mainCamera.transform.position;
        Quaternion rotate = mainCamera.transform.rotation;
        float time = 0.0f;
        while (time < 1.0f)
        {
            mainCamera.transform.position = Vector3.Lerp(position, cameraStartPosition.position, time / 1.0f);
            mainCamera.transform.rotation = Quaternion.Lerp(rotate, cameraStartPosition.rotation, time / 1.0f);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartMovie();
        }
    }
}
