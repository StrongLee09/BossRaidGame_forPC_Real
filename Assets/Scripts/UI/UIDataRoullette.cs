
using System;
public class UIDataRoullette : UIData
{
	// 유저가 확인 버튼을 눌렀을 때 호출되는 콜백함수를 저장하는 변수입니다.
	public Action Callback
	{
		get;
		private set;
	}

	// 새로운 클래스를 생성할 때 변수들을 같이 전달해주어 객체를 생성하는 생성자입니다.
	public UIDataRoullette( Action callback = null)
		: base(UIType.Roullette)
	{
		this.Callback = callback;
	}
}
