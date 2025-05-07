using StarworkGC.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Game.BattleController;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;


namespace Game
{

    /// <summary>
    /// Centrale ID 0 , Centrale e Destro 0 e 1 (2 nemici), Tutti ecc
    /// </summary>
    /// 

    public class BattleEnemyManager : MonoBehaviour
    {
        [Header("EnemyObject")]
        public GameObject centralEnemyContainer;
        public GameObject rightEnemyContainer;
        public GameObject leftEnemyContainer;
        public GameObject centralEnemy;
        GameObject newCentralEnemy;
        GameObject newCentralEnemyShade;
        public GameObject rightEnemy;
        public GameObject leftEnemy;

        public List<GameObject> enemySpriteDatabase;
        [SerializeField] Button centralButton;
        [SerializeField] Button leftButton;
        [SerializeField] Button rightButton;

        [Header("EnemyAnimator")]
        [SerializeField] Animator centralEnemyAnimator;

        [Header("PanelShowStats")]
        [SerializeField] internal GameObject pnlShowEnemyStats, pnlShowEnemyEffectsStats;
        [SerializeField] internal GameObject[] effectStatsIcon;
        bool pnlStatsOff;

        [Header("EnemyName&Lvl")]
        [SerializeField] TMP_Text centralEnemyName;
        [SerializeField] TMP_Text rightEnemyName;
        [SerializeField] TMP_Text LeftEnemyName;
        [SerializeField] TMP_Text centralEnemyLvl;
        [SerializeField] TMP_Text rightEnemyLvl;
        [SerializeField] TMP_Text LeftEnemyLvl;
        
        [Header("EnemyStatus")]
        [SerializeField] GameObject objLeftStatus;
        [SerializeField] GameObject objCentralStatus;
        [SerializeField] GameObject objRightStatus;

        [SerializeField] BattleController BattleController;
        List<Unit> enemies = new List<Unit>();
        List<GameObject> enemiesList = new List<GameObject>();

        [SerializeField] TMP_Text centralPopupDamage, centralPopupText;
        static float dmg;
        private void Awake()
        {
            clickedTargetName = centralEnemyContainer.name;
            rightEnemyContainer.gameObject.SetActive(false);
            leftEnemyContainer.gameObject.SetActive(false);
            //centralEnemyObj.gameObject.SetActive(false);
            centralEnemyAnimator.gameObject.SetActive(false);
            // centralEnemyName.text = centralEnemy;
        }

        void OnEnable()
        {
            ShowEnemy();
            pnlStatsOff = true;
            centralButton.onClick.RemoveAllListeners();
            centralButton.onClick.AddListener(OnClickOnCentralEnemy);
            leftButton.onClick.RemoveAllListeners();
            leftButton.onClick.AddListener(OnClickOnLeftEnemy);
            rightButton.onClick.RemoveAllListeners();
            rightButton.onClick.AddListener(OnClickOnRightEnemy);
            enemyShieldSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 100);
            enemyShieldSprite.sprite = IconsDatabase.Singleton.GetSpriteIcon(IconsDatabase.QUESTIONMARKICON);
            pnlShowEnemyStats.GetComponent<Animator>().enabled = false;
            pnlShowEnemyStats.GetComponent<CanvasGroup>().alpha = 1;
        }

        GameObject GetSpriteAndAnimator(string enemyName)
        {
            foreach (var item in enemySpriteDatabase)
            {
                if (item.gameObject.name == enemyName)
                {
                    return item;
                }
            }
            return null;
        }

