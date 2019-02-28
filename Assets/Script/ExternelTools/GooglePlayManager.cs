using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;

//참고한 자료 : https://minhyeokism.tistory.com/72?category=700407, https://huiyoi.tistory.com/95
public static class GooglePlayManager
{
    public delegate void voidFunc();

    //게임서비스 플러그인 초기화시에 EnableSavedGames()를 넣어서 저장된 게임 사용할 수 있게 합니다.

    //주의 하실점은 구글플레이 개발자 콘솔의 게임서비스에서 해당게임의 세부정보에서 저장된 게임 사용을 

    //하도록 설정하셔야 합니다.


    public static void Init()

    {
#if UNITY_ANDROID
        //if (PlayGamesPlatform.Instance != null) return;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        //.EnableSavedGames()
        // registers a callback to handle game invitations received while the game is not running.
        //.WithInvitationDelegate(null)
        // registers a callback for turn based match notifications received while the
        // game is not running.
        //.WithMatchDelegate(null)
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        //.RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        //.RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        //.RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        Debug.Log("init!");
#elif UNITY_IOS
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif

    }


    //인증여부 확인

    public static void LogIn(voidFunc successFunc, voidFunc failFunc)
    {
        Debug.Log("login");
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("login success");
                successFunc();
                // to do ...
                // 구글 플레이 게임 서비스 로그인 성공 처리
            }
            else
            {
                Debug.Log("login fail");
                failFunc();
                // to do ...
                // 구글 플레이 게임 서비스 로그인 실패 처리
            }
        });
    }

    public static void LogOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public static bool CheckLogin()

    {

        return Social.localUser.authenticated;

    }

    public static void UnlockAchievement(string achive, int score)
    {
        if (score >= 100)
        {
#if UNITY_ANDROID
            Social.ReportProgress(achive, 100f, null);
#elif UNITY_IOS
            Social.ReportProgress("Score_100", 100f, null);
#endif
        }
    }
    public static void ShowAchievementUI()
    {
        if (PlayGamesPlatform.Instance == null)
        {
            Init();
        }
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 업적 UI 표시 요청할 것
        if (CheckLogin() == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("login success");
                    Social.ShowAchievementsUI();
                    return;
                }
                else
                {
                    Debug.Log("login fail");
                    return;
                }
            });
        }

        Social.ShowAchievementsUI();
    }

    public static void ReportScore(string borad, int score)
    {
#if UNITY_ANDROID
        Social.ReportScore(score, borad, (bool success) =>
        {
            if (success)
            {
                // Report 성공
                // 그에 따른 처리
            }
            else
            {
                // Report 실패
                // 그에 따른 처리
            }
        });
#elif UNITY_IOS
 
        Social.ReportScore(score, "Leaderboard_ID", (bool success) =>
            {
                if (success)
                {
                    // Report 성공
                    // 그에 따른 처리
                }
                else
                {
                    // Report 실패
                    // 그에 따른 처리
                }
            });
        
#endif
    }

    public static void ShowLeaderboardUI()
    {
        if (PlayGamesPlatform.Instance == null)
        {
            Init();
        }
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 리더보드 UI 표시 요청할 것
        if (CheckLogin() == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("login success");
                    Social.ShowLeaderboardUI();
                    return;
                }
                else
                {
                    Debug.Log("login fail");
                    return;
                }
            });
        }

