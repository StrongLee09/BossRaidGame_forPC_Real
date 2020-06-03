using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    Roullette,
    CardBoard,
    Battle
}
public class UIManager : MonoBehaviour
{
    //유저에게 보여줄 UI 팝업창들을 저장해놓은 리스트.
    List<UIData> _UIQueue;
    // UIType.Alert 유형은 UIControllerAlert
    Dictionary<UIType, UIController> _UIMap;
    // 현재 화면에 떠있는 다이얼로그를 지정합니다.
    UIController _currentUI;
    // 싱글톤 패턴으로 하나의 인스턴스를 전역적으로 공유하기 위해 instance를 여기에 생성합니다.
    private static UIManager instance = new UIManager();

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }
    private UIManager()
    {
        _UIQueue = new List<UIData>();
        _UIMap = new Dictionary<UIType, UIController>();
    }

    // Regist 함수로 특정 UIType에 매칭되는 UIController를 지정합니다.
    public void Regist(UIType type, UIController controller)
    {
        _UIMap[type] = controller;
    }

    // Push함수로 UIData를 추가합니다. 
    public void Push(UIData data)
    {
        // UI 리스트를 저장하는 변수에 새로운 UI 데이터를 추가합니다. 
        _UIQueue.Add(data);

        if (_currentUI == null)
        {
            // 다음으로 보여줄 
            ShowNext();
        }
    }

    // Pop 함수로 리스트에서 마지막으로열린 UI를 닫습니다.
    public void Pop()
    {
        if (_currentUI != null)
        {
            _currentUI.Close(
                delegate {
                    _currentUI = null;

                    if (_UIQueue.Count > 0)
                    {
                        ShowNext();
                    }
                }
            );
        }
    }

    private void ShowNext()
    {
        // 다이얼로그를 리스트에서 첫번째 멤버를 가져옵니다.
        UIData next = _UIQueue[0];
        // 가져온 멤버의 다이얼로그 유형을 확인합니다. 
        // 그래서 그 다이얼로그 유형에 맞는 다이얼로그 콘트롤러(DialogController)를 조회합니다.
        UIController controller = _UIMap[next.Type].GetComponent<UIController>();
        // 조회한 다이얼로그 콘트롤러를 현재 열린 팝업의 다이얼로그 콘트롤러로 지정합니다.
        _currentUI = controller;
        // 현재 보여주열 다이럴로그 데이터를 화면에 표시합니다.
        _currentUI.Build(next);
        // 다이얼로그를 화면에 보여주는 애니메이션을 시작합니다.
        _currentUI.Show(delegate { });
        // 다이얼로그 리스트에서 꺼내온 데이터를 제거합니다.
        _UIQueue.RemoveAt(0);
    }

    // 현재 팝업 윈도우가 표시되있는지 확인하는 함수입니다.
    // _currentDialog가 비어있으면, 현재 화면에 팝업이 떠있지 않다고 판단합니다.
    public bool IsShowing()
    {
        return _currentUI != null;
    }
}
