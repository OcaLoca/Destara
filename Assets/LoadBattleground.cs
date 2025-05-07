//* ----------------------------------------------
//* 
//* 				MobyBit
//* 
//* Creation Date: 01/09/2020 00:10:30
//* 
//* Copyright � MobyBit
//* ----------------------------------------------
//*/
//
//sing System.Collections;
//sing System.Collections.Generic;
//sing UnityEngine;
//sing UnityEngine.UI;
//amespace Game
//
//   public class LoadBattleground : MonoBehaviour
//   {
//       [SerializeField] RawImage imageToShow;
//       [SerializeField] Material normalMap;
//
//       public List<ScriptableImage> imagesDatabase;
//       public List<Material> normalMapDatabase;
//
//       void Awake()
//       {
//           foreach (ScriptableImage data in Resources.LoadAll<ScriptableImage>("BattleGround"))
//           {
//               imagesDatabase.Add(data);
//           }
//           foreach (Material data in Resources.LoadAll<Material>("NormalMapBattleGround"))
//           {
//               normalMapDatabase.Add(data);
//           }
//       }
//
//       public ScriptableImage LoadScriptableImage()
//       {
//           foreach (ScriptableImage scriptableImage in imagesDatabase)
//           {
//               if (scriptableImage.chapterSection == PlayerManager.Singleton.currentPage.chapterSection)
//               {
//                   return scriptableImage;
//               }
//           }
//           return null;
//       }
//
//       public void LoadImageTexture()
//       {
//           ScriptableImage temporaryImage = LoadScriptableImage();
//
//           if ((temporaryImage == null) || (PlayerManager.Singleton.currentPage.pageID == "sceltaClasse"))
//           {
//               Debug.LogError("NoImage");
//               imageToShow.CrossFadeAlpha(0.0f, 0.0f, false);
//           }
//
//           else
//           {
//               imageToShow.CrossFadeAlpha(5.0f, 0.0f, false);
//               imageToShow.texture = temporaryImage.imageToRender;
//               imageToShow.rectTransform.localScale = new Vector3(temporaryImage.GetHeigh(), temporaryImage.GetWidht(), temporaryImage.GetDepth());
//               if (temporaryImage.isAllPage) { return; }
//               //imageToShow.gameObject.transform.position = new Vector3(temporaryImage.GetXPosition(), temporaryImage.GetYPosition(), temporaryImage.GetZPosition());
//           }
//       }
//   }
//
