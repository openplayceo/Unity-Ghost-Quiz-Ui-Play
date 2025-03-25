
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Quiz 속성을 제외한 모든 UI 요소의 변경 (위치, 활성화 등)
public class managerUiPlay : MonoBehaviour
{
    managerQuiz quizCompo;
    managerArchieve archieveCompo;
    managerAdGhost mngrAdGhost;
    managerLevelResult mngrLevelResult;

    animAdReward animAdRewardCompo1;
    animCat animCatCompo;
    quizMgr quizmgr;

    #region Var
    //UI 텍스트 속성
    
    [SerializeField] Text[] uiTextPlay;
    /*
    Text_CategoryName; // 0:
    Text_numHeart; // 1:
    Text_QuizNum; // 2:
    Text_noHeart; // 3:
    Text_QuizFail; // 4:
    Text_Exit; // 5:
    */
    Text[] uiTextLevel; // 퀴즈 레벨 텍스트 : categoryLevel / ButtonLv_

    // UI 오브젝트이나, 단순 활성/비활성화 용도
    GameObject categoryName; // 퀴즈 카테고리 이름
    GameObject UiButtonBackCat; // 뒤로가기 버튼

    // 퀴즈 ui 게임오브젝트
    GameObject UiCategory;
    GameObject UiLevel;
    GameObject UiLevelSub;
    GameObject UiTextReview; // 클리어한 레벨 재진입시 표시할 '복습모드'
    GameObject UiQuizCard;
    GameObject UiButtons4;
    GameObject UiButtonsOX;
    GameObject UiWarnNoHeart;
    Image uiImgGhostNoHeart; // 보상형광고 ui 유령이미지

    // 보상형광고에서 교체할 유령이미지 : 2개 이므로 직접등록
    // quizGhost_success, quizGhost_clearNot
    [SerializeField] Sprite[] uiSpriteGhost; 

    // 플레이 이벤트 Ui 윈도우
    GameObject UiEventQuiz;
    GameObject UiConfirmExit; // 각종 확인 여부창
    GameObject UiButtonYes; // '예'버튼 : exit
    GameObject UiButtonNo; // '아니오'버튼 : cancel
    GameObject UiButtonConfirm; // '확인'버튼
    GameObject UiButtonHint; // 힌트버튼
    GameObject UiHeartMinus; // 오답 시 하트감소 ui
    Animator animHint; // 힌트버튼 애니메이터 속성

    //추가 설명 Ui 윈도우 변수
    GameObject UiResultDescript;
    GameObject UiDescription;

    // 힌트 메세지
    GameObject UiHintShow;
    GameObject UiHintNoCoin;
    GameObject UiHintHeart; // 힌트 : 하트 -2 사용 
    Text textHintMsg;
    bool isHintUse = true; // 힌트사용여부, 퀴즈 당 한번씩만, 기본 'true'
    int numHintUse; // 힌트사용 횟수

    // 퀴즈 결과 Ui
    GameObject UiResult;
    Image imgUiResult; // Result 배경이미지 속성
    Text textResult;
    Image imgResultChar; // 결과용 캐릭터 이미지
    GameObject particleFxResult; // 파티클효과
    GameObject UiResetCombo; // 5콤보 시 하트보상 ui
    Sprite imgQuizSuccess; // 캐릭터 : 정답일 때
    Sprite imgQuizFail; // 캐릭터 : 오답일 때
    //CanvasGroup UiGroupRestCombo; // AdReward Ui Group 액세스용 (2018.3.12f1 Bug)

    // 스테이지 클리어 결과 Ui
    GameObject UiResultClear;
    GameObject UiTxtGroupScore;
    GameObject UiTxtGroupRank;
    GameObject UiButtonResultClear; // 클리어 ui 닫기버튼
    Text textResultClear; // 클리어 여부 표시 텍스트
    Image imgResultCat; // 클리어한 카테고리 이미지
    Text textClearCatNum; // 카테고리 id
    Image imgClearChar; // 캐릭터
    GameObject UiImgFx; // 후광이미지
    GameObject UiMsgCloudSave; // 클라우드 세이브 확인 메세지
    GameObject UiMsgRanked; // 랭크점수 업데이트 확인 메세지
    GameObject UiLevelProgress; // 퀴즈 클리어 성공후 레벨과정(경험치) ui

    // 클리어 결과 text
    [SerializeField] Text[] uiTextResult;
    /* // 아래의 순서로 직접연결
    Text_ButtonResultClear // 0 : 결과창 버튼 텍스트
    Text_resultClearSuccessQuiz // 1 : 성공한 퀴즈 결과 텍스트 (사용하지 않음)
    Text_resultClearFailQuiz; // 2 : 실패한 퀴즈 결과 텍스트
    Text_resultClearScore; // 3 : 난이도 점수 합산
    Text_resultClearScoreFail; // 4 : 틀린 퀴즈 감점 합산
    Text_resultClearScoreGet; // 5 : 현재 퀴즈 최종 합산
    Text_resultClearTotalCat; // 6 : 현재 카테고리 합산
    Text_resultClearScoreTotal; // 7 : 모든 카테고리 총합
    */

    // 데이터 엑세스 변수 (필요 시 public으로 전환)
    int numDepth = 0; //Ui 단계 depth : 안드로이드 기기 Back 버튼 용
    string isLevelSubClear = "N"; // 하위레벨 (스테이지) 클리어 여부
    int numHeart = 0; // 하트 갯수
    string catName; // 카테고리 이름
    [HideInInspector] public int numCat; // 카테고리 번호 : managerUiLvBtn.cs에서 참조됨
    [HideInInspector] public int numCatLevel; //레벨 번호 : managerUiLvBtn.cs에서 참조됨
    int numCatLevelSub; //하위레벨 번호
    bool isCleared; // 현재 서브레벨 클리어 여부
    Camera cameraMain; // 카메라 : 배경 컬러 변경용

    // 모든 카테고리 총합 + 개별 점수 총합 표시용 텍스트 배열
    Text[] uiTextScore;

    // 하트보상 관련
    Text textHeartGet; // 하트보상 조건 메세지
    Text textHeartGetNum; // 하트보상 갯수
    int numHeartReward = 3; // 광고보상 하트 갯수 +10 => 3개로 변경
    int numHeartRewardGhost = 4; // 광고보상 (광고유령) +4개
    int numHeartCombo = 1; // 퀴즈5개 콤보 하트 갯수 +2 => +1개로 변경
    int numHeartMinus = 3; // 오답 시 하트 하락 갯수 -3
    int numHeartHint = 2; // 힌트에 사용되는 하트 갯수 -2
    int numHeartReview = 1; // 복습시 획득하는 하트 갯수 +1
    int numHeartLvUp = 2; // 레벨업 시 획득하는 하트 갯수 +2
    int caseHeart; // 하트보상 케이스 (Ui) 1: 광고 2: 콤보
    int quizNumCombo; // 연속 성공 콤보
    int quizNumComboSucc; // 콤보달성 횟수

    // Result, Score 관련
    Image imgResultClear; // 클리어 배경 이미지

    // Result : Color
    Color32 colorResultSuccess = new Color32(0, 170, 160, 255); // 클리어 배경 색상 : 오렌지
    Color32 colorResultSuccess1 = new Color32(130, 170, 0, 255); // 클리어 배경 색상 : 그린
    Color32 colorResultSuccess2 = new Color32(170, 170, 170, 255); // 클리어 배경 색상 : 그레이
    Color32 colorResultFail = new Color32(170, 60, 60, 255); // 클리어 배경 색상 : 분홍
    Color32 colorBgNormal = new Color32(255, 210, 30, 255); // 기본 퀴즈 백그라운드 색상
    Color32 colorBgReivew = new Color32(80, 200, 190, 255); // 퀴즈 복습모드 백그라운드 색상

