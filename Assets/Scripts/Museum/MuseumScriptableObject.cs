using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Museum", menuName = "ScriptableObjects/ScriptableMuseum", order = 0)]
public class MuseumScriptableObject : ScriptableObject{

    
    [SerializeField] private string MuseumItemID;
    [SerializeField] private string MuseumItemTitle;
    [SerializeField] private string MuseumItemDescription;
    [SerializeField] private Sprite MuseumItemImg;
    [SerializeField] public bool isUnlocked;
    [SerializeField] public bool alreadyShow;


    #region Getters
    public string GetItemID { get => MuseumItemID; }
    public string GetItemName { get => MuseumItemTitle; }
    public string GetItemDescription { get => MuseumItemDescription; }
    public bool GetItemUnlocked { get => isUnlocked; }

    public Sprite GetImage
    {
        get => MuseumItemImg;
    }

  // public void Setup(MuseumScriptableObject scriptableArtContent)
  // {
  //     //txtDescription = scriptableArtContent.GetStory;
  //     // texture = scriptableArtContent.GetImage;
  //
  //     UploadData();
  // }
  // void UploadData()
  // {
  //
  // }


    #endregion
}
