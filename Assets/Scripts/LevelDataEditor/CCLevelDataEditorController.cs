using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CCLevelDataEditorController : MonoBehaviour
{
    [SerializeField] private CCBoardDataViewModel _databoardViewModel;
    [SerializeField] private TMP_InputField _jsonDataInputField;
    [SerializeField] private TMP_InputField _jsonDataOutputInputField;
    [SerializeField] private TMP_InputField _csvFilePathInputField;
    [SerializeField] private TMP_Dropdown _dropdownType;
    [SerializeField] private GameObject _errorMessageGameObject;

    private List<string> LevelDataProperties = new List<string>()
    {
       "questions", "words", "pictures"
    };

    public enum InputDataType
    {
        JSON,
        QUESTIONS,
        WORDS,
        PICTURES
    }

    private InputDataType _curInputDataType = InputDataType.JSON;
    private Coroutine _showErrorMessageCO = null;

    private void Start()
    {
        PopulateDropdowType();
    }

    void PopulateDropdowType()
    {
        _dropdownType.options.Clear();
        Type enumInputDataTypeType = typeof(InputDataType);
        for (int i = 0; i < Enum.GetNames(enumInputDataTypeType).Length; i++)
        {
            _dropdownType.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(enumInputDataTypeType, i)));
        }

        _dropdownType.value = 0;
    }

    public void OnDropdownTypeChanged(int value)
    {
        _curInputDataType = (InputDataType)value;
    }

    public void ParseJsonData()
    {
        string inputDataText = _jsonDataInputField.text;
        inputDataText = inputDataText.Trim(new char[] {' ', '\t'});
        string[] inputItems = null;
        switch (_curInputDataType)
        {
            case InputDataType.JSON:
                try
                {
                    CCLevelData data = JsonUtility.FromJson<CCLevelData>(inputDataText);
                    if (data == null)
                    {
                        Debug.LogError("============> NULLLLLLLLLLLLLLLLLL");
                        if (_showErrorMessageCO != null)
                        {
                            StopCoroutine(_showErrorMessageCO);
                        }
                        _showErrorMessageCO = StartCoroutine(ShowErrorMessage());
                    }
                    else
                    {
                        _databoardViewModel.DestroyAllQuestionDataViewModelList();
                        _databoardViewModel.GenerateRowUIs(data);
                    }
                }
                catch
                {
                    if (_showErrorMessageCO != null)
                    {
                        StopCoroutine(_showErrorMessageCO);
                    }
                    _showErrorMessageCO = StartCoroutine(ShowErrorMessage());
                }
                break;
            case InputDataType.WORDS:
                inputItems = inputDataText.Split('\n');
                _databoardViewModel.PopulateWordsColumn(inputItems);
                break;
            case InputDataType.PICTURES:
                inputItems = inputDataText.Split('\n');
                _databoardViewModel.PopulatePicturesColumn(inputItems);
                break;
            case InputDataType.QUESTIONS:
                inputItems = inputDataText.Split('\n');
                _databoardViewModel.PopulateQuestionsColumn(inputItems);
                break;
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

        string savePath = Path.Combine(Application.persistentDataPath, "data.csv");
        File.WriteAllText(savePath, csvDataStr);
        _csvFilePathInputField.text = savePath;
        
        // string savePath = "data.csv";
        // string path = EditorUtility.SaveFilePanel("Save data as csv", "", "data.csv", "csv");
        // if (path.Length != 0)
        // {
        //     File.WriteAllText(path, csvDataStr);
        //     _csvFilePathInputField.text = path;
        // }
    }

    public void OnBtnClearColClicked()
    {
        switch (_curInputDataType)
        {
            case InputDataType.JSON:
                _databoardViewModel.ClearAllQuestionDataViewModelList();
                break;
            case InputDataType.WORDS:
                _databoardViewModel.ClearWordColData();
                break;
            case InputDataType.PICTURES:
                _databoardViewModel.ClearPictureColData();
                break;
            case InputDataType.QUESTIONS:
                _databoardViewModel.ClearQuestionColData();
                break;
        }
    }

    public void ClearAllDataField()
    {
        _databoardViewModel.DestroyAllQuestionDataViewModelList();
        _jsonDataInputField.text = "";
        _jsonDataOutputInputField.text = "";
        _csvFilePathInputField.text = "";
    }

    IEnumerator ShowErrorMessage()
    {
        _errorMessageGameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _errorMessageGameObject.SetActive(false);
    }
}
