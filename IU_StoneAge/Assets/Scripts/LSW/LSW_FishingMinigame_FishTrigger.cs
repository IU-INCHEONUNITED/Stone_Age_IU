using UnityEngine;

public class LSW_FishingMinigame_FishTrigger : MonoBehaviour
{
	public bool beingCaught = false;
	private LSW_FishingMinigame minigameController;

	private void Start()
	{
		minigameController = FindObjectOfType<LSW_FishingMinigame>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (minigameController.reelingFish)
		{
			if (other.CompareTag("CatchingBar") && !beingCaught)
			{
				beingCaught = true;
				PlayerPrefs.SetInt("QuestClear", 1);
                minigameController.FishInBar();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("CatchingBar") && beingCaught)
		{
			beingCaught = false;
			minigameController.FishOutOfBar();
		}
	}
}