    [HideInInspector] public int quizNum = 1; // 현재 풀고 있는 퀴즈 번호 : managerQuiz.cs와 연동됨
    [HideInInspector] [SerializeField] bool IsQuizSuccess = false; // 해당 퀴즈의 풀이 성공/ 실패 여부
    int quizNumTotal; // 출제된 퀴즈의 총 수
    int quizNumSuccess; // 성공한 퀴즈 수
    int quizNumFail; // 실패한 퀴즈 수

    int quizScoreCurrent; // 현재 총점
    int quizScoreFail; // 현재 감점
    int quizScoreFinal; // 최종 점수 = 총점 - 감점
    int quizScoreCatN; // 임시 카테고리 총점
    int quizScoreCatC; // 현재 카테고리 총점
    int quizScoreCatTotalT; // 임시 모든 카테고리 총합
    int quizScoreCatTotalC; // 현재 모든 카테고리 총합
    int quizScoreCat1; // 카테고리1 총점
    int quizScoreCat2; // 카테고리2 총점
    int quizScoreCat3; // 카테고리3 총점
    int quizScoreCat4; // 카테고리4 총점
    int quizScoreCat5; // 카테고리4 총점
    int quizScoreCatTotal; // 모든 카테고리 총합
    int quizScoreDiff; // 난이도 별 점수
    string tempQuizAnswer;

    string[] nameLevel; //레벨 버튼 이름 array
    string[] nameLevelSub; //하위레벨 버튼 이름 array

    bool g_quizResult; // 퀴즈결과 저장 Ui에서 사용
    bool IsBackBtnActivate; // Back버튼 활성화 여부
    bool isRewardAdGhost; // 광고유령 사용여부
    int BgmNum;
    int numConfirmCase; // 확인 케이스
    int numHintCase; // 힌트 사용 케이스
    int numQuizNumAdGhost; // 광고유령 등장용 퀴즈번호
    int numRewardPlay; // play 중 보상형광고 유형 0: 하트부족시 / 1: 광고유령 클릭시
    #endregion

    void Start()
    {
        // UI 오브젝트
        Transform tempCanvas = GameObject.Find("Canvas").transform;
        categoryName = tempCanvas.Find("Text_CategoryName").gameObject;
        UiButtonBackCat = tempCanvas.Find("Group_heart_back").Find("Button_back").gameObject;
        UiCategory = tempCanvas.Find("category").gameObject;
        UiLevel = tempCanvas.Find("categoryLevel").gameObject;
        UiLevelSub = tempCanvas.Find("categoryLevelSub").gameObject;
        UiTextReview = tempCanvas.Find("Text_reviewMode").gameObject;
        UiQuizCard = tempCanvas.Find("quizCard").gameObject;
        UiButtons4 = tempCanvas.Find("Buttons_4").gameObject;
        UiButtonsOX = tempCanvas.Find("Buttons_OX").gameObject;
        UiWarnNoHeart = tempCanvas.Find("result_description").Find("warningNoHeart").gameObject;
        uiImgGhostNoHeart = UiWarnNoHeart.transform.Find("imgGhostMask").GetComponent<Image>();
        
        // UI 오브젝트 : EventQuiz
        UiEventQuiz = tempCanvas.Find("eventQuiz").gameObject;
        UiConfirmExit = UiEventQuiz.transform.Find("ConfirmExit").gameObject;
        UiButtonYes = UiConfirmExit.transform.Find("Button_exit").gameObject;
        UiButtonNo = UiConfirmExit.transform.Find("Button_cancel").gameObject;
        UiButtonConfirm = UiConfirmExit.transform.Find("Button_confirmed").gameObject;

        UiHintShow = UiEventQuiz.transform.Find("HintShow").gameObject;
        UiHintHeart = UiHintShow.transform.Find("HintHeart").gameObject;
        UiHintNoCoin = UiEventQuiz.transform.Find("HintNo").gameObject;
        textHintMsg = UiHintNoCoin.transform.Find("Text_HintNo").GetComponent<Text>();

        // UI 오브젝트 : Result
        UiResultDescript = tempCanvas.Find("result_description").gameObject;
        UiDescription = UiResultDescript.transform.Find("description").gameObject;
        UiResult = UiResultDescript.transform.Find("result").gameObject;
        UiButtonHint = UiResult.transform.Find("Button_hintF").gameObject;
        UiHeartMinus = UiResult.transform.Find("HeartMinus").gameObject;
        textResult = UiResult.transform.Find("Text_result").GetComponent<Text>();
        imgResultChar = UiResult.transform.Find("Image_resultChar").GetComponent<Image>();
        particleFxResult = UiResult.transform.Find("particleTemp").gameObject;
        UiResetCombo = UiResultDescript.transform.Find("AdReward").gameObject;
        textHeartGet = UiResetCombo.transform.Find("AdFinish").Find("Text_heartReward").GetComponent<Text>();
        textHeartGetNum = UiResetCombo.transform.Find("AdFinish").Find("Image_heartReward").Find("Text_numHeart").GetComponent<Text>();

        // UI 오브젝트 : ResultClear
        UiResultClear = UiResultDescript.transform.Find("resultClear").gameObject;
        textResultClear = UiResultClear.transform.Find("Text_resultClear").GetComponent<Text>();
        UiTxtGroupScore = UiResultClear.transform.Find("txtGroupScore").gameObject;
        UiTxtGroupRank = UiResultClear.transform.Find("txtGroupRank").gameObject;
        UiButtonResultClear = UiResultClear.transform.Find("Button_resultClear").gameObject;
        textClearCatNum = UiTxtGroupScore.transform.Find("Text_ClearCatNum").GetComponent<Text>();
        imgResultCat = UiResultClear.transform.Find("Image_categoryClear").GetComponent<Image>();
        imgClearChar = UiResultClear.transform.Find("Image_char").GetComponent<Image>();
        UiImgFx = UiResultClear.transform.Find("Image_fx").gameObject;
        UiMsgCloudSave = tempCanvas.Find("Image_infoSaved").gameObject;
        UiMsgRanked = tempCanvas.Find("Image_infoRanked").gameObject;
        UiLevelProgress = UiResultDescript.transform.Find("levelProgress").gameObject;

        // 컴포넌트 : managerQuiz / managerSound / quizmgr
        quizmgr = GameObject.Find("QuizMgr").GetComponent<quizMgr>();
        quizCompo = GameObject.Find("managerQuiz").GetComponent<managerQuiz>();
        archieveCompo = GameObject.Find("managerArchieve").GetComponent<managerArchieve>(); // 업적관리 스크립트
        mngrAdGhost = GameObject.Find("managerAdGhost").GetComponent<managerAdGhost>(); // 광고유령 스크립트
        mngrLevelResult = gameObject.GetComponent<managerLevelResult>(); // 레벨과정 스크립트
        cameraMain = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        // Ui 애니메이션
        animCatCompo = UiCategory.GetComponent<animCat>(); // 카테고리 애니메이션
        animAdRewardCompo1 = UiResetCombo.GetComponent<animAdReward>(); // 하트보상 애니메이션
        animHint = UiButtonHint.GetComponent<Animator>(); // 힌트애니메이션

        imgUiResult = UiResult.GetComponent<Image>(); // UiResult의 image 속성
        imgResultClear = UiResultClear.GetComponent<Image>(); // UiResultClear의 image 속성

        // 캐릭터 이미지
        imgQuizSuccess = Resources.Load<Sprite>("imagesChar/quizGhost_success");
        imgQuizFail = Resources.Load<Sprite>("imagesChar/quizGhost_fail");

        // 버튼 리스너 처리
        Button tempBtn;
        // Play 씬 Back버튼
        tempBtn = UiButtonBackCat.GetComponent<Button>();
        tempBtn.onClick.AddListener(EventBackButton);
        // quiz 푸는 중간에 나가기
        tempBtn = UiConfirmExit.transform.Find("Button_cancel").GetComponent<Button>(); // quiz 나가기 : 취소
        tempBtn.onClick.AddListener(() => ConfirmExitQuiz(false));
        tempBtn = UiConfirmExit.transform.Find("Button_exit").GetComponent<Button>(); // quiz 나가기 : 승인
        tempBtn.onClick.AddListener(() => ConfirmExitQuiz(true));
        // 퀴즈 카테고리 버튼 (일반, 영단어)
        tempBtn = UiCategory.transform.Find("catButtons").GetChild(0).GetComponent<Button>(); // 일반상식 버튼
        tempBtn.onClick.AddListener(() => selectCategory(1));
        tempBtn = UiCategory.transform.Find("catButtons").GetChild(4).GetComponent<Button>(); // 영단어 버튼
        tempBtn.onClick.AddListener(() => selectCategory(5));
        // 카테고리 별 레벨 버튼
        for (int i = 0; i < UiLevel.transform.childCount; i++)
        {
            tempBtn = UiLevel.transform.GetChild(i).GetComponent<Button>();
            tempBtn.onClick.AddListener(ToLevelSub);
        }

        // 배열변수 처리
        // uiTextLevel
        int tempLvCount = UiLevel.transform.childCount;
        uiTextLevel = new Text[tempLvCount];
        for (int i = 0; i < UiLevel.transform.childCount; i++)
        {
            Text tempTxtLv = UiLevel.transform.GetChild(i).GetChild(0).GetComponent<Text>();
            //Debug.Log("카테고리 텍스트 :"+ i + ";" + tempTxtLv);
            uiTextLevel[i] = tempTxtLv;
        }

        // uiTextScore
        uiTextScore = new Text[6]; // 6개 미리 지정
        uiTextScore[0] = UiCategory.transform.Find("Text_scoreCatTotal").GetComponent<Text>(); // 모든 카테고리 점수들의 총합
        Transform tempCatButtons = UiCategory.transform.Find("catButtons");
        for (int i = 0; i < tempCatButtons.childCount; i++)
        {
            Text tempTxtCat = tempCatButtons.GetChild(i).GetChild(2).GetComponent<Text>();
            uiTextScore[i+1] = tempTxtCat;
        }

        Init();
    }

