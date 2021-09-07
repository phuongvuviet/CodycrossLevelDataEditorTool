using System.Collections;
using System.Collections.Generic;
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

    public void ClearAllQuestionDataViewModelList()
    {
        for (int i = 0; i < _questionDataViewModelList.Count; i++)
        {
            Destroy(_questionDataViewModelList[i].gameObject);
        }
        _questionDataViewModelList.Clear();
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
