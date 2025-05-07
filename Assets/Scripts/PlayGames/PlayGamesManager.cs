using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayGamesManager : MonoBehaviour
{

  // //[SerializeField] Button leaderboardBtn;
  // //public GameObject errorPanel;
  // [SerializeField] Button btnTestSignIn;
  // [SerializeField] Text txtTestSignIn;
  //
  // // Start is called before the first frame update
  //
  //
  //
  // private void OnEnable()
  // {
  //     btnTestSignIn.onClick.RemoveAllListeners(); 
  //     btnTestSignIn.onClick.AddListener(SignIn);
  // }
  // void Start()
  // {
  //
  //
  //     PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
  //     PlayGamesPlatform.InitializeInstance(config);
  //     // recommended for debugging:
  //     //PlayGamesPlatform.DebugLogEnabled = true;
  //     // Activate the Google Play Games platform
  //     PlayGamesPlatform.Activate();
  //
  //
  //     //SignIn();
  // }
  //
  // private void SignIn()
  // {
  //     PlayGamesPlatform.Instance.Authenticate((bool success) =>
  //     {
  //         if (success)
  //         {
  //             Debug.Log("login successfull");
  //             txtTestSignIn.text = "Login successfull";
  //            // ShowLeaderboard();
  //         }
  //         else
  //         {
  //             Debug.Log("login failed");
  //             txtTestSignIn.text = "Login failed ";
  //             //leaderboardBtn.gameObject.GetComponent<Button>().enabled = false;
  //             //SignIn();
  //         }
  //
  //     });
  //
  // }
  //
  // private void SignOut()
  // {
  //     PlayGamesPlatform.Instance.SignOut();
  //     //leaderboardBtn.gameObject.SetActive(false);
  // }
  //
  //
  //
  // private void UpdateLeaderboardScore(int score)
  // {
  //
  //
  //     PlayGamesPlatform.Instance.ReportScore(score, "CgkI89y9qvEPEAIQAg", (bool success) => {
  //
  //     });
  // }
  //
  // private void ShowLeaderboard()
  // {
  //     PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI89y9qvEPEAIQAg");
  // }
  //
  // 
  //
  //
  //
  // public void SignInBtn()
  // {
  //     CloseErrorPanel();
  //     SignIn();
  // }
  //
  // public void SignOutBtn()
  // {
  //     SignOut();
  // }
  //
  // public void UpdateScore(int score)
  // {
  //     UpdateLeaderboardScore(score);
  // }
  //
  // public void Show()
  // {
  //     SignIn();
  //     ShowLeaderboard();
  // }
  //
  //
  // public void OpenErrorPanel()
  // {
  //
  //
  //     //errorPanel.SetActive(true);
  //
  // }
  //
  // public void CloseErrorPanel()
  // {
  //     //errorPanel.GetComponent<Animator>().SetTrigger("Hide");
  // }
  //
  //
}