    // 초기화 세팅
    private void Init()
    {
        // 임시 : 출제된 퀴즈의 총수 = 10, 난이도 1 점수 = 2
        quizNumTotal = 10;
        quizScoreDiff = 2;

        // managerData에서 하트 갯수를 가져옴.
        numHeart = managerData.instanceData.numHeart;
        quizScoreCat1 = managerData.instanceData.quizScoreCat1;
        quizScoreCat2 = managerData.instanceData.quizScoreCat2;
        quizScoreCat3 = managerData.instanceData.quizScoreCat3;
        quizScoreCat4 = managerData.instanceData.quizScoreCat4;
        quizScoreCat5 = managerData.instanceData.quizScoreCat5;
        quizScoreCatTotal = managerData.instanceData.quizScoreTotal;

        quizScoreCatTotalT = quizScoreCatTotal; // 카테고리 선택 화면에서 표시 할 점수
        quizScoreCatTotalC = quizScoreCatTotal; // 결과 화면에서 표시 할 점수
        ScoreCatUpdate(); // 카테고리 점수 ui 갱신
        numDepth = 0; //Ui Depth

        //ui 초기화 on : 처음 켜져야 할 항목
        UiCategory.SetActive(true);

        //ui 초기화 off
        categoryName.SetActive(false);
        UiLevel.SetActive(false);
        UiLevelSub.SetActive(false);

        UiConfirmExit.SetActive(false);
        UiHintShow.SetActive(false);
        UiHintNoCoin.SetActive(false);

        UiResult.SetActive(false);
        UiDescription.SetActive(false);

        //ui 초기화 off : 클리어
        UiResultClear.SetActive(false);
        UiTxtGroupScore.SetActive(false);
        UiTxtGroupRank.SetActive(false);

        IsBackBtnActivate = true; // Back 버튼 활성화여부

        numHeartUpdate(); // 하트갯수 업데이트

        if (!managerGoogleAds.instanceGoogleAd.isBannerLoaded) // 배너광고가 로드 되지 않았다면,
        {
            managerGoogleAds.instanceGoogleAd.LoadBannerAd(); // 배너광고 표시
        }
        else
        {
            if (managerGoogleAds.instanceGoogleAd.isBannerHidden)
            {
                managerGoogleAds.instanceGoogleAd.UnhideBannerAd();
            }
        }
    }

    // 안드로이드 백버튼 : 임시 비활성화됨
    /*
    void Update()
    {         
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            EventBackButton();
        }
    }*/

    #region UI_Button_Event
    // 타이틀로 돌아가기 + 배너광고 닫기
    void backToTtitle()
    {
        // 배너광고 닫기 : Google Admob
        managerGoogleAds.instanceGoogleAd.CloseBannerAd(); // Admob : to 타이틀 시 끄기
        SceneManager.LoadScene(0);
    }

    // 카테고리 선택 버튼
    void selectCategory(int cat)
    {        
        switch (cat)
        {
            case 1:
                uiTextPlay[0].text = "일반상식";
                quizScoreCatN = quizScoreCat1;
                quizScoreCatC = quizScoreCat1;
                break;

            case 2:
                uiTextPlay[0].text = "속담";
                quizScoreCatN = quizScoreCat2;
                quizScoreCatC = quizScoreCat2;
                break;

            case 3:
                uiTextPlay[0].text = "사자성어";
                quizScoreCatN = quizScoreCat3;
                quizScoreCatC = quizScoreCat3;
                break;

            case 4:
                uiTextPlay[0].text = "세계사";
                quizScoreCatN = quizScoreCat4;
                quizScoreCatC = quizScoreCat4;
                break;

            case 5:
                uiTextPlay[0].text = "영어단어";
                quizScoreCatN = quizScoreCat5;
                quizScoreCatC = quizScoreCat5;
                break;

        }

        catName = uiTextPlay[0].text;
        numCat = cat;
        //Debug.Log("카테고리 :" + catName + numCat);

        animCatCompo.animCatFadeOut(); // 버튼 애니메이션
        managerSound.instanceSound.SoundClick1();

        //managerData category 설정
        managerData.instanceData.gCategory = cat-1; // 0부터 시작

        showLevel();
    }

    // Back버튼과 안드로이드 Back 버튼과 연계
    // Group_heart_back / Button_back
    void EventBackButton ()
    {
        if (IsBackBtnActivate)
        {
            switch (numDepth)
            {

                case 0:// category (Back & Exit to Title)
                    backToTtitle();
                    break;

                case 1:// category <- level
                    BackToCategory();
                    break;

                case 2:// category/level <- levelSub
                    BackToLevel();
                    break;

                case 3:// category/level/levelSub <- QuizPlay
                    BackFromPlay();
                    break;
                
                case 4: // 보상형 광고 권유 닫기 (취소)
                    //ShowDescription();
                    HideWarnAndReward2();
                    break;

                case 5: // 보상형 광고 에러 닫기 (취소)
                    BackFromPlayCancel();
                    break;

                case 6: // 현재 총 문제 완료시 공지 닫기
                    break;

                default: // 

                    break;
            }

        }
      
    }

    // 버튼 : 카테고리로 돌아가기
    void BackToCategory()
    {
        numDepth = 0;

        categoryName.SetActive(false);
        UiLevel.SetActive(false);

        UiCategory.SetActive(true);
        animCatCompo.animCatFadeIn(); // 버튼 애니메이션

        quizScoreCat1 = managerData.instanceData.quizScoreCat1;
        quizScoreCat2 = managerData.instanceData.quizScoreCat2;
        quizScoreCat3 = managerData.instanceData.quizScoreCat3;
        quizScoreCat4 = managerData.instanceData.quizScoreCat4;
        quizScoreCat5 = managerData.instanceData.quizScoreCat5;

        ScoreCatUpdate();

        managerSound.instanceSound.SoundBack();
  
    }

