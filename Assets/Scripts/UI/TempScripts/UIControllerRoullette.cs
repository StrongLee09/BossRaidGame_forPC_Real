using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIControllerRoullette : UIController
{
	
    UIDataRoullette Data
    {
        get;
        set;
    }
	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		// DialogManager에 현재 이 다이얼로크 콘트롤러 클래스가 확인창을 다룬다는 사실을 등록합니다.
		UIManager.Instance.Regist(UIType.Roullette, this);
		
	}
	// 확인 팝업창이 생성될 때 호출되는 함수입니다.
	public override void Build(UIData data)
	{
		base.Build(data);
		// 데이터가 없는데 Build를 하면 로그를 남기고 예외처리를 합니다.
		if (!(data is UIDataRoullette))
		{
			Debug.LogError("Invalid dialog data!");
			return;
		}

		// DialogDataAlert로 데이터를 받고 화면의 제목과 메시지의 내용을 입력합니다.
		Data = data as UIDataRoullette;
	}

	
	public void RoulletteEnd()
	{
		// 확인 버튼을 누르면, Callback 함수를 호출합니다. 
		// calls child's callback
		if (Data != null && Data.Callback != null)
			Data.Callback();
		// 모든 과정이 끝났으므로, 현재 팝업을 DialogManager에서 제거합니다.
		UIManager.Instance.Pop();
	}
}