#if UNITY_ANDROID
        Social.ShowLeaderboardUI();
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI("Leaderboard_ID", UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
    }

    //--------------------------------------------------------------------
    public static void ShowSelectUI()
    {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = false;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);
    }


    static void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
        }
        else
        {
            // handle cancel or error
        }
    }

    //게임 저장은 다음과 같이 합니다.

    public static void SaveToCloud()

    {
        if(PlayGamesPlatform.Instance == null)
        {
            Init();
        }

        if (!CheckLogin()) //로그인되지 않았으면

        {
            LogIn(null, null);
            //로그인루틴을 진행하던지 합니다.

        }


        //파일이름에 적당히 사용하실 파일이름을 지정해줍니다.

        OpenSavedGame("SaveData", true);

    }


    static void OpenSavedGame(string filename, bool bSave)

    {

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;


        if (bSave)

            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToSave); //저장루틴진행

        else

            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToRead); //로딩루틴 진행

    }



    //savedGameClient.OpenWithAutomaticConflictResolution호출시 아래 함수를 콜백으로 지정했습니다. 준비된경우 자동으로 호출될겁니다.

    static void OnSavedGameOpenedToSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            //파일이 준비되었습니다. 실제 게임 저장을 수행합니다.
            //저장할데이터바이트배열에 저장하실 데이터의 바이트 배열을 지정합니다.
            SaveGame(game, SaveManager.OnCloudSaveStart(), DateTime.Now.TimeOfDay);
            //SaveGame(game, "저장할데이터바이트배열", DateTime.Now.TimeOfDay);
        }
        else
        {
            Debug.LogWarning("클라우드 저장 중 파일열기에 실패 했습니다: " + status);
            SaveManager.FirstSetUp();
            SaveManager.JsonSave(SaveManager.FileName, SaveManager.Path);
            //파일열기에 실패 했습니다. 오류메시지를 출력하든지 합니다.
        }
    }
    

    static void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);
        /*
        if (savedImage != null)
        {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below

            byte[] pngData = savedImage.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);
        }*/
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }


    static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.LogWarning("클라우드 저장이 완료되었습니다.");
            //데이터 저장이 완료되었습니다.
        }
        else
        {
            Debug.LogWarning("클라우드 저장에 실패 했습니다: " + status);
            SaveManager.FirstSetUp();
            SaveManager.JsonSave(SaveManager.FileName, SaveManager.Path);
            //데이터 저장에 실패 했습니다.
        }
    }


    //----------------------------------------------------------------------------------------------------------------

    //클라우드로 부터 파일읽기

    public static void LoadFromCloud()
    {
        if (PlayGamesPlatform.Instance == null)
        {
            Init();
        }
        if (!CheckLogin())
        {
            LogIn(null, null);
            //로그인되지 않았으니 로그인 루틴을 진행하던지 합니다.
        }
        //내가 사용할 파일이름을 지정해줍니다. 그냥 컴퓨터상의 파일과 똑같다 생각하시면됩니다.
        OpenSavedGame("SaveData", false);
    }



    static void OnSavedGameOpenedToRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            LoadGameData(game);
        }
        else
        {
            Debug.LogWarning("클라우드 로드 중 파일열기에 실패 했습니다: " + status);
            SaveManager.JsonLoad(SaveManager.FileName, SaveManager.Path);
            SaveManager.FirstSetUp();
            SaveManager.ApplySave();
            //파일열기에 실패 한경우, 오류메시지를 출력하던지 합니다.
        }
    }
    
    //데이터 읽기를 시도합니다.
    static void LoadGameData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }


    static void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            //데이터 읽기에 성공했습니다.
            //data 배열을 복구해서 적절하게 사용하시면됩니다.
            SaveManager.OnCloudLoadCompleted(data);
            SaveManager.FirstSetUp();
            SaveManager.ApplySave();
            SaveToCloud();
        }
        else
        {
            Debug.LogWarning("클라우드 로드 중 데이터 읽기에 실패 했습니다: " + status);
            SaveManager.JsonLoad(SaveManager.FileName, SaveManager.Path);
            SaveManager.FirstSetUp();
            SaveManager.ApplySave();
            //읽기에 실패 했습니다. 오류메시지를 출력하던지 합니다.
        }
    }

    //--------------세이브 삭제 ------------------
    public static void DeleteGameData(string filename)
    {
        // Open the file to get the metadata.
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, DeleteSavedGame);
    }

    static void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.Delete(game);
        }
        else
        {
            // handle error
        }
    }

}