    // 카테고리 점수 Ui Text 업데이트
    void ScoreCatUpdate ()
    {
        uiTextScore[0].text = "<size=60>"+quizScoreCatTotal.ToString() + "</size>\n전체총점";
        uiTextScore[1].text = quizScoreCat1.ToString() + "점";
        uiTextScore[2].text = quizScoreCat2.ToString() + "점";
        uiTextScore[3].text = quizScoreCat3.ToString() + "점";
        uiTextScore[4].text = quizScoreCat4.ToString() + "점";
        uiTextScore[5].text = quizScoreCat5.ToString() + "점";
    }

    // 하트갯수 표시 업데이트
    void numHeartUpdate()
    {
        uiTextPlay[1].text = numHeart.ToString();
    }

    // 카테고리를 선택하면 레벨 보여주기
    void showLevel()
    {
        numDepth = 1;

        categoryName.SetActive(true);
        UiLevel.SetActive(true);

        // 레벨 버튼 텍스트 갱신
        
        if(numCat == 1)
        {
            uiTextLevel[0].text = "레벨 1 - 쉬움";
            uiTextLevel[1].text = "레벨 2 - 중간";
            uiTextLevel[2].text = "레벨 3 - 약간어려움";
            uiTextLevel[3].text = "레벨 4 - 어려움";

        } else if(numCat == 5)
        {
            uiTextLevel[0].text = "레벨 1 - 기초단어";
            uiTextLevel[1].text = "레벨 2 - 숙어";
            uiTextLevel[2].text = "레벨 3 - 다중의미";
            uiTextLevel[3].text = "레벨 4 - 본격테스트";
        }

        managerSound.instanceSound.SoundClick1();
    }

    // 버튼 : 하위 레벨에서 뒤로가기
    void BackToLevel()
    {
        numDepth = 1;
        UiLevel.SetActive(true);

        // 하위레벨 버튼들을 비활성화
        for (int i=0; i<UiLevelSub.transform.childCount; i++)
        {
            UiLevelSub.transform.GetChild(i).gameObject.SetActive(false);
        }

        UiLevelSub.SetActive(false); // 하위레벨 버튼 부모 비활성화
        uiTextPlay[0].text = catName;
        managerSound.instanceSound.SoundBack();
    }

    // 버튼 : 카테고리 레벨 선택 했을 떄 + 하트의 갯수 체크
    void ToLevelSub()
    {
        // 해당 레벨 버튼 오브젝트의 이름을 가져옴
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        int btnChildNum = btn.transform.GetSiblingIndex(); // 해당버튼의 child index
        string levelBtn = btn.ToString();
        nameLevel = levelBtn.Split('_');

        // 해당 레벨 버튼 오브젝트의 번호를 가져옴
        var iLevel = int.Parse(nameLevel[1]);
        //Debug.Log("레벨 : " + btnChildNum + " / " + iLevel);

        // 레벨 텍스트를 가져옴
        Text lvText = uiTextLevel[btnChildNum];
        string txtLvText = lvText.text.Substring(4);
        uiTextPlay[0].text = catName + " " + iLevel.ToString() + txtLvText;
        
        showLevelSub(iLevel);
        managerData.instanceData.gLevel = iLevel;

        // 해당 레벨 문제 생성
        // QuizMgr gCategory gLevel 설정 필수(현재는 Inspactor 에서 설정)
        quizmgr.setLevel(iLevel);
        quizmgr.setMyQuiz();
    }
    #endregion

    #region Reward_Ad
    // 하트가 없어서 동영상 권유 Ui 팝업
    public void ShowWarnNoHeart ()
    {
        //isRewardAdGoogle = true; // 이전에는 false
        numRewardPlay = 0; // 하트 +3개 보상용
        numDepth = 4;
        uiImgGhostNoHeart.sprite = uiSpriteGhost[1]; // 유령 이미지 교체
        uiTextPlay[3].text = "하트가 하나도 없어요.8ㅅ8...\n광고를 보고 하트 "+numHeartReward.ToString()+"개를 획득해 보아요! ,,ㅎㅅㅎ,,";
        UiResultDescript.SetActive(true);
        UiWarnNoHeart.SetActive(true);

        managerSound.instanceSound.SoundNoHint();
    }

    // 플레이 중 광고유령 버튼 누르면 팝업
    public void ShowGhostAdHeart()
    {
        //isRewardAdGoogle = true;
        numRewardPlay = 1; // 하트 +4개 보상용
        numDepth = 4;
        uiImgGhostNoHeart.sprite = uiSpriteGhost[0]; // 유령 이미지 교체
        uiTextPlay[3].text = "유령군이 나타났어요!\n광고를 보고 하트 "+numHeartRewardGhost.ToString()+"개를 획득해 보아요! ,,ㅎㅅㅎ,,";
        UiResultDescript.SetActive(true);
        UiWarnNoHeart.SetActive(true);

        managerSound.instanceSound.SoundNoHint();
    }

    // 하트보너스 Ui 닫기버튼 액션 : 보상형 동영상 결과 / 퀴즈 5개 연속 콤보 결과
    public void HideHeartPlusEvent ()
    {
        switch (caseHeart)
        {
            case 1: // 보상형 동영상 결과 후 닫기
                HideWarnAndReward();
                break;

            case 2: // 퀴즈 5개 연속 콤보 별과 후 닫기
                HideBonusHeart();
                break;

            case 3: // 레벨(스테이지) 클리어 후 표시 후 닫기
                HideBonusHeartResult();
                break;
        }
    }

    // 리워드 (동영상) 광고시청을 시도
    public void ActUnityAd ()
    {
        mngrAdGhost.AdGhostHide();

        //권유팝업 끄기
        UiResultDescript.SetActive(false);
        UiWarnNoHeart.SetActive(false);

        caseHeart = 1;
        managerGoogleAds.instanceGoogleAd.ShowRewardAd(1); // admob : Play 유령군 이벤트
        //managerUnityAds.instance.ShowRewardAd(1); // unity ads => 2023.10.27 사용중지
    }

    // 동영상 광고시청 실패 화면
    public void ShowAdRewardError()
    {
        numDepth = 5;
        numConfirmCase = 1;

        ShowConfirm(0);

        uiTextPlay[5].text = "인터넷이 연결되지 않아 광고가 재생되지 못했습니다.\n다시 시도 해보시겠습니까?";
    }

    // 동영상 광고시청을 '완료'했을 경우 결과 표시 + 하트증정 + 세이브
    public void ShowAdRewardFinish()
    {
        UiResultDescript.SetActive(true);
        textHeartGet.text = "광고시청 보상!";

        if (numRewardPlay == 0)
        {
            numHeart = numHeart + numHeartReward;
            textHeartGetNum.text = "+" + numHeartReward.ToString();
        } else if (numRewardPlay == 1)
        {
            numHeart = numHeart + numHeartRewardGhost;
            textHeartGetNum.text = "+" + numHeartRewardGhost.ToString();
            isRewardAdGhost = true; // 광고유령 사용 여부 : true
        }

        /*
        numHeart = numHeart + numHeartReward;
        textHeartGetNum.text = "+" + numHeartReward.ToString();
        */
        managerData.instanceData.numHeart = numHeart;
        managerData.instanceData.DataSave();

        UiResetCombo.SetActive(true);
        numHeartUpdate(); // 하트갯수 업데이트

        managerSound.instanceSound.SoundQuizSuccess();
    }

    // 동영상 관련 Ui 모두 끄기
    public void HideWarnAndReward ()
    {
        animAdRewardCompo1.animAdFadeOut(1);

        //UiWarnNoHeart.SetActive(false);
        //UiResetCombo.SetActive(false);
    }

    // 동영상 권유 팝업 끄기
    public void HideWarnAndReward2()
    {
        // 뎁스 설정
        if (numRewardPlay == 0) // 서브레벨 중
        {
            numDepth = 2;
        } else if (numRewardPlay == 1) // 퀴즈 플레이중
        {
            numDepth = 3;
        }

        //numDepth = 2;
        //UiWarnNoHeart.SetActive(false);
        UiResetCombo.SetActive(false);
        UiWarnNoHeart.SetActive(false);
        UiResultDescript.SetActive(false);

    }
    #endregion

