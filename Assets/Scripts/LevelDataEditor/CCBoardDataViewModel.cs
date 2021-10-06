using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CCBoardDataViewModel : MonoBehaviour
{
    [SerializeField] CCQuestionDataViewModel questionDataViewModelPrefab;
    [SerializeField] private GameObject _rowParentGameObject;
    private List<CCQuestionDataViewModel> _questionDataViewModelList = new List<CCQuestionDataViewModel>();
    
    public void GenerateRowUIs(CCLevelData levelData)
    {
        int numRows = levelData.questions.Length;
        for (int i = 0; i < numRows; i++)
        {
            // Debug.LogError("Child count: " + _rowParentGameObject.transform.childCount);
            CCQuestionDataViewModel newRow = Instantiate(questionDataViewModelPrefab, _rowParentGameObject.transform);
            newRow.Init(this, levelData.questions[i], levelData.words[i], levelData.pictures[i]);
            AddToLevelDataViewModelList(newRow);
        }
    }

    string FormatString(string input)
    {
        return input.Trim(' ', '\n', '\r', '\t');
    }
    
    public void PopulateWordsColumn(string[] items)
    {
        AddNewLevelDataViewModelsIfNecessary(items);
        for (int i = 0; i < items.Length; i++)
        {
            _questionDataViewModelList[i].WordTxt = FormatString(items[i]);
        }
    }
    public void PopulatePicturesColumn(string[] items)
    {
        AddNewLevelDataViewModelsIfNecessary(items);
        for (int i = 0; i < items.Length; i++)
        {
            _questionDataViewModelList[i].PictureTxt = FormatString(items[i]);
        }
    }
    public void PopulateQuestionsColumn(string[] items)
    {
        AddNewLevelDataViewModelsIfNecessary(items);
        for (int i = 0; i < items.Length; i++)
        {
            _questionDataViewModelList[i].QuestionTxt = FormatString(items[i]);
        }
    }
    void AddNewLevelDataViewModelsIfNecessary(string[] items) 
    {
        if (items.Length > _questionDataViewModelList.Count)
        {
            for (int i = _questionDataViewModelList.Count; i < items.Length; i++)
            {
                CCQuestionDataViewModel newRow = Instantiate(questionDataViewModelPrefab, _rowParentGameObject.transform);
                newRow.Init(this, "", "", "");
                AddToLevelDataViewModelList(newRow);
            }
        }
    }
    public void DestroyAllQuestionDataViewModelList()
    {
        for (int i = 0; i < _questionDataViewModelList.Count; i++)
        {
            Destroy(_questionDataViewModelList[i].gameObject);
        }
        _questionDataViewModelList.Clear();
    }
    public void ClearAllQuestionDataViewModelList()
    {
        ClearPictureColData();
        ClearQuestionColData();
        ClearWordColData();
    }

    public void ClearQuestionColData()
    {
        for (int i = 0; i < _questionDataViewModelList.Count; i++)
        {
            _questionDataViewModelList[i].QuestionTxt = "";
        }
    }
    public void ClearPictureColData()
    {
        for (int i = 0; i < _questionDataViewModelList.Count; i++)
        {
            _questionDataViewModelList[i].PictureTxt = "";
        }
    }
    public void ClearWordColData()
    {
        for (int i = 0; i < _questionDataViewModelList.Count; i++)
        {
            _questionDataViewModelList[i].WordTxt = "";
        }
    }

    private void AddToLevelDataViewModelList(CCQuestionDataViewModel newRow)
    {
        if (_rowParentGameObject.transform.childCount > 1)
        {
            newRow.transform.SetSiblingIndex(_rowParentGameObject.transform.childCount - 2);
        }

        _questionDataViewModelList.Add(newRow);
    }

    public void DeleteLevelDataRow(CCQuestionDataViewModel row)
    {
        Destroy(row.gameObject);
        bool result = _questionDataViewModelList.Remove(row);
        if (result == false)
        {
            Debug.LogError("========> Can not remove row");
        }
    }

    public void CreateNewQuestionDataViewModel()
    {
        CCQuestionDataViewModel questionDataViewModel = Instantiate(questionDataViewModelPrefab, _rowParentGameObject.transform);
        questionDataViewModel.Init(this, "", "", "");
        AddToLevelDataViewModelList(questionDataViewModel);
    }

    public List<CCQuestionDataViewModel> GetQuestionDataViewModelList()
    {
        return _questionDataViewModelList;
    }
}
