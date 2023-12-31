using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarCatch : MonoBehaviour
{
	public Slider slider; // 슬라이더 참조
	public float speed = 1.5f; // 슬라이더 움직이는 속도
	private bool isMovingRight = false; // 슬라이더 움직이는 방향
										//public float catchRange = 0.1f; // '캐치' 가능한 범위
	public float minCatchRange = 0.36f; // '캐치' 가능한 범위
	public float maxCatchRange = 0.66f; // '캐치' 가능한 범위

	public Image[] checkResult;         // 성공 여부 체크 이미지 배열
	public Sprite successImage;			// 체크 성공 시 이미지
	public Sprite failImage;			// 체크 실패 시 이미지
	int checkCount = 0;					// 체크박스 카운트

	public Text startComment;           // 시작 시 설명 멘트를 표시할 Text 컴포넌트
	public Text startText;              // 시작 메시지를 표시할 Text 컴포넌트
	//public AudioClip readySound;		// Ready 사운드 클립
	public AudioClip startSound;        // Start 사운드 클립
	public AudioClip bgmSound;          // 배경 사운드 클립
	private AudioSource audioSource;    // 오디오 소스 컴포넌트

	public bool isGameStart = false;    // 게임이 시작되었는지 여부

	int resultCnt = 0;					// 결과 카운트
	public Sprite[] resultImages;		// 결과물 이미지

	private HJH_Result hjh_Result;

	private void Start()
	{
		hjh_Result = GetComponent<HJH_Result>();

		checkCount = 0;
		resultCnt = 0;
		audioSource = GetComponent<AudioSource>();

		// 시작 메시지를 표시하기 위해 코루틴을 시작.
		StartCoroutine(GameStart());
	}

	void Update()
	{
		if (!isGameStart)
		{
			return;
		}

		// 슬라이더 움직이기
		if (isMovingRight)
		{
			slider.value += speed * Time.deltaTime;
			if (slider.value >= slider.maxValue)
			{
				isMovingRight = false;
			}
		}
		else
		{
			slider.value -= speed * Time.deltaTime;
			if (slider.value <= slider.minValue)
			{
				isMovingRight = true;
			}
		}


		// '캐치' 확인
		#region ### 터치 시 변경
		// ### if (Input.GetKeyDown(KeyCode.Space)) 대신 사용
		//	if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		//	{
		//		Vector3 touchPosition = Input.GetTouch(0).position;
		//		GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
		//		PointerEventData eventData = new PointerEventData(EventSystem.current);
		//		eventData.position = touchPosition;
		//		List<RaycastResult> results = new List<RaycastResult>();
		//		raycaster.Raycast(eventData, results);
		#endregion
		if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바 누르면 "캐치" 시도
		{
			Image result = checkResult[checkCount].GetComponent<Image>();
			Color color = result.color;
			color.a = 1;
			if (slider.value >= minCatchRange && slider.value <= maxCatchRange)
			{
				result.sprite = successImage;
				result.color = color;
				resultCnt++;
				Debug.Log("Catch Success!");
			}
			else
			{
				result.sprite = failImage;
				result.color = color;
				Debug.Log("Catch Fail ㅠㅠ.");
			}

			checkCount++;
		}

		if (checkCount == 3)
		{
			Debug.Log("게임 종료!!!");
			audioSource.Stop();
			isMovingRight = false;
			isGameStart = false;

			if (resultCnt >= 2)
			{
				Debug.Log(hjh_Result);
				hjh_Result.SetResult("주먹도끼", resultImages[0], true);
			}
			else
			{
				hjh_Result.SetResult("깨진 돌조각", resultImages[1], false);
			}
		}
	}

	private IEnumerator GameStart()
	{
		//// Ready 텍스트와 Ready 사운드 재생
		//readyText.gameObject.SetActive(true);
		//audioSource.PlayOneShot(readySound);

		//yield return new WaitForSeconds(1f);

		// Start 텍스트와 Start 사운드 재생
		audioSource.PlayOneShot(startSound);
		startComment.gameObject.SetActive(true);
		startText.gameObject.SetActive(false);

		yield return new WaitForSeconds(2.1f);
		startComment.gameObject.SetActive(false);
		startText.gameObject.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		startText.gameObject.SetActive(false);

		// 게임 진행
		audioSource.PlayOneShot(bgmSound);
		isGameStart = true;
	}
}