    // ConfirmExit 처리
    #region UI_Exit_to_Title
    // 퀴즈 도중 나가기
    void ConfirmExitQuiz(bool exit)
    {
        if (exit)
        {
            switch (numConfirmCase)
            {
                case 0: // 승인 : 퀴즈 밖으로 나가기
                    ExitQuizPlay();
                    break;

                case 1: // 승인 : 보상형 동영상 재시도
                    ActUnityAd();
                    break;
            }
        }
        else
        {
            switch (numConfirmCase)
            {
                case 0: // 퀴즈에서 밖으로 나갈 때 취소
                    BackFromPlayCancel();
                    break;

                case 1: // 보상형 동영상 에러 취소
                    numDepth = 2;
                    BackFromPlayCancel();
                    break;
            }
        }
    }
    
    // ConfirmExit 창 열기 (Yes / No : Ok)
    void ShowConfirm(int type)
    {
        UiEventQuiz.SetActive(true);
        UiConfirmExit.SetActive(true);

        switch (type)
        {
            case 0:
                UiButtonYes.SetActive(true);
                UiButtonNo.SetActive(true);
                break;

            case 1:
                UiButtonConfirm.SetActive(true);
                break;
        }
    }
    #endregion

    #region Exit_from_Quiz
    //상위레벨 버튼 누른 후 : 하위 레벨 보여주기
    void showLevelSub(int level)
    {
        int tempSubbtnNum = quizmgr.getSubBtnNumber();

        numCatLevel = level;
        numDepth = 2;

        UiLevel.SetActive(false);
        UiLevelSub.SetActive(true);

        // 문제 갯수 만큼 버튼 활성화
        for (int i=0; i < tempSubbtnNum; i++)
        {
            UiLevelSub.transform.GetChild(i).gameObject.SetActive(true);
        }

        managerSound.instanceSound.SoundClick1();
    }

    // 모든 퀴즈 완료 시 공지 확인창 팝업 : (임시 : 사용하지 않음)
    /*public void ShowGuideAllClear()
    {
        ShowConfirm(1);

        uiTextPlay[5].text = "와우! 모든 퀴즈를 풀었어요!\n다음 업데이트를 기대 해 주세요.";
    }*/

    // 버튼 : 퀴즈 나가기 확인창 팝업
    void BackFromPlay ()
    {
        numConfirmCase = 0;
        ShowConfirm(0);
        uiTextPlay[5].text = "퀴즈에서 나오시겠어요?";
        managerSound.instanceSound.SoundBack();       
    }

    // 버튼 : 퀴즈 나가기 확인창 닫기 (취소)
    void BackFromPlayCancel()
    {
        //numDepth = 3;
        HideEventPlay(); // 사운드 있음

    }
    #endregion

    // 퀴즈 플레이에서 뒤로가기 승인 : 하위레벨 리스트로 + 세이브
    void ExitQuizPlay()
    {
        numDepth = 2;
        isRewardAdGhost = false; // 광고유령 사용 여부 reset

        categoryName.SetActive(true);

        UiLevelSub.SetActive(true);
        UiQuizCard.SetActive(false);
        cameraMain.backgroundColor = colorBgNormal;

        // 4지 선다 / OX 버튼 비활성화
        UiButtons4.SetActive(false);
        UiButtonsOX.SetActive(false);

        mngrAdGhost.AdGhostHide(); // 광고유령 비활성화
        ResetLevelSub(); // 모든 퀴즈 변수 초기화
        HideEventPlay();

        managerData.instanceData.DataSave(); // 데이터를 세이브

        managerSound.instanceSound.SoundClick1();
        managerBGM.instanceBgm.PlayBgmTitle(); // Title BGM 재생

    }

    // Quiz 플레이 : 문제 + 버튼 표시
    #region Enter_Quiz_Play
    // 하위 레벨 버튼을 눌렀을 때 + 하트갯수 체크 + 광고유령 등장시기(퀴즈번호) 조정
    public void ToQuizPlay()
    {
        // 해당 서브레벨 버튼 오브젝트의 이름을 가져옴 (이후 서브레벨 버튼의 번호만 가져오기)
        string levelSubBtn = EventSystem.current.currentSelectedGameObject.ToString();
        nameLevelSub = levelSubBtn.Split('_');
        // 해당 서브레벨 버튼 오브젝트의 번호를 가져옴
        var iLevelSub = int.Parse(nameLevelSub[1]);
        numCatLevelSub = iLevelSub;

        isLevelSubClear = managerData.instanceData.getLevelClear(numCatLevel, numCatLevelSub);

        if (numHeart > 0 || isLevelSubClear == "Y")
        {

            // QuizMgr sub레벨 설정
            quizmgr.setSubLevel(iLevelSub);
            // QuizMgr 해당 하위레벨 문제를 불러옵니다.
            quizmgr.getMyQuiz(iLevelSub);

            IsBackBtnActivate = false;
            UiButtonBackCat.SetActive(false); //Back 버튼 비활성화

            // BGM 랜덤선택 및 재생
            if (BgmNum >=3)
            {
                BgmNum = 0;
            }

            switch (BgmNum)
            {
                case 0:
                    managerBGM.instanceBgm.PlayBgm1();
                    break;

                case 1:
                    managerBGM.instanceBgm.PlayBgm2();
                    break;

                case 2:
                    managerBGM.instanceBgm.PlayBgm3();
                    break;
            }
            BgmNum++;

            // 광고유령 등장 퀴즈넘버 랜덤처리
            // 퀴즈 4 ~ 6 사이 등장
            numQuizNumAdGhost = Random.Range(4, 9);

            managerBGM.instanceBgm.isBgmTitleOn = false;

            // 다음단계 : 퀴즈 준비로 넘어감
            PrepairQuiz(iLevelSub);

        }
        else if (numHeart <=0) // 하트가 0, 0보다 작을 경우
        {
            //광고영상 시청 권유 Ui 팝업
            ShowWarnNoHeart();
        }

    }

    // 퀴즈 준비 + 클리어 유무 확인
    // + 통계전송 (사용하지 않음 2023.10.29)
    void PrepairQuiz (int levelSub)
    {     
        numCatLevelSub = levelSub;

        // 퀴즈문제 로드
        quizCompo.setQuiz();

        // 퀴즈카드 활성화
        ShowQuizCard(quizmgr.getQuizType(quizNum));

        // 클리어 유무 확인
        isLevelSubClear = managerData.instanceData.getLevelClear(numCatLevel, numCatLevelSub);

        if (isLevelSubClear == "Y")
        {
            //광고로 인해 "복습모드" 텍스트 표시 비활성화
            cameraMain.backgroundColor = colorBgReivew;

            // 유니티 애널리틱스 : 복습과정 진행시작 통계전송
            //managerUnityAnalytics.instanceUnityAnalytics.ReportReviewPlay();
        }
        else
        {
            isLevelSubClear = "N"; // 임시 :
        }
    }

    // 퀴즈 준비가 완료되면
    public void ShowQuizCard(string quizMethod)
    {
        //Debug.Log("퀴즈타입 : " + quizMethod);
        tempQuizAnswer = quizMethod; // 임시저장 : 퀴즈답안지는 퀴즈내용 애니가 끝난후
        numDepth = 3;

        categoryName.SetActive(false);
        UiLevelSub.SetActive(false);

        UiQuizCard.SetActive(true);

        QuizUiUpdate(); //퀴즈번호, 성공, 실패횟수 업데이트
    }

    // 선택답안지 표시 : 텍스트 애니메이션이 끝나면 (managerQuiz) + 광고유령 등장
    public void ShowQuizAnswer()
    {
        switch (tempQuizAnswer)
        {
            case "Q":
                UiButtonsOX.SetActive(false);
                UiButtons4.SetActive(true);
                break;

            case "O":
                UiButtons4.SetActive(false);
                UiButtonsOX.SetActive(true);
                break;

            default:
                UiButtons4.SetActive(true);
                break;

        }
        // Back 버튼 활성화
        IsBackBtnActivate = true;
        UiButtonBackCat.SetActive(true);

        // 조건이 맞을 경우, 광고유령 활성화
        if (numQuizNumAdGhost == quizNum || numHeart < 6)
        {
            if(!isRewardAdGhost && isLevelSubClear == "N")
            {
                mngrAdGhost.ShowAdGhost();
            }
            
        }
    }

