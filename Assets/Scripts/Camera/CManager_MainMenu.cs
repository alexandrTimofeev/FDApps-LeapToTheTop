using UnityEngine;
using UnityEngine.UI;

public class CManager_MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas canvasMainMenu;
    [SerializeField] private Canvas canvasLevelMap;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonBackToMenu;

    [Header("Camera Movement")]
    [SerializeField] private Transform cameraTargetPosition;
    [SerializeField] private float cameraMoveDuration = 2f;
    [SerializeField] private AnimationCurve cameraEaseCurve;

    [Header("Camera Depth Movement")]
    [SerializeField] private Transform cameraDepthTargetPosition;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource paper;

    private Camera mainCamera;
    private Vector3 cameraStartPosition;
    private Quaternion cameraStartRotation;

    private bool isMovingToMap = false;
    private bool isMovingToMenu = false;
    private float moveTimer = 0f;

    private bool isMovingToDepth = false;
    private float depthMoveTimer = 0f;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraStartPosition = mainCamera.transform.position;
        cameraStartRotation = mainCamera.transform.rotation;

        buttonPlay.onClick.AddListener(OnPlayButtonClicked);
        buttonBackToMenu.onClick.AddListener(OnBackToMenuClicked);
    }

    private void Update()
    {
        if (isMovingToMap)
        {
            moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(moveTimer / cameraMoveDuration);
            float easedT = cameraEaseCurve.Evaluate(t);

            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition.position, easedT);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraStartRotation, cameraTargetPosition.rotation, easedT);

            if (t >= 1f)
            {
                isMovingToMap = false;
                paper.Play();
                canvasLevelMap.gameObject.SetActive(true);
            }
        }

        if (isMovingToMenu)
        {
           
            moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(moveTimer / cameraMoveDuration);
            float easedT = cameraEaseCurve.Evaluate(t);

            mainCamera.transform.position = Vector3.Lerp(cameraTargetPosition.position, cameraStartPosition, easedT);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraTargetPosition.rotation, cameraStartRotation, easedT);

            if (t >= 1f)
            {
                isMovingToMenu = false;
                canvasMainMenu.gameObject.SetActive(true);
            }
        }

        if (isMovingToDepth)
        {
            depthMoveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(depthMoveTimer / cameraMoveDuration);
            float easedT = cameraEaseCurve.Evaluate(t);

            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraDepthTargetPosition.position, easedT);
            mainCamera.transform.rotation = Quaternion.Slerp(cameraStartRotation, cameraDepthTargetPosition.rotation, easedT);

            if (t >= 1f)
            {
                isMovingToDepth = false;
            }
        }

    }
    private void OnPlayButtonClicked()
    {
       audioSource.Play();
        canvasMainMenu.gameObject.SetActive(false);
        isMovingToMap = true;
        moveTimer = 0f;
    }

    private void OnBackToMenuClicked()
    {
        paper.Play();
        canvasLevelMap.gameObject.SetActive(false);
        isMovingToMenu = true;
        moveTimer = 0f;
    }

    public void FlyCameraToMapDepth()
    {
        cameraStartPosition = mainCamera.transform.position;
        cameraStartRotation = mainCamera.transform.rotation;
        depthMoveTimer = 0f;
        isMovingToDepth = true;
    }
}