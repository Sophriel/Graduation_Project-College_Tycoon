using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seminar : MonoBehaviour
{
	public MainCamera MainCam;
	private Professor[] targetProfessors;

	private UnityEngine.UI.Slider slider;
	private ParticleSystem ps;

	private void Start()
	{
		slider = GetComponent<UnityEngine.UI.Slider>();
		ps = GetComponentInChildren<ParticleSystem>();
	}

	public void StartSemninar(List<Professor> professors)
	{
		gameObject.SetActive(true);
		GameManager.Instance.CloseUI();
		ScheduleManager.Instance.GameTime.OnPauseButton();

		targetProfessors = professors.ToArray();

		StartCoroutine(Progress());
	}

	private IEnumerator Progress()
	{
		foreach (Professor p in targetProfessors)
		{
			MainCam.FocusToTarget(p.Character.transform.position);
			yield return new WaitForSecondsRealtime(2.0f);

			iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
			slider.value = 0;
			yield return new WaitForSecondsRealtime(0.4f);

			while (true)
			{
				while (slider.value < slider.maxValue - 1)
				{
					if (Input.GetButton("Jump"))
						break;

					slider.value++;
					yield return null;
				}
				if (Input.GetButton("Jump"))
					break;
				yield return new WaitForSecondsRealtime(0.01f);

				while (slider.value > slider.minValue + 1)
				{
					if (Input.GetButton("Jump"))
						break;

					slider.value--;
					yield return null;
				}
				if (Input.GetButton("Jump"))
					break;
				yield return new WaitForSecondsRealtime(0.01f);
			}

			if (slider.value < 60 && slider.value > 40)
			{
				SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Build);
				ps.Play();
				p.Fame += Random.Range(2, 4);
				p.Teaching += Random.Range(10, 20);
				p.Researching += Random.Range(10, 20);
				p.Task += Random.Range(10, 20);
			}

			else
			{
				SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Cant);
				p.Fame += Random.Range(0, 2);
				p.Teaching += Random.Range(1, 10);
				p.Researching += Random.Range(1, 10);
				p.Task += Random.Range(1, 10);
			}

			p.UpdateText();
		}

		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableMenu", "ignoretimescale", true));
		MainCam.EndFocusing();
		ScheduleManager.Instance.GameTime.OnPlayButton();
	}

	private void DisableMenu()
	{
		gameObject.SetActive(false);
	}
}