    // 퀴즈 갱신 시 업데이트 : 퀴즈 번호, 성공, 실패횟수
    public void QuizUiUpdate()
    {
        uiTextPlay[2].text = "퀴즈 " + quizNum.ToString() + "/" + quizNumTotal.ToString();        
    }

    // Quiz 플레이 중 '힌트'버튼을 눌렀을 때 (플레이 이벤트 윈도우)
    // 보상형 광고 보기로 변경예정
    public void ShowHint(int hintCase)
    {
        switch (hintCase)
        {
            case 0:// 기본 상태에서 힌트를 봤을 경우
                numHintCase = 0;
                break;

            case 1:// 퀴즈 결과창에서 힌트를 봤을 경우
                numHintCase = 1;
                break;
        }

        numDepth = 7;
        UiEventQuiz.SetActive(true);

        //사용할 수 있는지 여부 확인
        if (!isHintUse)
        {
            //사용불능
            UiHintNoCoin.SetActive(true);
            textHintMsg.text = "이미 힌트를 사용했어요 ㅇㅅㅇ...";

            managerSound.instanceSound.SoundNoHint();           
        }
        else
        {
            //하트갯수가 2보다 크거나 이미 완료한 퀴즈라면 사용가능 (힌트 시 -2사용)
            if (numHeart > 2 || isLevelSubClear == "Y")
            {
                //사용가능
                if (isLevelSubClear == "Y") // 이미 클리어 했을 경우 하트를 소진하지 않는다.
                {

                }
                else
                {
                    //하트갯수 -2
                    numHeart = numHeart - numHeartHint;
                    UiHintHeart.SetActive(true);
                    numHeartUpdate(); // 하트갯수 업데이트
                }
                numHintUse++; //힌트사용 가산
                UiHintShow.SetActive(true);

                // 힌트 텍스트 업데이트
                // textHint.text = ",,ㅇㅅㅇ,,";

                managerSound.instanceSound.SoundHint();

                // 유니티 애널리틱스 통계전송 : 힌트사용
                //managerUnityAnalytics.instanceUnityAnalytics.ReportUseHint(uiTextPlay[0].text, numCatLevel, numCatLevelSub, quizNum);
                isHintUse = false;

            } else
            {
                //사용불능
                UiHintNoCoin.SetActive(true);
                textHintMsg.text = "하트가 부족해요. 8ㅅ8;";

                managerSound.instanceSound.SoundNoHint();
            }
            
        }

    }

    // 플레이 이벤트 윈도우 숨김
    public void HideEventPlay()
    {
        UiEventQuiz.SetActive(false);
        UiConfirmExit.SetActive(false);
        UiButtonYes.SetActive(false);
        UiButtonNo.SetActive(false);
        UiButtonConfirm.SetActive(false);

        UiWarnNoHeart.SetActive(false);

        managerSound.instanceSound.SoundBack();        
    }

    // 플레이 이벤트 윈도우 숨김 (힌트)
    public void HideHint()
    {
        switch (numHintCase)
        {
            case 0: // 기본 상태에서 힌트를 봤을 경우
                numDepth = 3;
                break;

            case 1:// 퀴즈 결과창에서 힌트를 봤을 경우
                numDepth = 5;
                break;
        }

        UiEventQuiz.SetActive(false);
        UiHintNoCoin.SetActive(false);
        UiHintShow.SetActive(false);

        managerSound.instanceSound.SoundBack();      
    }

    // 퀴즈풀이 성공/실패 결과 Ui (Ui Desctription 에 속해 있음)
    public void ShowQuizResult (bool quizResult)
    {
        g_quizResult = quizResult; // 글로벌 변수에 저장 
        numDepth = 7;

        UiResultDescript.SetActive(true);
        UiResult.SetActive(true);

        if (quizResult)
        {
            // 정답/오답에 따라 텍스트 전환
            // 정답
            particleFxResult.SetActive(true);
            imgResultChar.sprite = imgQuizSuccess;
            textResult.text = "정답입니다!";
            IsQuizSuccess = true;

            UiButtonHint.SetActive(false); //힌트버튼 없음

            imgUiResult.color = colorResultSuccess; // 그린 배경
            managerSound.instanceSound.SoundQuizSuccess();
            
            if (isLevelSubClear == "N") //클리어하지 않은 상태에서
            {
                quizNumSuccess++; //성공한 퀴즈갯수 증가
                quizNumCombo++; // 성공한 퀴즈 콤보
            }
        } else
        {
            //오답
            imgResultChar.sprite = imgQuizFail;
            textResult.text = "틀렸습니다!";
            IsQuizSuccess = false;

            //UiHeartMinus.SetActive(true); // 하트감소 ui 표시
            UiButtonHint.SetActive(true); // 힌트버튼 표시
            // 힌트버튼 애니메이션 재생 (한번만)
            animHint.SetTrigger("BtnScaleOnce");

            imgUiResult.color = colorResultFail; // 분홍 배경
            managerSound.instanceSound.SoundQuizFail();
            
            // 해당레벨을 클리어 하지 않은경우에만 하트소진, 퀴즈실패 갯수 증가
            if (isLevelSubClear == "N")
            {
                //numHeart--; // 하트소진
                UiHeartMinus.SetActive(true); // 하트감소 ui 표시
                numHeart = numHeart - numHeartMinus; // 신규 : 하트소진 -3

                // 하트의 < 0 처리
                if (numHeart < 0)
                {
                    numHeart = 0;
                }

                quizNumCombo = 0; // 콤보 초기화
                quizNumFail++; // 실패한 퀴즈갯수 증가

                numHeartUpdate(); // 하트갯수 업데이트

                // 유니티 애널리틱스로 통계전송
                //managerUnityAnalytics.instanceUnityAnalytics.ReportQuizFail(uiTextPlay[0].text, numCatLevel, numCatLevelSub, quizNum);
            }

        }

        QuizUiUpdate();
    }

    // 퀴즈 성공/실패 Ui를 닫고 퀴즈풀이 연속성공(5개) 체크 + 보너스 하트 증정
    public void CheckBonusHeart()
    {
        UiHeartMinus.SetActive(false); // 하트감소 ui 표시 비활성화
        particleFxResult.SetActive(false); // 하트파티클 비활성화
        UiResult.SetActive(false); // 결과창Ui 비활성화

        if (quizNumCombo >= 5)// 5개 연속으로 성공할 경우
        {
            quizNumComboSucc++; // 콤보성공 횟수를 증가
            ShowDescription();
            
            quizNumCombo = 0; // 성공 콤보 초기화
        } else
        {
            ShowDescription();
        }
        
    }

    // 보너스 하트 ui 닫기 버튼 누르면 페이드 애니메이션
    public void HideBonusHeart ()
    {
        animAdRewardCompo1.animAdFadeOut(2);
    }

    // 보너스하트 Ui 페이드 애니메이션이 끝나면
    public void HideBonusHeart2()
    {
        UiResetCombo.SetActive(false);
        ShowDescription();
    }

    // (퀴즈 성공/실패 / 닫으면) 추가설명 표시 조건 + 힌트 애니메이션 (틀렸을 시)
    public void ShowDescription ()
    {

        if (IsQuizSuccess) // 퀴즈 풀이 성공
        {
            numDepth = 6;           
            UiDescription.SetActive(true); //추가설명을 표시 한다.
        } else // 퀴즈 풀이 실패 : 추가설명 활성화 하지 않음
        {
            numDepth = 3;            
            UiResultDescript.SetActive(false);
            if (numHeart <= 0 && isLevelSubClear == "N") // 하트가 0 보다 적고 클리어 하지 않았다면
            {
                ShowLevelSubClear(); // 클리어 실패 표시
            } else
            {
                // 힌트버튼 애니메이션 재생 (한번만)
                // animHint.SetTrigger("BtnScaleOnce");
            }
        }
        managerSound.instanceSound.SoundClick2();
    }