        Animator centralAnimator;
        RuntimeAnimatorController animation;
        Vector3 cEnemyOriginalPosition;
        Color32 cEnemyOriginalColor;
        SpriteRenderer cEnemySprite;
        public void ShowEnemy()
        {
            enemies = BattleController.enemies; // si spacca che nel momento ne rimuovo uno si abbassa e i numeri o 1 2 nn vanno
            enemiesList.Add(centralEnemyContainer);
            enemiesList.Add(leftEnemyContainer);
            enemiesList.Add(rightEnemyContainer);

            switch (BattleController.enemies.Count)
            {
                case 1:
                    Unit cEnemy = BattleController.enemies[0];

                    //rightEnemyContainer.gameObject.SetActive(false);
                    //leftEnemyContainer.gameObject.SetActive(false);
                    ActivateCentralEnemy(); //Spengo e riaccendo l'Animator o non funziona

                    newCentralEnemy = Instantiate(GetSpriteAndAnimator(cEnemy.IDToLoadEnemyPrefab), centralEnemyContainer.transform);
                    pnlShowEnemyStats.gameObject.SetActive(true);
                    newCentralEnemy.name = cEnemy.name;
                    centralAnimator = newCentralEnemy.GetComponent<Animator>();
                    centralAnimator.runtimeAnimatorController = cEnemy.runtimeAnimatorController;
                    cEnemySprite = newCentralEnemy.GetComponentInChildren<SpriteRenderer>();
                    cEnemySprite.sprite = cEnemy.sprite;
                    cEnemySprite.material = cEnemy.normalMap;
                    cEnemySprite.transform.localScale = (Vector3)cEnemy.scale;
                    cEnemySprite.transform.localPosition = (Vector3)cEnemy.localPosition;
                    cEnemyOriginalColor = cEnemySprite.color;
                    CentralEnemyEnterInScene(cEnemySprite);
                    if (cEnemy.enemyShade != null)
                    {
                        ActivateCentralEnemyShadeInScene(cEnemy.enemyShade);
                    }
                    //OMBRA
                    break;
                case 2:
                    centralEnemyContainer.gameObject.SetActive(false);
                    rightEnemyContainer.gameObject.SetActive(true);
                    leftEnemyContainer.gameObject.SetActive(true);
                    LeftEnemyName.gameObject.SetActive(true);
                    LeftEnemyLvl.gameObject.SetActive(true);
                    objLeftStatus.gameObject.SetActive(true);
                    pnlShowEnemyStats.gameObject.SetActive(true);
                    //targetLeft.gameObject.SetActive(true);
                    break;
                case 3:
                    rightEnemyContainer.gameObject.SetActive(true);
                    leftEnemyContainer.gameObject.SetActive(true);
                    centralEnemyContainer.gameObject.SetActive(true);

                    objCentralStatus.gameObject.SetActive(true);
                    centralEnemyName.gameObject.SetActive(true);
                    pnlShowEnemyStats.gameObject.SetActive(true);
                    // targetCentral.gameObject.SetActive(true);
                    //SetEnemiesScaleAndPosition(3);

                    // leftEnemy.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    // leftEnemy.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    break;
                default:
                    break;
            }
        }

        void CentralEnemyEnterInScene(SpriteRenderer sp)
        {
            var sequence = DOTween.Sequence();

            if (newCentralEnemyShade != null)
            {
                sequence.Append(newCentralEnemyShade.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f));
            }

            sequence.Append(cEnemySprite.DOFade(1f, 0.5f));

