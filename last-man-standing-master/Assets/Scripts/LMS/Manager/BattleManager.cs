using BitBenderGames;
using GAMO.Common;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LMS.Model;
using LMS.UI;

namespace LMS.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;
        public static List<UnitObject> ListUnitInBattle;
        private static bool isPlaying = false;
        public static bool IsPlaying
        {
            get { return isPlaying; }
        }
        private static bool isCinematic = false;
        public static bool IsCinematic
        {
            get { return isCinematic; }
        }
        private static bool isPause = false;
        public static bool IsPause
        {
            get { return isPause; }
        }
        public Transform DropSkillParent;
        [SerializeField]
        private MobileTouchCamera MobileCamera;
        [SerializeField]
        private BoxCollider SpawnBoxCollider;
        [SerializeField]
        private BattleSurvivalStage BattleStage;
        [Header("Reference")]
        [SerializeField]
        private Button BtnPause;
        [SerializeField]
        private Button BtnResume;
        [SerializeField]
        private Button BtnHomeMenu;
        [SerializeField]
        private GameObject PauseMenu;
        [SerializeField]
        private BattleResultDialog BattleResultMenu;
        [SerializeField]
        private Transform GroupToggleUnit;
        [SerializeField]
        private List<UIToggleUnitController> listToggleUnit = new List<UIToggleUnitController>();
        [SerializeField]
        private TouchInputController TouchController;
        [SerializeField]
        private Transform GroupBtnSkill;
        [SerializeField]
        private List<UISpellSlotController> listBtnSkill = new List<UISpellSlotController>();

        public UnitObject playerSelectedUnit;
        public Camera MainCamera
        {
            get { return MobileCamera.GetComponent<Camera>(); }
        }


        //private bool
        private float _timingPowerUp = 0;
        private float _timingHoldingSpawn = 0;
        private float _timeInGame = 0;
        private int _enemyIndex = 1;
        private float MapWidth = 7;
        private float MapHeight = 4;

        private StageScript currentStageScript;
        public PlayerStats userPlayer;
        public PlayerStats enemyPlayer;
        private List<PlayerStats> listPlayer;

        #region Static
        public static bool ColliderIsCharacter(Collider2D col, GameObject checker)
        {
            if (col == null || checker == null) return false;
            return col.gameObject != checker &&
                (col.gameObject.CompareTag(CONSTANT.TagUnit) || col.gameObject.CompareTag(CONSTANT.TagBuilding));
        }

        public static bool IsEnemyAndAlive(GameObject obj, int teamIndex)
        {
            if (obj == null) return false;
            UnitObject charObj = obj.GetComponent<UnitObject>();
            if (charObj == null) return false;
            return charObj.TeamIndex != teamIndex && charObj.TeamIndex != 0 && charObj.IsAlive;
        }

        public static bool IsAlias(GameObject obj, int teamIndex)
        {
            if (obj == null) return false;
            UnitObject charObj = obj.GetComponent<UnitObject>();
            if (charObj == null) return false;
            return charObj.TeamIndex == teamIndex && charObj.TeamIndex != 0;
        }

        public static bool IsEnemy(GameObject obj, int teamIndex)
        {
            if (obj == null) return false;
            UnitObject charObj = obj.GetComponent<UnitObject>();
            if (charObj == null) return false;
            return charObj.TeamIndex != teamIndex && charObj.TeamIndex != 0;
        }

        public static bool IsAlive(GameObject obj)
        {
            if (obj == null) return false;
            UnitObject charObj = obj.GetComponent<UnitObject>();
            if (charObj == null) return false;
            return charObj.IsAlive;
        }

        public static bool IsBuilding(GameObject obj)
        {
            if (obj == null) return false;
            UnitObject charObj = obj.GetComponent<UnitObject>();
            if (charObj == null) return false;
            return charObj.unitScript.Type == UnitScript.UnitType.Building;
        }

        public static GameObject CreatePrefab(string path)
        {
            GameObject obj = SimplePool.Spawn(Resources.Load(path) as GameObject, CONSTANT.SpawnHidePosition);
            return obj;
        }

        public static GameObject CreateVFX(string name)
        {
            return SimplePool.Spawn(Resources.Load(string.Format(CONSTANT.PathVFXPrefabs, name)) as GameObject, CONSTANT.SpawnHidePosition);
        }
        public static void DestroyGameObject(GameObject obj)
        {
            SimplePool.Despawn(obj);
        }

        #endregion
        #region Find Target Func
        public static List<UnitObject> FindEnemyInRadius(int teamIndex, Vector2 position, float radius)
        {
            List<UnitObject> _listUnit = new List<UnitObject>();
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                UnitObject unit = ListUnitInBattle[i];
                if (unit.TeamIndex != teamIndex)
                {
                    if (unit.IsAlive)
                    {
                        float _dis = Vector2.Distance(new Vector2(position.x, position.y / CONSTANT.ScaleAxisY), new Vector2(unit.transform.position.x, unit.transform.position.y / CONSTANT.ScaleAxisY));
                        if (_dis <= radius)
                        {
                            _listUnit.Add(unit);
                        }
                    }
                }
            }
            return _listUnit;
        }

        public static List<UnitObject> FindAliasInRadius(int teamIndex, Vector2 position, float radius)
        {
            List<UnitObject> _listUnit = new List<UnitObject>();
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                UnitObject unit = ListUnitInBattle[i];
                if (unit.TeamIndex == teamIndex)
                {
                    if (unit.IsAlive)
                    {
                        float _dis = Vector2.Distance(new Vector2(position.x, position.y / CONSTANT.ScaleAxisY), new Vector2(unit.transform.position.x, unit.transform.position.y / CONSTANT.ScaleAxisY));
                        if (_dis <= radius)
                        {
                            _listUnit.Add(unit);
                        }
                    }
                }
            }
            return _listUnit;
        }

        public static List<UnitObject> FindUnitObjectInRadius(System.Predicate<UnitObject> predicate, Vector2 position, float range)
        {
            List<UnitObject> _listUnit = new List<UnitObject>();
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                UnitObject unit = ListUnitInBattle[i];
                float _dis = Vector2.Distance(new Vector2(position.x, position.y / CONSTANT.ScaleAxisY), new Vector2(unit.transform.position.x, unit.transform.position.y / CONSTANT.ScaleAxisY));
                if (_dis <= range)
                {
                    if (predicate(unit))
                    {
                        _listUnit.Add(unit);
                    }
                }
            }
            return _listUnit;
        }

        // dang sai
        public static List<UnitObject> FindEnemyInArc(int teamIndex, Vector2 position, float range, float arcDegree, bool isFacingLeft)
        {
            List<UnitObject> _listUnit = new List<UnitObject>();
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                UnitObject unit = ListUnitInBattle[i];
                if (unit.TeamIndex != teamIndex)
                {
                    if (unit.IsAlive)
                    {
                        float _dis = Vector2.Distance(new Vector2(position.x, position.y / CONSTANT.ScaleAxisY), new Vector2(unit.transform.position.x, unit.transform.position.y / CONSTANT.ScaleAxisY));
                        if (_dis <= range)
                        {
                            Vector2 relativePos = unit.transform.position.ToVector2() - position;
                            float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                            Debug.Log(unit.Name + " - angle: " + angle);
                            if (isFacingLeft)
                            {
                                if (Mathf.Abs(angle) >= 180 - arcDegree / 2)
                                {
                                    _listUnit.Add(unit);
                                }
                            }
                            else
                            {
                                if (Mathf.Abs(angle) <= arcDegree / 2)
                                {
                                    _listUnit.Add(unit);
                                }
                            }
                        }
                    }
                }
            }
            return _listUnit;
        }

        public static List<UnitObject> FindUnitObjectInRect(System.Predicate<UnitObject> predicate, Vector2 position, Rect rect)
        {
            Rect _rect = new Rect(rect.position + position, rect.size);
            List<UnitObject> _listUnit = new List<UnitObject>();
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                UnitObject unit = ListUnitInBattle[i];
                if (_rect.Contains(unit.transform.position.ToVector2()))
                {
                    _listUnit.Add(unit);
                }
            }
            return _listUnit;
        }

        #endregion
        #region MonoBehavior
        private void Awake()
        {
            Instance = this;
            ListUnitInBattle = new List<UnitObject>();
        }

        public void DestroyManager()
        {
            DestroyObject(this.gameObject);
            Instance = null;
            isPlaying = false;
            isPause = false;
            Time.timeScale = 1;
        }

        // Use this for initialization
        void Start()
        {
            Init();

            StartGame();
            BtnPause.onClick.AddListener(() =>
            {
                isPause = true;
                Time.timeScale = isPause ? 0 : 1;
                PauseMenu.SetActive(isPause);
                TouchController.enabled = !isPause;
            });
            BtnResume.onClick.AddListener(() =>
            {
                isPause = false;
                Time.timeScale = isPause ? 0 : 1;
                PauseMenu.SetActive(isPause);
                TouchController.enabled = !isPause;
            });
            BtnHomeMenu.onClick.AddListener(() =>
            {
                DestroyManager();
                GameManager.Instance.DestroyManager();
                SceneManager.LoadScene(0);
                Time.timeScale = 1;
            });
        }

        void Update()
        {
            if (isPlaying)
            {
                //if (LoseCondition())
                //{
                //    BattleResultMenu.ShowDialog(false);
                //    EndGame();
                //}
                for (int i = 0; i < listPlayer.Count; i++)
                {
                    listPlayer[i].Update(Time.deltaTime);
                }

                _timeInGame += Time.deltaTime;
                _timingPowerUp -= Time.deltaTime;
                _timingHoldingSpawn -= Time.deltaTime;

                //CheckSpawnStage(_timeInGame);
                UpdateStage(Time.deltaTime);

                System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(_timeInGame);
                //TextTime.text = Ulti.TimeSpanToStringRemain(timeSpan);
                if (_timingPowerUp <= 0)
                {
                    _timingPowerUp += CONSTANT.TimePerPowerUp;
                    //SpawnRandomPowerUp();
                }
            }
        }
        #endregion

        #region Func game

        public void PauseGameForCinematic()
        {
            isCinematic = true;
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                if (ListUnitInBattle[i].IsAlive)
                {
                    //ListUnitInBattle[i].unitState = UnitObject.UnitState.Idle;
                    ListUnitInBattle[i].SetStopAnimationWhenCinematic(true);
                }
            }
        }

        public void ResumeGameAfterCinematic()
        {
            isCinematic = false;
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                if (ListUnitInBattle[i].IsAlive)
                {
                    ListUnitInBattle[i].SetStopAnimationWhenCinematic(false);
                }
            }
        }

        public void StartGame()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                BattleStage.LoadBattle(this);
                for (int i = 0; i < userPlayer.ListEquippedUnit.Count; i++)
                {
                    UnitObject obj = SpawnSingleCreepsAtPosition(userPlayer.ListEquippedUnit[i].ToUnitInfo(), userPlayer.PlayerIndex, Vector3.zero);
                    obj.CanTakePowerUp = true;
                    obj.IsPlayerUnit = true;
                    userPlayer.ListPlayUnit.Add(obj);
                }
                LoadUIUnitPlayer(userPlayer);
                BattleStage.SetupPlayerTeamPosition(userPlayer.ListPlayUnit);
            }
        }

        public void ShowEndGame(bool win)
        {
            BattleResultMenu.ShowDialog(win);
            EndGame();
        }

        private void EndGame()
        {
            isPlaying = false;
            for (int i = 0; i < ListUnitInBattle.Count; i++)
            {
                if (ListUnitInBattle[i].IsAlive)
                {
                    ListUnitInBattle[i].SetStopAnimationWhenCinematic(true);
                }
            }
        }

        private void Init()
        {
            // ========================================= Load stage ============================================
            //if (GameManager.Instance != null)
            //{
            //    currentStageScript = new StageScript(GameManager.Instance.SelectedGroupMapIndex, GameManager.Instance.SelectedMapIndex);
            //}
            //else
            //    currentStageScript = new StageScript(1, 1);
            //MobileCamera.BoundaryMax = new Vector2(currentStageScript.CastleDistance / 2 + 1, 5);
            //MobileCamera.BoundaryMin = new Vector2(-currentStageScript.CastleDistance / 2 - 1, -5);
            // =========================================== Player Common =============================================
            ListUnitInBattle = new List<UnitObject>();
            listPlayer = new List<PlayerStats>();

            PlayerStats player1 = new PlayerStats(0, 1, false);
            PlayerStats player2 = new PlayerStats(1, 2, true);
            listPlayer.Add(player1);
            listPlayer.Add(player2);


            player1.ListEnemyPlayer.Add(player2);
            player2.ListEnemyPlayer.Add(player1);

            userPlayer = player1;
            enemyPlayer = player2;

            // ======================================== Player =================================================
            //for (int i = 0; i < GameManager.playerInfo.ListUnitEquiped.Count; i++)
            //{
            //    UnitInfo _unit = GameManager.playerInfo.ListUnitEquiped[i];
            //    player1.EquipUnit(_unit.index, _unit.level, _unit.star);
            //}
            //LoadUIUnitPlayer(player);
            if (GameManager.Instance == null)
            {
                //player1.EquipUnit(1, 0, 1);
                //player1.EquipUnit(2, 0, 1);
                //player1.EquipUnit(3, 0, 1);
                //player1.EquipUnit(4, 0, 1);
                player1.EquipUnit(5, 0, 1); // Samurai
                //player1.EquipUnit(6, 0, 1); // Light Sorcerer
                //player1.EquipUnit(7, 0, 1); // Golem
                player1.EquipUnit(8, 0, 1); // Priest
                //player1.EquipUnit(9, 0, 1); // Scout
                player1.EquipUnit(10, 0, 1); // bard
            }
            else
            {
                for (int i = 0; i < GameManager.playerInfo.ListUnitEquipped.Count; i++)
                {
                    UnitScript _unit = GameManager.playerInfo.ListUnitEquipped[i];
                    player1.EquipUnit(_unit.Index, _unit.Level, _unit.Star);
                }
            }
            // =================================================================================================

            // ================= Stage ===============================
            currentStage = new GameStageArena();
            currentStage.TimeStage = 300;
            currentStage.listWaveInfo.Add(new NewWaveInfo()
            {
                DelayTime = 5,
                TimePerSpawn = new RangeValue(5, 8),
                NumberUnitPerSpawn = new RangeValue(1, 2),
                Unit = new UnitInfo(201, 0, 1),
            });
            currentStage.listWaveInfo.Add(new NewWaveInfo()
            {
                DelayTime = 10,
                TimePerSpawn = new RangeValue(15, 18),
                NumberUnitPerSpawn = new RangeValue(1, 2),
                Unit = new UnitInfo(201, 0, 1),
            });
            currentStage.listWaveInfo.Add(new NewWaveInfo()
            {
                DelayTime = 15,
                TimePerSpawn = new RangeValue(10, 14),
                NumberUnitPerSpawn = new RangeValue(1, 2),
                Unit = new UnitInfo(202, 0, 1),
            });
            StartStage();
        }

        private void LoadUIUnitPlayer(PlayerStats player)
        {
            for (int i = 0; i < listToggleUnit.Count; i++)
            {
                listToggleUnit[i].gameObject.SetActive(false);
            }
            if (player.ListEquippedUnit.IsNullOrEmpty()) return;
            for (int i = 0; i < player.ListEquippedUnit.Count; i++)
            {
                if (i < listToggleUnit.Count)
                {
                    listToggleUnit[i].LoadUnit(player.ListPlayUnit[i]);
                    listToggleUnit[i].gameObject.SetActive(true);
                }
                else
                {
                    //GameObject obj = Instantiate(Resources.Load(CONSTANT.PathToggleUnit)) as GameObject;
                    GameObject obj = Instantiate(listToggleUnit[0].gameObject) as GameObject;
                    obj.transform.SetParent(GroupToggleUnit);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.GetComponent<UIToggleUnitController>().LoadUnit(player.ListPlayUnit[i]);
                    obj.GetComponent<UIToggleUnitController>().Toggle.group = GroupToggleUnit.GetComponent<ToggleGroup>();
                    obj.gameObject.SetActive(true);

                    listToggleUnit.Add(obj.GetComponent<UIToggleUnitController>());
                }
                listToggleUnit[i].Toggle.isOn = false;
                listToggleUnit[i].Toggle.onValueChanged.RemoveAllListeners();
                int _tmp = i;
                listToggleUnit[i].Toggle.onValueChanged.AddListener((x) =>
                {
                    if (x) SelectControlUnit(player.ListPlayUnit[_tmp]);
                });
            }
            listToggleUnit[0].Toggle.isOn = true;
            playerSelectedUnit = player.ListPlayUnit[0];
            LoadUISkillUnit();
        }

        private void LoadUISkillUnit()
        {
            for (int i = 0; i < listBtnSkill.Count; i++)
            {
                listBtnSkill[i].gameObject.SetActive(false);
            }
            if (playerSelectedUnit == null) return;
            for (int i = 0; i < playerSelectedUnit.listSkill.Count; i++)
            {
                if (i < listBtnSkill.Count)
                {
                    listBtnSkill[i].LoadSkill(playerSelectedUnit.listSkill[i], null);
                    listBtnSkill[i].gameObject.SetActive(true);
                }
                else
                {
                    //GameObject obj = Instantiate(Resources.Load(CONSTANT.PathToggleUnit)) as GameObject;
                    GameObject obj = Instantiate(listBtnSkill[0].gameObject) as GameObject;
                    obj.transform.SetParent(GroupBtnSkill);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.GetComponent<UISpellSlotController>().LoadSkill(playerSelectedUnit.listSkill[i], null);
                    obj.gameObject.SetActive(true);

                    listBtnSkill.Add(obj.GetComponent<UISpellSlotController>());
                }
            }
        }

        private void SelectControlUnit(UnitObject unitObj)
        {
            if (playerSelectedUnit != unitObj)
            {
                if (playerSelectedUnit != null)
                    playerSelectedUnit.SetSelected(false);
                playerSelectedUnit = unitObj;
                unitObj.SetSelected(true);
                LoadUISkillUnit();
            }
        }

        public void OnPickItem(RaycastHit hitInfo)
        {
            //Debug.Log("Picked a collider: " + hitInfo.collider);
            //ShowInfoText("" + hitInfo2D.collider, 2);
            RaycastHitOnClick(hitInfo);
        }

        public void OnPickItem2D(RaycastHit2D hitInfo2D)
        {
            //Debug.Log("Picked a collider2D: " + hitInfo2D.collider);
            //selectedUnit = hitInfo2D.collider.GetComponent<UnitObject>();
            //ShowUIOnSelectedUnit();
        }

        private void SpawnOnClick()
        {
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0, Screen.height * 0.1f, 0));
            ////Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 1);
            //int layer = LayerMask.GetMask("SpawnCreep");
            //if (Physics.Raycast(ray, out hit, 1000, layer))
            //{
            //    //Debug.Log(hit.collider.gameObject.name);
            //    UnitScript _unit = null;
            //    for (int i = 0; i < listToggleUnit.Count; i++)
            //    {
            //        if (listToggleUnit[i].Toggle.isOn)
            //        {
            //            if (listToggleUnit[i].unitScript.CurrentCooldown > 0) return;
            //            _unit = listToggleUnit[i].unitScript;
            //        }
            //    }
            //    if (_unit != null)
            //        SpawnMultiCreepsAtPosition(_unit.ToUnitInfo(), player.PlayerIndex, hit.point);
            //}
        }

        private void RaycastHitOnClick(RaycastHit hit)
        {
            playerSelectedUnit.CommandMoveToPosition(hit.point);
            GameObject vfx = CreateVFX(CONSTANT.VfxMoveTarget);
            vfx.transform.position = hit.point;
            vfx.transform.localScale = new Vector3(2, 2, 1);
        }

        private void SpawnRandomPowerUp()
        {
            List<int> _listPowerUpIndex = new List<int>() { 6 };
            int rdm = _listPowerUpIndex.GetRandomValue();
            PowerUpScript power = new PowerUpScript(rdm);
            GameObject obj = Instantiate(Resources.Load(string.Format(CONSTANT.PathPowerUp, power.Code)) as GameObject);
            obj.transform.parent = transform;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<PowerUpObject>().LoadPowerUp(power);
            Vector3 pos = RandomPlayablePosition();
            obj.transform.localPosition = pos;

        }

        private void SpawnRandomCreep()
        {
            float range = 2;
            Vector2 v2 = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
            userPlayer.ListPlayUnit.Add(SpawnSingleCreepsAtPosition(enemyPlayer.ListEquippedUnit[0].ToUnitInfo(), enemyPlayer.PlayerIndex, v2));
        }

        private Dictionary<int, int> dictCountNumberUnit = new Dictionary<int, int>();
        private int GetCountNumber(int index)
        {
            if (dictCountNumberUnit.ContainsKey(index))
                return dictCountNumberUnit[index];
            return 0;
        }
        private void CountNumberSetValue(int index, int number)
        {
            if (dictCountNumberUnit.ContainsKey(index))
            {
                dictCountNumberUnit[index] = number;
            }
            else
            {
                dictCountNumberUnit.Add(index, number);
            }
        }

        public UnitObject SpawnSingleCreepsAtPosition(UnitInfo unitInfo, int playerIndex, Vector3 pos)
        {
            if (!isPlaying) return null;
            UnitScript _unit = new UnitScript(unitInfo);

            GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathPrefabUnit, _unit.Code)) as GameObject;
            if (_prefab == null)
            {
                Debug.LogError("Cant find " + string.Format(CONSTANT.PathPrefabUnit, _unit.Code));
                return null;
            }
            // VFX
            GameObject vfx = CreateVFX(CONSTANT.VfxSpawn);
            vfx.transform.position = pos;
            // Unit
            GameObject obj = SimplePool.Spawn(_prefab, pos, Quaternion.identity);
            obj.transform.parent = transform;
            obj.transform.localScale = new Vector3(_unit.Scale, _unit.Scale);
            obj.SetActive(true);
            obj.GetComponent<UnitObject>().LoadCharacter(_unit, listPlayer[playerIndex], listPlayer[playerIndex].TeamIndex);
            //obj.transform.position = pos;
            //obj.GetComponent<UnitObject>().MovingToPosition(listPlayer[playerIndex].GetNextDefaultTarget());
            ListUnitInBattle.Add(obj.GetComponent<UnitObject>());

            CountNumberSetValue(playerIndex, GetCountNumber(playerIndex) + 1);
            if (listPlayer[playerIndex].TeamIndex == enemyPlayer.TeamIndex)
                BattleStage.CountingEnemySpawn(1);
            obj.GetComponent<UnitObject>().CallBackOnDead.AddListener(() =>
            {
                if (obj.GetComponent<UnitObject>().TeamIndex == enemyPlayer.TeamIndex)
                    BattleStage.CountingEnemyDie(1);
                ListUnitInBattle.Remove(obj.GetComponent<UnitObject>());
                //Debug.Log("OnDead: " + obj.name);
            });


            return obj.GetComponent<UnitObject>();
        }

        public void OnClickResetScene()
        {
            DestroyManager();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion

        #region Battle Logic
        private bool WinCondition()
        {
            return false;
        }

        private bool LoseCondition()
        {
            if (!isPlaying) return false;
            return GetCountNumber(userPlayer.PlayerIndex) <= 0;
        }

        #endregion

        #region Stage
        private GameStageArena currentStage;
        private List<QueueWaveInfo> listQueue = new List<QueueWaveInfo>();
        private void StartStage()
        {
            for (int i = 0; i < currentStage.listWaveInfo.Count; i++)
            {
                QueueWaveInfo queue = new QueueWaveInfo(currentStage.listWaveInfo[i]);
                queue.CallBackSpawn = SpawnNewWaveInfo;
                listQueue.Add(queue);
            }
        }

        private void UpdateStage(float deltaTime)
        {
            for (int i = 0; i < listQueue.Count; i++)
            {
                listQueue[i].Update(deltaTime);
            }
        }
        private void SpawnNewWaveInfo(NewWaveInfo wave)
        {
            int rdm = wave.RandomNumberUnitPerSpawn();
            for (int i = 0; i < rdm; i++)
            {
                SpawnSingleCreepsAtPosition(wave.Unit, enemyPlayer.PlayerIndex, RandomSpawnPosition());
            }
        }

        private Vector2 RandomPlayablePosition()
        {
            return new Vector2(Random.Range(-MapWidth, MapWidth), Random.Range(-MapHeight, MapHeight));
        }

        private Vector2 RandomSpawnPosition()
        {
            //return new Vector2(Random.Range(14f, 15f), Random.Range(-2.5f, 2.5f));
            return BattleStage.RandomEnemySpawnPosition();
        }

        public class QueueWaveInfo
        {
            public delegate void CallBackSpawnWave(NewWaveInfo wave);
            public CallBackSpawnWave CallBackSpawn;
            public float nextSpawnTimer;
            public NewWaveInfo waveInfo;

            public QueueWaveInfo(NewWaveInfo waveInfo)
            {
                this.waveInfo = waveInfo;
                nextSpawnTimer = waveInfo.DelayTime;
            }

            public void Update(float deltaTime)
            {
                nextSpawnTimer -= deltaTime;
                if (nextSpawnTimer <= 0)
                {
                    nextSpawnTimer += waveInfo.RandomTimePerSpawn();
                    CallBackSpawn(waveInfo);
                }
            }
        }
        #endregion


    }

}