    // 추가설명 윈도우 닫기(숨김)
    public void HideDescription()
    {
        //numDepth = 3; //퀴즈 상태
        UiDescription.SetActive(false);
        UiResultDescript.SetActive(false);

        managerSound.instanceSound.SoundClick1();
        
        // 서브레벨 10문제 중 'n개'(10개 이상) 풀었을 경우 **
        if (quizNum >= quizNumTotal)
        {
            numDepth = 6;
            ShowLevelSubClear(); // 서브레벨 클리어
        } else if (quizNum < quizNumTotal)
        {
            numDepth = 3;
            quizNum++; //다음 퀴즈번호
            quizCompo.QuizReset(); // 퀴즈ui를 리셋 : 버튼색상을 초기화 함
            ResetQuiz();
        }
    }

    // 서브레벨 클리어 : 현재Score, 카테고리Score 계산 및 표시,
    // + 데이터 저장 + 통계전송(Analytics)
    public void ShowLevelSubClear()
    {
        // 광고유령 비활성화
        mngrAdGhost.AdGhostHide();

        if (numHeart > 0 || isLevelSubClear == "Y") // 하트가 0 보다 크거나 클리어 한 상태라면
        {
            //isCleared = true; // level progress 표시 유무
            // 클리어 Ui구성 표시
            UiImgFx.SetActive(true);

            // 클리어 여부에 따라 "클리어" 또는 "복습완료" 표시
            if (isLevelSubClear == "Y")
            {
                textResultClear.text = "복습완료";
                imgResultClear.color = colorResultSuccess2; // 윈도우 컬러 : 그레이

                // 유니티 애널리틱스 : 복습완료 통계 전송
                //2023.10.29 사용중지
                //managerUnityAnalytics.instanceUnityAnalytics.ReportReviewPlayDone();

            }
            else
            {
                isCleared = true; // level progress 표시 유무
                textResultClear.text = "레벨 클리어!";
                imgResultClear.color = colorResultSuccess1; // 윈도우 컬러 : 그린

                // 해당레벨의 클리어 횟수 카운트
                int levelClearCount = managerData.instanceData.getLevelClearCount(numCatLevel);

                // 업적체크 (카테고리 넘버, 레벨, 클리어갯수)
                // 2019.05.24 임시중단 : 05.25 해결완료

                // 구글 + 게임센터 업적
                archieveCompo.CheckArchievement(numCat, numCatLevel, levelClearCount);

                // 통계정보 전송
                //2023.10.29 사용중지
                //managerUnityAnalytics.instanceUnityAnalytics.ReportSubLevelClear(numCat, numCatLevel, numCatLevelSub);
            }

            imgClearChar.sprite = Resources.Load<Sprite>("imagesChar/quizGhost_clear");
            managerSound.instanceSound.SoundLevelClear();

            //하위 레벨 클리어 반영 여부
            managerData.instanceData.IsClear = true;
            
            // Score 합산
            quizScoreCurrent = quizNumSuccess * (quizScoreDiff * numCatLevel); // 성공점수 = 성공 * (난이도 점수 x 레벨)
            quizScoreFail = quizNumFail * -1; // 실패점수 = 실패횟수 * -1
            quizScoreFinal = quizScoreCurrent + quizScoreFail; // 획득점수 = 성공점수 + 실패점수
            
            // 현재 카테고리 점수에 합산 + 카테고리 배경 이미지 변경
            switch (numCat)
            {
                case 1:
                    quizScoreCatN = quizScoreCatC + quizScoreFinal;
                    imgResultCat.sprite = Resources.Load<Sprite>("ImagesCat/Icon_head_quiz");
                    break;

                case 2:
                    quizScoreCatN = quizScoreCatC + quizScoreFinal;
                    imgResultCat.sprite = Resources.Load<Sprite>("ImagesCat/Icon_proverb");
                    break;

                case 3:
                    quizScoreCatN = quizScoreCatC + quizScoreFinal;
                    imgResultCat.sprite = Resources.Load<Sprite>("ImagesCat/Icon_letterOld");
                    break;

                case 4:
                    quizScoreCatN = quizScoreCatC + quizScoreFinal;
                    imgResultCat.sprite = Resources.Load<Sprite>("ImagesCat/Icon_History");
                    break;
                    
                case 5:
                    quizScoreCatN = quizScoreCatC + quizScoreFinal;
                    imgResultCat.sprite = Resources.Load<Sprite>("ImagesCat/Icon_History");
                    break;
            }

            quizScoreCatTotal = quizScoreCatTotalC + quizScoreFinal; // 모든 카테고리에 점수를 추가
            managerBGM.instanceBgm.PlayBgmClear2(); // Clear2 BGM 재생

            // 복습모드 / 콤보성공 하트보상 여부 검증
            if (isLevelSubClear == "Y") // 복습모드 완료시 +2하트 증정
            {
                caseHeart = 3;
                textHeartGet.text = "복습모드 보상";
                textHeartGetNum.text = "+" + numHeartReview.ToString();
                numHeart = numHeart + numHeartReview;

                numHeartUpdate(); // 하트갯수 업데이트

                UiResetCombo.SetActive(true); // AdRewad Ui 활성화

            } else if (quizNumComboSucc > 0)
            {
                caseHeart = 3;
                textHeartGet.text = "퀴즈 5개 연속 " + quizNumComboSucc.ToString() + "성공!";
                int numHeartsCombo = quizNumComboSucc * numHeartCombo;
                textHeartGetNum.text = "+" + numHeartsCombo.ToString();
                numHeart = numHeart + numHeartsCombo; // 하트를 최종 합산

                numHeartUpdate(); // 하트갯수 업데이트

                UiResetCombo.SetActive(true); // AdRewad Ui 활성화
                managerSound.instanceSound.SoundQuizSuccess();

            } else
            {
                ShowButtonResultClear(); // 닫기버튼을 표시
            }

        }
        else if (numHeart <= 0 && isLevelSubClear == "N") // 하트가 0보다 적고 클리어 하지 않은 상태라면
        {
            isCleared = false; // level progress 표시 유무
            UiImgFx.SetActive(false);
            textResultClear.text = "클리어 실패";
            imgResultClear.color = colorResultFail; // 윈도우 컬러 : 분홍색
            imgClearChar.sprite = Resources.Load<Sprite>("imagesChar/quizGhost_clearNot1");
            ShowButtonResultClear(); // 닫기버튼을 표시

            managerSound.instanceSound.SoundNoHint();
            managerData.instanceData.IsClear = false;
            managerBGM.instanceBgm.PlayBgmClear1(); // Clear1 BGM 재생

            // Unity Analytics 통계전송
            //managerUnityAnalytics.instanceUnityAnalytics.ReportLevelFail(uiTextPlay[0].text, numCatLevel, numCatLevelSub);
        }

        UiResultDescript.SetActive(true);
        UiResultClear.SetActive(true);
        UiTxtGroupScore.SetActive(true);

        // 점수표시 
        // (카테고리 + 레벨 + 서브레벨)
        textClearCatNum.text = string.Format(catName + " " + numCatLevel.ToString() + "-" + numCatLevelSub.ToString());
        
        uiTextResult[2].text = "실패 횟수 : " + quizNumFail.ToString();
        uiTextResult[3].text = "레벨 점수 : " + quizScoreCurrent.ToString();
        uiTextResult[4].text = "실패 감점 : " + quizScoreFail.ToString();

        // 감점을 +n 으로 표시
        int tempScoreFail = quizScoreFail * -1;
        uiTextResult[5].text = "획득점수 : " + quizScoreFinal.ToString();
        uiTextResult[6].text = catName + " 총점 : " + quizScoreCatN.ToString();
        uiTextResult[7].text = "전체총점 : " + quizScoreCatTotal.ToString();

        uiTextResult[0].text = "닫기";
        numDepth = 6;

        // 저장관련 ******************************************
        // 저장관련 ******************
        // 저장하기 전에 중복여부 체크

        managerData.instanceData.numHeart = numHeart; // 하트 갯수를 managerData로 전송
        //카테고리 점수 *************
        //카테고리 점수 전송
        switch (numCat)
        {
            case 1:
                managerData.instanceData.quizScoreCat1 = quizScoreCatN;
                break;

            case 2:
                managerData.instanceData.quizScoreCat2 = quizScoreCatN;
                break;

            case 3:
                managerData.instanceData.quizScoreCat3 = quizScoreCatN;
                break;

            case 4:
                managerData.instanceData.quizScoreCat4 = quizScoreCatN;
                break;

            case 5:
                managerData.instanceData.quizScoreCat5 = quizScoreCatN;
                break;
        }

        quizScoreCatTotalT = quizScoreCatTotal; // 카테고리 선택 화면에서 표시 할 점수
        quizScoreCatTotalC = quizScoreCatTotal; // 결과 화면에서 표시 할 점수
        quizScoreCatC = quizScoreCatN; // 카테고리 총점
        managerData.instanceData.quizScoreTotal = quizScoreCatTotal; // managerData에 종합점수를 저장
        
        //카테고리 점수 *************
        string isClear; // 초기화용
        //Debug.Log("level-sub : " + numCatLevel + "-" + numCatLevelSub);
        isClear = managerData.instanceData.getLevelClear(numCatLevel, numCatLevelSub); // 클리어 유무 확인
        if (isClear == "Y") // 이미 클리어한 레벨이면
        {
            managerData.instanceData.IsOverlap = true; // managerData에 IsOverlap변수 true설정
        }
        else
        {
            managerData.instanceData.IsOverlap = false;
        }
        //Debug.Log("overlap : " + isClear);

        // 초기에 클리어 데이터가 없을경우에 대해서도 처리
        if (isClear == "N" || isClear == "") // 클리어 하지 않은 레벨일 경우 아래 점수, 클리어상태 저장 호출
        {
            managerData.instanceData.DataSave(); // 게임데이터 저장
            // 현재 게임 클리어 유무 처리
            if (g_quizResult) // 현재 스테이즈 클리어 시에만 저장
            {
                managerData.instanceData.ClearDataSave(); // 스테이지 클리어 저장
            }
            // ***** 저장 데이터 병합 *****
            managerData.instanceData.SetCloudSaveData(); // 두개의 (Data, ClearData) 바이너리 string을 하나로 병합
        }
        // 저장관련 ******************
        // 저장관련 ******************************************

        // 구글 랭킹점수 등록        
        if (g_quizResult) // 현재 서브레벨을 클리어 했을 경우
        {
            if (isClear == "N") // 이전에 클리어하지 않은 레벨 일 경우
            {
                
                if (managerGooglePlay.instanceGooglePlay.isAuthenticated) // 로그인이 되어 있다면
                {
                    managerGooglePlay.instanceGooglePlay.ReportScore(quizScoreCatTotal);
                }
                else
                {
                    // 로그인 권장 표시
                }
            }    
        }
    }

