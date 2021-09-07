using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CCLevelDataEditorController : MonoBehaviour
{
    [SerializeField] private CCBoardDataViewModel _databoardViewModel;
    [SerializeField] private TMP_InputField _jsonDataInputField;
    [SerializeField] private TMP_InputField _jsonDataOutputInputField;
    [SerializeField] private TMP_InputField _csvFilePathInputField;

    private List<string> LevelDataProperties = new List<string>()
    {
       "questions", "words", "pictures"
    };

    public void ParseJsonData()
    {
        _databoardViewModel.ClearAllQuestionDataViewModelList();
        CCLevelData data = JsonUtility.FromJson<CCLevelData>(_jsonDataInputField.text);
        if (data == null)
        {
            Debug.LogError("====================> Error");
        }
        else
        {
            _databoardViewModel.GenerateRowUIs(data);
        }
    }

    public void ConvertToJsonFormat()
    {
        List<CCQuestionDataViewModel> questionDataViewModels = _databoardViewModel.GetQuestionDataViewModelList();
        int numQuestions = questionDataViewModels.Count;
        CCLevelData levelData = new CCLevelData()
        {
            keywords = "",
            topic = "",
            pictures = new string[numQuestions],
            questions = new string[numQuestions],
            words = new string[numQuestions]
        };
        for (int i = 0; i < numQuestions; i++)
        {
            levelData.questions[i] = questionDataViewModels[i].QuestionTxt;
            levelData.words[i] = questionDataViewModels[i].WordTxt;
            levelData.pictures[i] = questionDataViewModels[i].PictureTxt;
        }

        string levelDataJsonStr = JsonUtility.ToJson(levelData);
        _jsonDataOutputInputField.text = levelDataJsonStr;
    }

    public void ConvertToCSVFormat()
    {
        List<CCQuestionDataViewModel> questionDataViewModels = _databoardViewModel.GetQuestionDataViewModelList();
        string csvDataStr = "";
        for (int i = 0; i < LevelDataProperties.Count; i++)
        {
            csvDataStr += LevelDataProperties[i];
            if (i < LevelDataProperties.Count - 1)
            {
                csvDataStr += ",";
            }
            else
            {
                csvDataStr += "\n";
            }
        }
        
        for (int i = 0; i < questionDataViewModels.Count; i++)
        {
            csvDataStr += questionDataViewModels[i].QuestionTxt + "," + questionDataViewModels[i].WordTxt + ","
                          + questionDataViewModels[i].PictureTxt + "\n";
        }
        string path = EditorUtility.SaveFilePanel("Save data as csv", "", "data.csv", "csv");
        if (path.Length != 0)
        {
            File.WriteAllText(path, csvDataStr);
            _csvFilePathInputField.text = path;
        }
    }

    public void ClearAllDataField()
    {
        _databoardViewModel.ClearAllQuestionDataViewModelList();
        _jsonDataInputField.text = "";
        _jsonDataOutputInputField.text = "";
        _csvFilePathInputField.text = "";
    }
}
