using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCQuestionDataViewModel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _questionTxt;
    [SerializeField] private TMP_InputField _wordTxt;
    [SerializeField] private TMP_InputField _pictureTxt;
    private CCBoardDataViewModel _boardDataViewModel;
    
    public string QuestionTxt
    {
        get => _questionTxt.text;
        set => _questionTxt.text = value;
    }

    public string WordTxt
    {
        get => _wordTxt.text;
        set => _wordTxt.text = value;
    }

    public string PictureTxt
    {
        get => _pictureTxt.text;
        set => _pictureTxt.text = value;
    }
    
    public void Init(CCBoardDataViewModel boardDataViewModel, string question, string word, string picture)
    {
        _boardDataViewModel = boardDataViewModel;
        _questionTxt.text = question;
        _wordTxt.text = word;
        _pictureTxt.text = picture;
    }

    public void OnBtnDeleteClicked()
    {
        _boardDataViewModel.DeleteLevelDataRow(this);
    }
}