    // 클리어표시 중 하트증정 Ui 닫기 페이드 애니메이션
    public void HideBonusHeartResult()
    {
        animAdRewardCompo1.animAdFadeOut(3);
    }

    // 서브레벨 클리어 닫기 버튼 표시 : 하트보상 ui를 닫으면 call을 받음 (animAdReward.cs)
    public void ShowButtonResultClear()
    {
        UiButtonResultClear.SetActive(true);
    }

    // new : 서브레벨 클리어 닫기 전 레벨과정 표시 유무
    public void HideResult()
    {
        if (isCleared && isLevelSubClear == "N") // 클리어 했으면, 버튼 누르면 레벨과정 보여주기
        {
            UiLevelProgress.SetActive(true);
            mngrLevelResult.CalcLevelResult();

            // 레벨업을 했다면 레벨업 보상(하트 +2) 보여주기
            if(mngrLevelResult.isLevelUp)
            {
                Debug.Log("레벨업 보상");
                
                caseHeart = 3;
                UiResetCombo.SetActive(true); // AdRewad Ui 활성화

                textHeartGet.text = "레벨 UP! 보상";
                textHeartGetNum.text = "+" + numHeartLvUp.ToString();
                numHeart = numHeart + numHeartLvUp;

                numHeartUpdate(); // 하트갯수 업데이트

                managerSound.instanceSound.SoundQuizSuccess();
            }

            // 레벨과정 닫기 버튼 활성화

        } else // 아니면 모두 닫기
        {
            HideLevelSubClear();
        }
    }

    // new : 레벨과정 닫기, 그리고 exit
    public void HideLevelProgress()
    {
       
        UiLevelProgress.SetActive(false);
        HideLevelSubClear();
    }
    #endregion

    #region Quiz_SubLevel_Clear
    // 서브레벨 클리어 닫기(숨김) + 퀴즈 플레이 빠져나오기, * 클라우드 (데이터) 저장
    // 전면광고 표시
    public void HideLevelSubClear()
    {
        //string isClear = "N";
        numDepth = 3;
        UiResultClear.SetActive(false);
        UiTxtGroupScore.SetActive(false);
        UiTxtGroupRank.SetActive(false);
        UiButtonResultClear.SetActive(false);

        // 퀴즈답안지 ui를 비활성화
        UiButtons4.SetActive(false);
        UiButtonsOX.SetActive(false);

        UiResultDescript.SetActive(false);

        managerSound.instanceSound.SoundClick1();
                        
        //클라우드 데이터 저장
        if (managerGooglePlay.instanceGooglePlay.isAuthenticated)
        {
            managerGooglePlay.instanceGooglePlay.numCaseLogin = 3;
            managerGooglePlay.instanceGooglePlay.SaveToCloud(managerData.instanceData.saveData);
        }

        ExitQuizPlay(); // 퀴즈에서 빠져 나오기 (Ui)

        // 전면광고 실행 : Google Admob
        managerGoogleAds.instanceGoogleAd.ShowInterstitialAd();
    }

    // 다음 퀴즈 준비 전
    public void ResetQuiz()
    {
        // 퀴즈답안지 ui를 비활성화
        UiButtons4.SetActive(false);
        UiButtonsOX.SetActive(false);

        isHintUse = true; //힌트사용 가능
        IsBackBtnActivate = false;
        UiButtonBackCat.SetActive(false); //Back 버튼 비활성화
        QuizUiUpdate(); //퀴즈번호, 성공, 실패횟수 업데이트
        
        quizCompo.setQuiz();
        ShowQuizCard(quizmgr.getQuizType(quizNum));
    }

    // 모든 퀴즈 변수 초기화
    public void ResetLevelSub()
    {
        UiResetCombo.SetActive(false);
        managerData.instanceData.numHeart = numHeart; //하트갯수를 managerData에 전송
        quizCompo.QuizReset(); // 4지 선다버튼 초기화
        numHintUse = 0; // 힌트사용 횟수 초기화
        quizNum = 1; // 현재 퀴즈번호 초기화
        quizNumSuccess = 0; // 성공한 퀴즈 초기화
        quizNumCombo = 0; // 퀴즈 콤보 초기화
        quizNumComboSucc = 0; // 퀴즈 콤보 성공 초기화 
        quizNumFail = 0; // 실패한 퀴즈 초기화
        isHintUse = true; // 힌트사용 초기화

        quizScoreCurrent = 0;
        quizScoreFail = 0;
        quizScoreFinal = 0;
    }

    public void CheckCloudSave() // 클라우드 세이브 성공했을 경우
    {
        UiMsgCloudSave.SetActive(true);
    }

    public void CheckRanked () // 총점을 랭크에 업데이트 했을 경우
    {
        UiMsgRanked.SetActive(true);
    }
    #endregion
}