            cEnemyOriginalPosition = sp.transform.localPosition;
            sp.transform.localPosition = new Vector3(-500f, cEnemyOriginalPosition.y);
            sp.transform.DOLocalMoveX(cEnemyOriginalPosition.x, 1.2f);
        }


        void ActivateCentralEnemyShadeInScene(GameObject enemyShadePrefab)
        {
            //  var sequence = DOTween.Sequence();
            //  sequence.Append(cEnemySprite.DOFade(1f, 0.5f));
            newCentralEnemyShade = Instantiate(enemyShadePrefab, centralEnemyContainer.transform);
            newCentralEnemyShade.SetActive(true);
        }

        public void EnemyAttackDotTweenAnim(Vector3 enemyOriginalPosition, float time = 0)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(cEnemySprite.transform.DOLocalMoveX(enemyOriginalPosition.x + 50f, 0.25f));
            sequence.Append(cEnemySprite.transform.DOLocalMoveX(enemyOriginalPosition.x, 0.25f));
        }

        public IEnumerator EnemyTakeDamageDotTweenAnim(Color enemyOriginalColor, float time = 0)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(cEnemySprite.DOColor(Color.red, 0.2f));
            sequence.Append(cEnemySprite.DOColor(enemyOriginalColor, 0.2f));
            yield return new WaitForSeconds(time);
        }

        IEnumerator EnemyFaintedDotTweenAnimation(Vector3 enemyOriginalPosition)
        {
            var sequence = DOTween.Sequence();

            //sequence.Append(cEnemySprite.transform.DOLocalMoveY(enemyOriginalPosition.y - 150f, 0.5f));
            sequence.Append(cEnemySprite.DOFade(0f, 0.5f));
            if (newCentralEnemyShade != null)
            {
                sequence.Append(newCentralEnemyShade.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f));
            }
            yield return new WaitForSeconds(0.5f);

            DestroyAllEnemiesObject();
        }


        public void DestroyAllEnemiesObject(GameObject target = null)
        {
            if (target == null)
            {
                Destroy(newCentralEnemy);

                if (newCentralEnemyShade != null)
                {
                    Destroy(newCentralEnemyShade);
                }
            }
        }

        public void PrepareCentralEnemyFloatingPopup(Unit unit, float damageAmount, bool damageSourceIsStatus)
        {
            if (!damageSourceIsStatus)
            {
                if (failedToAttack)
                {
                    centralPopupText.text = Localization.Get(LocalizationIDDatabase.MISS_POPUP);
                    centralPopupText.gameObject.SetActive(true);
                    consoleLog = LocalizationIDDatabase.MISS_LOG;
                    return;
                }

                if (damageIsCritical)
                {
                    centralPopupText.text = Localization.Get(LocalizationIDDatabase.CRITICAL_POPUP);
                    centralPopupText.gameObject.SetActive(true);
                    consoleLog = LocalizationIDDatabase.CRITICAL_LOG;
                }
            }

            centralPopupDamage.text = ((int)damageAmount).ToString();
            CheckDamage(damageAmount, centralPopupDamage);
            centralPopupDamage.gameObject.SetActive(true);
        }

        static internal void CheckDamage(float damage, TMP_Text text)
        {
            switch (damage)
            {
                case < 10:
                    text.color = Color.white;
                    break;
                case < 99:
                    text.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.GENERIC_ABILITY_COLOR);
                    break;
                default:
                    text.color = ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.DAMAGECOLOR);
                    break;
            }
        }

        public void DeletedPreviousSprite()
        {
            foreach (Transform child in centralEnemy.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in leftEnemy.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in rightEnemy.transform)
            {
                Destroy(child.gameObject);
            }
        }

        string clickedTargetName;
        public string GetClickedTarget { get => clickedTargetName; }

        public void OnClickOnCentralEnemy()
        {
            CentralEnemyUI();
            //targetCentral.gameObject.SetActive(true);
            //targetLeft.gameObject.SetActive(false);
            //targetRight.gameObject.SetActive(false);
            EnableStatsEnemyPanel();
            Debug.LogFormat("Hai cliccato il nemico centrale che è {0} e ha placeID {1} ", ReturnClickedButton().name, ReturnClickedButton().placeID);
        }
        public void OnClickOnLeftEnemy()
        {
            LeftEnemyUI();
            //targetCentral.gameObject.SetActive(false);
            //targetLeft.gameObject.SetActive(true);
            //targetRight.gameObject.SetActive(false);
            EnableStatsEnemyPanel();
            Debug.LogFormat("Hai cliccato il nemico a sinistra che è {0} e ha placeID {1} ", ReturnClickedButton().name, ReturnClickedButton().placeID);
        }
        public void OnClickOnRightEnemy()
        {
            RightEnemyUI();
            //targetCentral.gameObject.SetActive(false);
            //targetLeft.gameObject.SetActive(false);
            //targetRight.gameObject.SetActive(true);
            EnableStatsEnemyPanel();
            Debug.LogFormat("Hai cliccato il nemico a destra che è {0} e ha placeID {1} ", ReturnClickedButton().name, ReturnClickedButton().placeID);
        }

        public void ChangeEnemyUI(Unit target)
        {
            switch (target.placeID)
            {
                case "CentralEnemy":
                    CentralEnemyUI();
                    EnableStatsEnemyPanel();
                    break;
                case "LeftEnemy":
                    LeftEnemyUI();
                    EnableStatsEnemyPanel();
                    break;
                case "RightEnemy":
                    RightEnemyUI();
                    EnableStatsEnemyPanel();
                    break;
                default:
                    break;
            }
        }


        [SerializeField] internal Image enemyShieldSprite;

        void RightEnemyUI()
        {
            objLeftStatus.gameObject.SetActive(false);
            objRightStatus.gameObject.SetActive(true);
            objCentralStatus.gameObject.SetActive(false);
            clickedTargetName = rightEnemyContainer.name;
        }
        void LeftEnemyUI()
        {
            objLeftStatus.gameObject.SetActive(true);
            objRightStatus.gameObject.SetActive(false);
            objCentralStatus.gameObject.SetActive(false);
            clickedTargetName = leftEnemyContainer.name;
        }
        void CentralEnemyUI()
        {
            objLeftStatus.gameObject.SetActive(false);
            objRightStatus.gameObject.SetActive(false);
            objCentralStatus.gameObject.SetActive(true);
            clickedTargetName = centralEnemyContainer.name;
        }

        public Unit ReturnClickedButton()
        {
            return enemies.Find(u => u.placeID == clickedTargetName);
        }

        public void LastClickedEnemy()
        {
            switch (clickedTargetName)
            {
                case "CentralEnemy":
                    OnClickOnCentralEnemy();
                    break;
                case "LeftEnemy":
                    OnClickOnLeftEnemy();
                    break;
                case "RightEnemy":
                    OnClickOnRightEnemy();
                    break;
                default:
                    OnClickOnCentralEnemy();
                    break;
            }
        }

        public void RemoveTargetFromDeathEnemy()
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemiesList[i].activeSelf)
                {
                    switch (i)
                    {
                        case 0:
                            OnClickOnCentralEnemy();
                            break;
                        case 1:
                            OnClickOnLeftEnemy();
                            break;
                        case 2:
                            OnClickOnRightEnemy();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static Vector3 leftEnemyTransform;
        public static Vector3 rightEnemyTransform;
        public static Vector3 centralEnemyTransform;

        public void AssignUnitPlace(Unit tmpUnit)
        {
            if (tmpUnit.placeID == null) { return; }
            else
            {
                switch (tmpUnit.placeID)
                {
                    case "CentralEnemy":
                        centralEnemyName.text = Localization.Get(tmpUnit.name);
                        centralEnemyLvl.text = UIUtility.GetLevelForUI(tmpUnit.level);
                        centralEnemyTransform = centralEnemyContainer.transform.localPosition;
                        break;
                    case "RightEnemy":
                        rightEnemyName.text = tmpUnit.name;
                        rightEnemyLvl.text = UIUtility.GetLevelForUI(tmpUnit.level);
                        rightEnemyTransform = rightEnemyContainer.transform.localPosition;
                        break;
                    case "LeftEnemy":
                        LeftEnemyName.text = tmpUnit.name;
                        LeftEnemyLvl.text = UIUtility.GetLevelForUI(tmpUnit.level);
                        leftEnemyTransform = leftEnemyContainer.transform.localPosition;
                        break;
                    default:
                        break;
                }
            }
        }

        void EnableStatsEnemyPanel()
        {
            if (pnlStatsOff)
            {
                pnlShowEnemyStats.SetActive(true);
                pnlStatsOff = false;
            }
        }
        public void CleanUIOnCloseBattleView()
        {
            pnlShowEnemyStats.gameObject.SetActive(false);
            //targetCentral.gameObject.SetActive(false);
            //targetLeft.gameObject.SetActive(false);
            //targetRight.gameObject.SetActive(false);
        }

        SpriteRenderer targetSprite = null;
        AudioClip targetDeathScream = null;
        public void EnemyAnimationController(Unit unit, string move = null, bool onlyAttack = false, bool hit = false, bool dodge = false, float duration = 0, float damage = 0)
        {
            targetSprite = null;
            targetDeathScream = null;
            bool CentralEnemy = false;

            switch (unit.placeID)
            {
                case "CentralEnemy":
                    targetSprite = centralEnemy.GetComponentInChildren<SpriteRenderer>();
                    CentralEnemy = true;
                    break;
                
                default:
                    break;
            }

            if (unit.isDead)
            {
                targetDeathScream = unit.battleScreamSound;
                //RunEnemyDeathAnimation(); // da richiamare piu avanti 
                return;
            }

            if (onlyAttack || dodge)
            {
                EnemyAttackDotTweenAnim(targetSprite.transform.localPosition, duration);
                return;
            }
            else if (hit)
            {
                StartCoroutine(EnemyTakeDamageDotTweenAnim(targetSprite.color, duration));
                if (CentralEnemy) { ShakeCentralEnemy.Instance.ShakeObject(damage); }
                return;
            }
            else
               if (CentralEnemy) { PlayCentralEnemyAnimator(move); }
        }

        internal void RunEnemyDeathAnimation()
        {
            
            StartCoroutine(EnemyFaintedDotTweenAnimation(targetSprite.transform.localPosition));
            pnlShowEnemyStats.GetComponent<Animator>().enabled = true;
            SoundEffectManager.Singleton.PlayAudioClip(targetDeathScream);
        }

        void PlayCentralEnemyAnimator(string move)
        {
            centralAnimator.SetTrigger(move);
        }
        public void DeactivateCentralenemy(Unit target)
        {
            EnemyAnimationController(target);
        }
        public void ActivateCentralEnemy()
        {
            centralEnemyContainer.gameObject.SetActive(true);
            StartCoroutine(ActivateAnimator(centralEnemyAnimator));
        }
        IEnumerator ActivateAnimator(Animator animator)
        {
            yield return new WaitForSeconds(0);
            animator.gameObject.SetActive(false);
            animator.gameObject.SetActive(true);
        }

        internal void ActivateEnemyIconStatsEffect()
        {
            pnlShowEnemyEffectsStats.SetActive(true);

            DeactivateEffectStatsIconContainerChild();

            if (BattleController.GetIsBurned)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.burned);
            }
            if (BattleController.GetIsConfused)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.confused);
            }
            if (BattleController.GetIsParalyzed)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.paralyzed, ColorDatabase.Singleton.GetColorByName(LocalizationDatabase.AP_COLOR));
            }
            if (BattleController.GetIsPoisoned)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.poisoned, ColorDatabase.Singleton.GetRarityColor(ScriptableItem.Rarity.Legendary));
            }
            if (BattleController.GetIsFreezed)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.freezed);
            }
            if (BattleController.GetIsInvulnerable)
            {
                AssignSpriteToEmptyImages(PlayerManager.Stats.invurneable);
            }
        }

        internal void DeactivateEffectStatsIconContainerChild()
        {
            foreach (var obj in effectStatsIcon)
            {
                obj.gameObject.SetActive(false);
                Transform childTransform = obj.transform.GetChild(0);
                childTransform.GetComponentInChildren<Image>().sprite = null;
            }
        }

        public void AssignSpriteToEmptyImages(PlayerManager.Stats stat, Color32? color = null)
        {
            Sprite spriteToAssign = IconsDatabase.Singleton.GetEffectStatSpriteByStatusType(stat);

            if (spriteToAssign == null)
            {
                return;
            }

            foreach (GameObject obj in effectStatsIcon)
            {
                if (obj == null)
                    continue;

                Transform childTransform = obj.transform.GetChild(0);
                Image icon = childTransform.GetComponentInChildren<Image>();

                if (icon.sprite == null)
                {
                    obj.SetActive(true);
                    icon.sprite = spriteToAssign;

                    if (color != null)
                    {
                        icon.color = (Color32)color;
                    }
                    break;
                }
            }
        }


        void ShakeAmountAndDurationManager(float damage, bool CentralEnemy = false, bool RightEnemy = false, bool LeftEnemy = false)
        {
            float shakeAmount;
            float shakeDuration;

            switch (damage)
            {
                case > 2000:
                    shakeAmount = 50;
                    shakeDuration = 3;
                    break;
                case > 1000:
                    shakeAmount = 35;
                    shakeDuration = 2.5f;
                    break;
                case > 500:
                    shakeAmount = 20;
                    shakeDuration = 2;
                    break;
                case > 250:
                    shakeAmount = 10;
                    shakeDuration = 1.5f;
                    break;
                case > 100:
                    shakeAmount = 5;
                    shakeDuration = 1;
                    break;
                case > 50:
                    shakeAmount = 3;
                    shakeDuration = 0.5f;
                    break;
                default:
                    shakeAmount = 2;
                    shakeDuration = 0.3f;
                    break;
            }

        }

    }
}
