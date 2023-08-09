using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GameControl : MonoBehaviour
{
    // các button trợ giúp gameplay
    public Button bUndo, bReplay, bHint;
    // layer đen trong suốt
    public GameObject blackLayer;
    // transform của điểm di chuyển theo chuột khi đang kéo line ( vì lúc thả ra nó ở giữa line nên đặt là midPoint)
    public Transform midPoint;
    // line đang chọn
    public Line selectedLine;
    public static GameControl instance;
    // spriteRender của midpoint
    public SpriteRenderer midPointRender;
    //
    public GameObject pointCellPrefap;
    // card = bảng nền các line  , goalcard = bảng hiển thị hình ảnh cần đạt được
    public Transform card, goalCard;
    // khoảng các giữa 2 điểm
    float pointCellDistance = 1.6F;
    // list lưu tất các các pointcell trong card
    public List<PointCell> listPointCell;
    // list lưu tất các các pointcell trong  goalCard
    public List<PointCell> listPointCellGoal;
    // list lưu tất các các line trên card
    public List<Line> listLine;
    // list lưu tất các các line trên goalCard
    public List<Line> listLineGoal;
    //
    public GameObject linePrefap;
    // biến check để khóa click vào line
    public bool canClickLine = true;
    // sprite hiển thị khi 2 dây cùng kiểu cắt nhau
    public GameObject crossSprite;
    // ánh sáng cho hiệu ứng lúc thắng
    public GameObject lightEffect;
    // đếm hint step
    int hintStep = 0;
    // list luu cac gia tri de undo
    public List<UndoModule> listUndoModuleInitLine1, listUndoModuleInitLine2, listUndoModuleMirrorLine, listUndoModuleMain;
    // hint
    public HintLine hintLine;
    bool isHinting = false;
    // điểm mà line hint chỉ tới
    PointCell hintTargetPointCell;
    //topObj = group các object bên trên card trong gameplay
    // bottomObj = group các object ở bên dưới card trong gameplay
    public GameObject topObj, bottomObj;
    // các giá trị ban đầu , giúp làm animation di chuyển
    Vector3 startTopObjPos, startBottomObjPos;
    public RectTransform bottomRect;

    // các text trên topObj
    public TMPro.TextMeshPro tCoinValue, tLevel;
    // text đếm số hint đang có trên nút hint
    public Text tHintCount;
    // list copy của listLineColor trong gameConfig , dùng để tạo 2 color ko trùng nhau
    List<Color> listColorTemp;
    // kiểu của line chính ( line có thể kéo )
    LineType _mainLineType;

    public LineType mainLineType
    {
        get { return _mainLineType; }
        set
        {
            // thay đổi list undo , để chỉ undo line main
            switch (value)
            {
                case LineType.init_line_1:
                    listUndoModuleMain = listUndoModuleInitLine1;
                    break;
                case LineType.init_line_2:
                    listUndoModuleMain = listUndoModuleInitLine2;
                    break;
            }
            //
            _mainLineType = value;
            // set các line không phải line main thành kiểu chìm + color
            for (int i = 0; i < listLine.Count; i++)
            {
                Line l = listLine[i];
                if (l.lineType != mainLineType)
                {
                    l.pc1.isMainType = false;
                    l.pc2.isMainType = false;
                    l.lineRenderer.sortingOrder = 9;
                }
            }
            // set các line là line main thành kiểu nổi + color
            for (int i = 0; i < listLine.Count; i++)
            {
                Line l = listLine[i];
                if (l.lineType == mainLineType)
                {
                    l.pc1.SetColor(l.lineSprite.color);
                    l.pc2.SetColor(l.lineSprite.color);
                    l.pc1.isMainType = true;
                    l.pc2.isMainType = true;
                    l.lineRenderer.sortingOrder = 10;
                }
            }
        }
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
       
        // load data + language
        LoadDataControl.LoadAllLevel(GameMode.Normal);
        LoadDataControl.LoadAllLevel(GameMode.Copy);
        LoadDataControl.LoadAllLevel(GameMode.Double);
        LoadDataControl.LoadLanguage();
        // 
        listUndoModuleInitLine1 = new List<UndoModule>();
        listUndoModuleInitLine2 = new List<UndoModule>();
        listUndoModuleMirrorLine = new List<UndoModule>();
        //PlayerPrefs.DeleteAll();
        
    }

    void Start()
    {
        //GlanceAds.StartAnalytics();
       
        // copy listLineColor sang temp
        listColorTemp = new List<Color>();
        for (int i = 0; i < GameConfig.instance.listLineColors.Count; i++)
        {
            listColorTemp.Add(GameConfig.instance.listLineColors[i]);
        }
        //
        mainLineType = LineType.init_line_1;
        // set một số giá trị ban đầu
        tCoinValue.text = GameManager.dataSave.coinCount.ToString();
        tHintCount.text = GameManager.dataSave.hintCount.ToString();
        startTopObjPos = topObj.transform.position;
        startBottomObjPos = bottomRect.anchoredPosition3D;
        bottomRect.anchoredPosition = new Vector2(0, -1000);
        GameManager.gameState = GameState.SelectLevel;
        listLine = new List<Line>();
        listLineGoal = new List<Line>();
        listPointCell = new List<PointCell>();
        listPointCellGoal = new List<PointCell>();

        // tạo bảng các point
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                // tao các pointCell trên card
                GameObject pointCell = (GameObject)Instantiate(pointCellPrefap, new Vector3(card.position.x - 2 * pointCellDistance + pointCellDistance * x, card.position.y + pointCellDistance * 2 - pointCellDistance * y, 0), Quaternion.Euler(0, 0, 0));
                // đặt pointCell vào trong card 
                pointCell.transform.SetParent(card);
                PointCell pc = pointCell.GetComponent<PointCell>();
                // add vào list lưu các pointCell
                listPointCell.Add(pc);
                pc.id = listPointCell.Count - 1;

                // tạo các pointCell trên goalCard
                GameObject pointCellGoal = (GameObject)Instantiate(pointCellPrefap, new Vector3(goalCard.position.x - 2 * pointCellDistance / 2 + pointCellDistance / 2 * x, goalCard.position.y + pointCellDistance / 2 * 2 - pointCellDistance / 2 * y, 0), Quaternion.Euler(0, 0, 0));
                PointCell pcGoal = pointCellGoal.GetComponent<PointCell>();
                listPointCellGoal.Add(pcGoal);

                pointCellGoal.transform.SetParent(goalCard);
                // scale nhỏ đi 1 nửa 
                pointCellGoal.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
                pcGoal.id = listPointCellGoal.Count - 1;
            }
        }


    }


    public void ClearOldLevel()
    {
        // đặt lại giá trị ban đầu cho một số biến , property
        canClickLine = true;
        midPoint.transform.position = new Vector3(20, 0, 0);
        crossSprite.transform.position = new Vector3(20, 0, 0);
        // clear các list undo
        listUndoModuleInitLine1.Clear();
        listUndoModuleInitLine2.Clear();
        listUndoModuleMirrorLine.Clear();
        hintStep = 0;
        // đặt lại các point cell
        for (int i = 0; i < listPointCell.Count; i++)
        {
            listPointCell[i].isEnablePoint = false;
            listPointCell[i].point.sortingOrder = 10;

        }
        // xóa các line trên card đi
        for (int i = 0; i < listLine.Count; i++)
        {
            Destroy(listLine[i].gameObject);
        }
        // xóa các line trên goalCard
        for (int i = 0; i < listLineGoal.Count; i++)
        {
            Destroy(listLineGoal[i].gameObject);
        }
        // lấy module data của level hiện tại
        GameManager.currenLevelModule = GameManager.LevelDataDict[GameManager.currentGameMode][GameManager.currentLevel];
        //
        listLine.Clear();
        listLineGoal.Clear();
        // sắp xếp ngẫu nhiên listColorTemp để lấy lại 2 giá trị color ngẫu nhiên mới
        listColorTemp.Shuffle();

        GameConfig.mainColor = listColorTemp[0];
        GameConfig.mainColor2 = listColorTemp[1];
        // riêng màn 1 chế độ double , phải set color cho 2 line thành đỏ và xanh 
        if (GameManager.currentGameMode == GameMode.Double && GameManager.currentLevel == 1)
        {
            GameConfig.mainColor = GameConfig.instance.listLineColors[0];
            GameConfig.mainColor2 = GameConfig.instance.listLineColors[1];
        }

        midPointRender.color = GameConfig.mainColor;
        //
        tLevel.text = LanguageManager.GetText(LanguageKey.level) + GameManager.currentLevel;
    }

    public void StartNewLevel()
    {
        if (PlayerPrefs.GetInt("firstGlance") == 1)
        {
            GlanceAds.ReplayAnalytics(GameManager.currentLevel);
            GlanceAds.LevelAnalytics(GameManager.currentLevel);
        }
        else
        {
            PlayerPrefs.SetInt("firstGlance", 1);
            GlanceAds.StartAnalytics();
            GlanceAds.LevelAnalytics(GameManager.currentLevel);

        }
        // khi bat dau vao choi thi bat am thanh bg game music len 
        AudioManager.PlayMusic(Random.Range(0, 100) > 50 ? AudioClipType.AC_BGM_GAME_1 : AudioClipType.AC_BGM_GAME_2);
        StopHint();
        ClearOldLevel();
        InitGoalLines();
        //top move effect
        topObj.transform.position = new Vector3(0, 20, 0);
        topObj.transform.DOMoveY(startTopObjPos.y, 0.5F).SetEase(Ease.OutQuad).SetDelay(1F);
        // bottom move effect
        bottomRect.anchoredPosition = new Vector2(0, -1000);
        bottomRect.DOLocalMoveY(startBottomObjPos.y, 0.5F).SetEase(Ease.OutQuad).SetDelay(1F);
        // card move effect;
        card.transform.localPosition = new Vector3(20, -0.78F, 0);
        card.DOLocalMoveX(0, 1F).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // tạo level mới
            InitNewtLevel();
            // hiện tutorial nếu có
            if (GameManager.currentLevel == 1 && GameManager.currentGameMode == GameMode.Normal)
            {
                GameConfig.instance.tutorialControl.StartTutorial(GameManager.currentGameMode);
            }
            if (GameManager.currentLevel == 1 && GameManager.currentGameMode == GameMode.Double)
            {
                GameConfig.instance.tutorialControl.StartTutorial(GameManager.currentGameMode);
            }
        });
    }

    public void NextLevel()
    {
        StopHint();
        ClearOldLevel();
        InitGoalLines();
        // hiệu ứng card di chuyển khi next level
        card.DOLocalMoveX(-12, 0.5F).OnComplete(() =>
        {
            card.transform.localPosition = new Vector3(12, -0.78F, 0);
            card.DOLocalMoveX(0, 1F).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                InitNewtLevel();
            });
        });
    }

    public void RestartLevel()
    {
        GlanceAds.ReplayAd("replay");
        GlanceAds.EndAnalytics(GameManager.currentLevel);
        GlanceAds.ReplayAnalytics(GameManager.currentLevel);
        GlanceAds.LevelAnalytics(GameManager.currentLevel);       
        // chỉ cho restart khi đang không có tutorial
        if (GameConfig.instance.tutorialControl.haveTutorial == false)
        {
            AudioManager.PlaySound(AudioClipType.AC_BUTTON);
            StopHint();
            ClearOldLevel();
            InitGoalLines();
            InitNewtLevel();
        }
    }

    void InitGoalLines()
    {
        // goal line 
        for (int i = 0; i < GameManager.currenLevelModule.goal_line.Count; i++)
        {

            List<List<int>> goalLine = GameManager.currenLevelModule.goal_line;

            Vector3 point1 = listPointCellGoal[goalLine[i][0]].transform.position;
            Vector3 point2 = listPointCellGoal[goalLine[i][1]].transform.position;

            Line l = CreateLine(point1, point2, 0.1F);

            l.lineRenderer.sortingOrder = 2;
            l.isBeizer = false;
            l.transform.SetParent(goalCard);
            // set 2 pointcell của goalline là 2 pointcell trên card để dễ check thắng 
            l.pc1 = listPointCell[goalLine[i][0]];
            l.pc2 = listPointCell[goalLine[i][1]];
            //
            listLineGoal.Add(l);
            l.lineType = LineType.goal_line;


        }
    }

    public void InitNewtLevel()
    {


        // init_line
        for (int i = 0; i < GameManager.currenLevelModule.init_line.Count; i++)
        {
            List<List<int>> initLine = GameManager.currenLevelModule.init_line;

            PointCell point1 = listPointCell[initLine[i][0]];
            PointCell point2 = listPointCell[initLine[i][1]];
            Line l = CreateLine(point1, point2, 0.2F);
            l.isBeizer = false;
            listLine.Add(l);
            //hiệu ứng line dài dần
            Vector3 pos = l.point2;
            l.point2 = point1.transform.position;
            DOTween.To(() => l.point2, x => l.point2 = x, pos, 1F).SetEase(Ease.OutCirc).SetDelay(0.5F);

            l.lineType = LineType.init_line_1;

        }
        // init_line_2 nếu có
        if (GameManager.currenLevelModule.init_line_2 != null && GameManager.currenLevelModule.init_line_2.Count > 0)
        {
            for (int i = 0; i < GameManager.currenLevelModule.init_line_2.Count; i++)
            {
                List<List<int>> initLine = GameManager.currenLevelModule.init_line_2;

                PointCell point1 = listPointCell[initLine[i][0]];
                PointCell point2 = listPointCell[initLine[i][1]];
                Line l = CreateLine(point1, point2, 0.2F);
                l.isBeizer = false;
                listLine.Add(l);
                Vector3 pos = l.point2;
                l.point2 = point1.transform.position;
                // hiệu ứng line dài dần
                DOTween.To(() => l.point2, x => l.point2 = x, pos, 1F).SetEase(Ease.OutCirc).SetDelay(0.5F);
                l.lineType = LineType.init_line_2;

            }
        }
        mainLineType = LineType.init_line_1;
    }


    public Line CreateLine(PointCell p1, PointCell p2, float width)
    {
        // tạo  line theo 2 pointCell
        p1.isEnablePoint = true;
        p2.isEnablePoint = true;


        Line l = CreateLine(p1.transform.position, p2.transform.position, width);
        l.transform.SetParent(card.transform);
        l.pc1 = p1;
        l.pc2 = p2;
        return l;
    }

    public Line CreateLine(Vector3 poin1, Vector3 point2, float width = 0.2F)
    {
        // tạo line theo 2 vector 
        GameObject lineO = (GameObject)Instantiate(linePrefap, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        Line l = lineO.GetComponent<Line>();
        l.point1 = poin1;
        l.point2 = point2;
        l.width = width;
        return l;
    }

    void ClearLine(PointCell pc1, PointCell pc2)
    {
        // xóa 1 line theo 2 pointCell
        for (int i = 0; i < listLine.Count; i++)
        {
            Line l = listLine[i];
            if (l.lineType != mainLineType)
            {
                continue;
            }
            if ((l.pc1 == pc1 && l.pc2 == pc2) || (l.pc1 == pc2 && l.pc2 == pc1))
            {
                ClearLine(l);
                break;
            }
        }
    }

    void ClearLine(Line line)
    {
        // xóa 1 line 
        if (line.lineType != mainLineType)
        {
            return;
        }
        listLine.Remove(line);
        Destroy(line.gameObject);
    }
    // fingerDistance = khoảng cách từ chuột đến midPoint
    Vector3 fingerDistance = new Vector3(0, 1, 0);

    // temp var , dùng để check thay đổi hintCount,coin qua đó thay đổi text hiển thị
    int coinTemp = -1;
    int hintCountTemp = -1;

    void Update()
    {
        if (PlayerPrefs.HasKey("DoneReplay"))
        {
            PlayerPrefs.DeleteKey("DoneReplay");
            if (PlayerPrefs.GetInt("InitialMusic") == 1)
            {
                AudioManager.SetMusicStatus(true);
            }
            if (PlayerPrefs.GetInt("InitialSound") == 1)
            {
                AudioManager.SetMusicStatus(true);
            }
        }
        if (GameManager.gameState == GameState.Playing)
        {
            bool canCreateNewLine = false;
            if (selectedLine != null)
            {
                // lấy vị trí chuôt
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                // nhích lên 1 đoạn để tránh bị ngón tay che và có thể nhìn thấy khi chơi trên đt
                pos += fingerDistance;
                // tìm vị trí poinCell gần nhất midPoint + khoảng cách
                GetNearestPointCell(pos);

                if (minDistance <= 0.5F)
                {
                    if (nearestPointCell != selectedLine.pc1 && nearestPointCell != selectedLine.pc2)
                    {
                        canCreateNewLine = true;
                    }

                    midPoint.position = nearestPointCell.transform.position;
                }
                else
                {
                    midPoint.position = pos;

                }
                //
                selectedLine.midPoint = midPoint.position;
                //hiện midpoint nếu đang ẩn
                if (midPointRender.enabled == false)
                {
                    midPointRender.enabled = true;
                }
            }
            else
            {
                // ẩn midpoint đi nếu đang ko chọn line nào
                if (midPointRender.enabled == true)
                {
                    midPointRender.enabled = false;
                }
            }

            ///////////
            if (Input.GetMouseButtonUp(0))
            {
                // 
                midPointRender.enabled = false;
                crossSprite.transform.position = new Vector3(9999, 0, 0);
                //
                if (selectedLine != null)
                {
                    // check tutorial
                    if (GameConfig.instance.tutorialControl.haveTutorial)
                    {
                        PointCell pcTarget = GameConfig.instance.tutorialControl.pcTarget;
                        PointCell pc1 = GameConfig.instance.tutorialControl.pc1;
                        PointCell pc2 = GameConfig.instance.tutorialControl.pc2;
                        // đặt không đúng điểm cần trong tutorial thì không cho đặt xuống
                        if (nearestPointCell != pcTarget || !isLineAMatchLineB(pc1, pc2, selectedLine.pc1, selectedLine.pc2))
                        {
                            canCreateNewLine = false;
                        }
                        else
                        {
                            // nếu thỏa mãn tutorial thì sang bước tiếp
                            if (canCreateNewLine)
                            {
                                GameConfig.instance.tutorialControl.tutorialStep++;
                            }
                        }
                    }
                    selectedLine.OnMouseRelease(canCreateNewLine);
                    selectedLine = null;
                    // control hint 
                    if (isHinting && hintTargetPointCell != null)
                    {
                        if (canCreateNewLine)
                        {
                            if (nearestPointCell == hintTargetPointCell)
                            {
                                hintStep++;
                            }
                            else
                            {
                                StopHint();
                            }
                        }
                    }
                }
            }
            // neu click chuot phai - tren dt la double click thi thay doi main type 
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                if (GameManager.currentGameMode == GameMode.Double)
                {
                    mainLineType = mainLineType == LineType.init_line_2 ? LineType.init_line_1 : LineType.init_line_2;
                    AudioManager.PlaySound(AudioClipType.AC_CHANGE_LINE);
                    if (GameManager.currentGameMode == GameMode.Double && GameConfig.instance.tutorialControl.haveTutorial)
                    {
                        if (GameConfig.instance.tutorialControl.tutorialStep == 1)
                        {
                            GameConfig.instance.tutorialControl.tutorialStep++;
                        }
                    }
                }
            }
#elif UNITY_ANDROID || UNITY_IOS
			for (var i = 0; i < Input.touchCount; ++i)
			{
			if (Input.GetTouch(i).phase == TouchPhase.Began) 
			{
			if(Input.GetTouch(i).tapCount == 2)
			{
			// double tap :)
			if (GameManager.currentGameMode == GameMode.Double) {
			mainLineType = mainLineType == LineType.init_line_2 ? LineType.init_line_1 : LineType.init_line_2;
			AudioManager.PlaySound (AudioClipType.AC_CHANGE_LINE);
			if (GameManager.currentGameMode == GameMode.Double && GameConfig.instance.tutorialControl.haveTutorial) {
			if (GameConfig.instance.tutorialControl.tutorialStep == 1) {
			GameConfig.instance.tutorialControl.tutorialStep++;
			}
			}
			}
			}
			}
			}
#endif
        }

        // update coin text 
        if (coinTemp != GameManager.dataSave.coinCount)
        {
            coinTemp = GameManager.dataSave.coinCount;
            tCoinValue.text = coinTemp.ToString();
        }
        // update hint text
        if (hintCountTemp != GameManager.dataSave.hintCount)
        {
            hintCountTemp = GameManager.dataSave.hintCount;
            tHintCount.text = hintCountTemp.ToString();

        }
    }

    //--------------
    public PointCell nearestPointCell;
    float minDistance;

    void GetNearestPointCell(Vector3 pos)
    {
        float minDis = 100;
        for (int i = 0; i < listPointCell.Count; i++)
        {
            float distance = Vector3.Distance(listPointCell[i].transform.position, pos);
            if (distance < minDis)
            {
                minDis = distance;
                minDistance = distance;
                nearestPointCell = listPointCell[i];

            }
        }
    }
    //-----------------



    public void CheckWin2()
    {


        // so sánh các line trên card với line trên goal
        bool isWin = true;
        for (int i = 0; i < listLine.Count; i++)
        {
            Line lineCheck = listLine[i];
            bool haveMatch = false;
            for (int j = 0; j < listLineGoal.Count; j++)
            {
                Line lineGoal = listLineGoal[j];
                bool isMatch = isLineAMatchLineB(lineGoal.pc1, lineGoal.pc2, lineCheck.pc1, lineCheck.pc2);
                bool isPart = LineAIsPartOfLineB(lineCheck.pc1, lineCheck.pc2, lineGoal.pc1, lineGoal.pc2);
                bool isRepart = LineAIsPartOfLineB(lineGoal.pc1, lineGoal.pc2, lineCheck.pc1, lineCheck.pc2);
                if (isMatch || isPart || isRepart)
                {
                    haveMatch = true;
                }


            }
            // neu nhu line dang check ko match voi line goal nao va cung ko la part cua line goal nao thi ko win
            if (haveMatch == false)
            {
                isWin = false;
                break;
            }
        }

        // so sánh các line trên goal với trên card 
        bool isWin2 = true;
        for (int i = 0; i < listLineGoal.Count; i++)
        {
            Line lineCheck = listLineGoal[i];
            bool haveMatch = false;
            for (int j = 0; j < listLine.Count; j++)
            {
                Line lineGoal = listLine[j];
                bool isMatch = isLineAMatchLineB(lineGoal.pc1, lineGoal.pc2, lineCheck.pc1, lineCheck.pc2);
                bool isPart = LineAIsPartOfLineB(lineCheck.pc1, lineCheck.pc2, lineGoal.pc1, lineGoal.pc2);
                bool isRepart = LineAIsPartOfLineB(lineGoal.pc1, lineGoal.pc2, lineCheck.pc1, lineCheck.pc2);
                if (isMatch || isPart || isRepart)
                {
                    haveMatch = true;
                }


            }
            if (haveMatch == false)
            {
                isWin2 = false;
                break;
            }
        }

        // nếu 2 lần so sánh đều ra kết quả match thì return win

        if (isWin && isWin2)
        {
            GlanceAds.LevelCompletedAnalytics(GameManager.currentLevel);
            StartCoroutine(WinCoroutine());
        }
    }
    //-------------------------------------------
    // check 2 line là line con của nhau dựa trên các vector điểm
    public bool LineAIsPartOfLineB(Vector3 A1, Vector3 A2, Vector3 B1, Vector3 B2)
    {
        bool checkPcA1 = IsCBetweenAB(B1, B2, A1);
        bool checkPcA2 = IsCBetweenAB(B1, B2, A2);
        if (checkPcA1 && checkPcA2)
        {
            return true;
        }
        if ((A1 == B1 && checkPcA2) || (A1 == B2 && checkPcA2))
        {
            return true;
        }
        if ((A2 == B1 && checkPcA1) || (A2 == B2 && checkPcA1))
        {
            return true;
        }
        return false;
    }
    //---------------------------------
    // check 2 line là line con của nhau dựa trên các pointCell
    public bool LineAIsPartOfLineB(PointCell pcA1, PointCell pcA2, PointCell pcB1, PointCell pcB2)
    {
        return LineAIsPartOfLineB(pcA1.transform.position, pcA2.transform.position, pcB1.transform.position, pcB2.transform.position);
    }
    //-----------------------------------
    // check 2 line trùng nhau
    public bool isLineAMatchLineB(PointCell pcA1, PointCell pcA2, PointCell pcB1, PointCell pcB2)
    {
        if ((pcA1 == pcB1 && pcA2 == pcB2) || (pcA1 == pcB2 && pcA2 == pcB1))
        {
            return true;
        }
        return false;
    }
    //-----------------------------------
    // hiệu ứng thắng
    IEnumerator WinCoroutine()
    {
        // thay đổi line trên card thành line thắng (hiện lên trên backlayer , bật mask....)
        for (int i = 0; i < listLine.Count; i++)
        {
            listLine[i].lineRenderer.enabled = false;
            listLine[i].lineSprite.enabled = true;
            listLine[i].pc1.point.sortingOrder = 14;
            listLine[i].pc2.point.sortingOrder = 14;
            listLine[i].pc1.mask.enabled = true;
            listLine[i].pc2.mask.enabled = true;
            listLine[i].mask.enabled = true;
        }
        blackLayer.SetActive(true);
        // hiệu ứng sáng 
        lightEffect.transform.position = new Vector3(-12, 0, 0);
        lightEffect.transform.DOMoveX(12, 4F);
        //
        yield return new WaitForSeconds(2F);
        //
        AudioManager.PlaySound(AudioClipType.AC_FINISH);
        GameManager.gameState = GameState.Win;
    }

    //-----------------------
    // check xem 3 điểm có thẳng hàng không và điểm check có nằm giữa 2 điểm còn lại không
    bool IsCBetweenAB(Vector3 point1, Vector3 point2, Vector3 pointCheck)
    {
        if (pointCheck == point1 || pointCheck == point2 || point1 == point2)
        {
            return false;
        }

        float d1 = Vector3.Distance(point1, pointCheck);
        float d2 = Vector3.Distance(pointCheck, point2);
        float d3 = Vector3.Distance(point1, point2);
        if (d3 - 0.01F < d1 + d2 && d3 + 0.01F > d1 + d2)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    // check và lấy điểm giao nhau của 2 line theo vector các điểm
    public Vector3 CrossPoint(Vector3 p1, Vector3 p2, Vector3 pCheck1, Vector3 pCheck2)
    {
        bool isOneLine = (p1 == pCheck1 && p2 == pCheck2) || (p1 == pCheck2 && p2 == pCheck1) ? true : false;

        if (isOneLine)
        {
            // return diem chinh giua cua 1 trong 2 line
            return new Vector3((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, 0);
        }

        // 
        bool isPart1 = LineAIsPartOfLineB(p1, p2, pCheck1, pCheck2);
        if (isPart1)
        {

            return new Vector3((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, 0);
        }

        bool isPart2 = LineAIsPartOfLineB(pCheck1, pCheck2, p1, p2);
        if (isPart2)
        {

            return new Vector3((pCheck1.x + pCheck2.x) / 2, (pCheck1.y + pCheck2.y) / 2, 0);
        }

        // vector 9999 == khong co diem giao nhau thoa man
        // neu 2 diem cua 1 doan thang la 1 diem thi return luon diem do

        // tinh toan he phuong trinh
        float a1 = (p2.y - p1.y) / (p2.x - p1.x);

        float b1 = p1.y - a1 * p1.x;

        float a2 = (pCheck2.y - pCheck1.y) / (pCheck2.x - pCheck1.x);

        float b2 = pCheck1.y - a2 * pCheck1.x;

        if (a1 == a2)
        {

            return new Vector3(9999, 9999, 9999);

        }
        Vector3 crossPoint;
        float crossX = (b2 - b1) / (a1 - a2);
        float crossY = a1 * crossX + b1;
        if (p2.x == p1.x && pCheck1.x != pCheck2.x)
        {
            crossPoint = new Vector3(p1.x, p1.x * a2 + b2, 0);

        }
        else if (p2.x != p1.x && pCheck1.x == pCheck2.x)
        {
            crossPoint = new Vector3(pCheck1.x, pCheck1.x * a1 + b1, 0);
        }
        else
        {
            crossPoint = new Vector3(crossX, crossY, p1.z);
        }
        //return crossPoint;
        if (IsCBetweenAB(p1, p2, crossPoint) && IsCBetweenAB(pCheck1, pCheck2, crossPoint))
        {
            return crossPoint;
        }
        else
        {
            return new Vector3(9999, 9999, 9999);
        }
    }
    //---------------------------------------------------------------

    #region Hint region

    IEnumerator hintCrt;

    public void Hint()
    {
        if(GlanceAds.instance != null)
        {
            GlanceAds.RewardedAdsAnalytics("HintReward", "CancelHintReward");
            GlanceAds.RewardedAd("Hint");
        }
        if (GameConfig.instance.tutorialControl.haveTutorial)
        {
            return;
        }
        AudioManager.PlaySound(AudioClipType.AC_BUTTON);
        if (GameManager.dataSave.hintCount > 0)
        {
            GameManager.dataSave.hintCount--;
            GameManager.SaveData();
            hintCrt = HintCoroutine();
            StartCoroutine(hintCrt);
        }
        else
        {
            GameManager.gameState = GameState.Shopping;
        }
    }

    public void StopHint()
    {
        if (hintCrt != null)
        {
            StopCoroutine(hintCrt);
        }
        hintLine.ClearHintLine();
        hintStep = 0;
        isHinting = false;
    }

    IEnumerator HintCoroutine()
    {
        isHinting = true;
        // nếu người chơi đã đi 1 hoặc nhiều nước rồi thì reset lại từ đầu
        if (listUndoModuleInitLine1.Count > 0 || listUndoModuleInitLine2.Count > 0)
        {

            ClearOldLevel();
            InitGoalLines();
            InitNewtLevel();
            yield return new WaitForSeconds(1F);
        }
        int totalHintCount = GameManager.LevelDataDict[GameManager.currentGameMode][GameManager.currentLevel].tips.Count;
        int oldHintCount = 0;
        if (totalHintCount > 0)
        {

            hintStep = 0;

            while (hintStep < totalHintCount)
            {
                if (oldHintCount == hintStep)
                {
                    List<int> listHintPoints = GameManager.LevelDataDict[GameManager.currentGameMode][GameManager.currentLevel].tips[hintStep];
                    PointCell pc1 = listPointCell[listHintPoints[0]];
                    PointCell pc2 = listPointCell[listHintPoints[1]];
                    PointCell pcTarget = listPointCell[listHintPoints[2]];
                    hintLine.CreateHintLine(pc1, pc2, pcTarget);
                    hintTargetPointCell = pcTarget;
                    oldHintCount++;
                }
                else
                {
                    yield return null;
                }

            }
        }
        else
        {
            Debug.Log("Khong co hint 1 ");
        }

        // lấy hint 2 nếu có
        List<List<int>> tips_2 = GameManager.LevelDataDict[GameManager.currentGameMode][GameManager.currentLevel].tips_2;

        if (tips_2 != null && tips_2.Count > 0)
        {
            hintStep = 0;
            oldHintCount = hintStep;
            totalHintCount = tips_2.Count;

            while (hintStep < totalHintCount)
            {
                if (oldHintCount == hintStep)
                {
                    List<int> listHintPoints = tips_2[hintStep];
                    PointCell pc1 = listPointCell[listHintPoints[0]];
                    PointCell pc2 = listPointCell[listHintPoints[1]];
                    PointCell pcTarget = listPointCell[listHintPoints[2]];
                    hintLine.CreateHintLine(pc1, pc2, pcTarget);
                    hintTargetPointCell = pcTarget;
                    oldHintCount++;
                }
                else
                {
                    yield return null;
                }

            }
        }
        // hint xong thì ẩn hintline đi
        hintLine.ClearHintLine();
        isHinting = false;
    }

    #endregion

    //-------------------------------------------------------

    #region Undo

    public void Undo()
    {
        if(GlanceAds.instance != null)
        {
            GlanceAds.RewardedAdsAnalytics("UndoReward", "CancelUndoReward");
            GlanceAds.RewardedAd("Undo");
        }
        if (GameConfig.instance.tutorialControl.haveTutorial == true)
        {
            // 
            return;
        }

        AudioManager.PlaySound(AudioClipType.AC_BUTTON);
        if (listUndoModuleMain.Count <= 0)
        {
            return;
        }
        // lấy giá trị undo gần nhất dc thêm vào
        UndoModule undo = listUndoModuleMain[listUndoModuleMain.Count - 1];

        // xóa 2 line mới tạo thành
        ClearLine(undo.pc1, undo.pcNew);
        ClearLine(undo.pc2, undo.pcNew);
        // đặt lại color (dùng cho chế độ 2 color)
        undo.pcNew.isMainType = undo.pcNewIsMainType;
        undo.pcNew.SetColor(undo.pcNewColor);
        //
        if (undo.clearDot)
        {
            undo.pcNew.isEnablePoint = false;
        }

        // tạo lại line bị xóa
        Line l = CreateLine(undo.pc1, undo.pc2, 0.2F);
        listLine.Add(l);
        midPoint.position = undo.pcNew.transform.position;
        l.haveMidPoint = true;
        l.midPoint = undo.pcNew.transform.position;
        l.lineType = undo.lineType;
        // hiệu ứng rung dây
        l.OnMouseRelease(false);

        if (isHinting)
        {
            StopHint();
        }
        // remove undo module
        listUndoModuleMain.Remove(undo);
    }

    #endregion
}

[System.Serializable]
public class UndoModule
{
    public PointCell pc1, pc2, pcNew;
    public bool clearDot = true;
    public LineType lineType;
    public bool pcNewIsMainType;
    public Color pcNewColor;

}

[System.Serializable]
public class LineTemplate
{
    public PointCell pc1, pc2;
}