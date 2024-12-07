using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.RemoteConfig;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Menus
{
    public class Launcher : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        #region Stuff
        /*------------------------/StartOfVariables/--------------------------*/

        public static Launcher instance;
        
        #region Constant Strings
        const string NETWORK_PLAYER = "NetworkPlayer", GAME_SCENE = "Intializeing Game", ROOM_LIST = "Scroll View/Viewport/Content",
        NAME = "Name", MODE = "mode",TYPE = "type", MAIN = "Main", MAIN_MENU = "MainMenu", FIGHT = "Fight", TO_MAIN = "ToMainMenu",
        REMOTE_VERSION = "minVersion", REMOTE_TEST_VERSION = "TestVersion", REMOTE_MAX_CCU = "maxCCU";
        #endregion
        
        #region Game Version/CCU
        [SerializeField] string gameVersion = "0.0.100";
        int _maxCCU = 100;
        #endregion

        #region InputField/Profile
        [SerializeField] TMP_InputField _usernameField, _roomNameField;
        public static ProfileData myProfile = new ProfileData(); // '1'
        #endregion

        #region Menu Tabs
        [SerializeField] GameObject _mainTab, _roomsTab, _createTab, _matchmakingTab;
        [SerializeField] GameObject reconnectButton;
        #endregion

        #region Rooms
        //Room Button Prefab
        [SerializeField] GameObject _roomButton;
        [SerializeField] TMP_Text _currentPlayersCount, _connectionState,gameVersionText;
        [SerializeField] TMP_InputField roomnameField;
        private List<RoomInfo> listOfRooms; //'2'
        int playersIndex;
        #endregion

        #region Scene Management
        [SerializeField] int multiplayerScene = 1;
        public int currentScene;
        #endregion

        #region Texts/normal Notifications
        [SerializeField] TMP_Text notificationText, _currentGamemodeName, _currentGameModeDescription;
        [SerializeField] GameObject notification;
        #endregion

        #region Gamemodes
        public delegate void OnPlayButtonWasPressed(int gameModeIndex);
        public OnPlayButtonWasPressed onPlayButtonWasPressed;

        [SerializeField] GameModeHolder gameModeHolderPrefab;
        public GameMode[] gameModes;
        int currentGameMode;
        int currentGameModeType;
        [SerializeField] GameObject[] GameModesSprites;
        #endregion

        #region Controls
        [SerializeField] GameObject movementAnalogCheckmark, movementArrowCheckmark, shootAnalogCheckmark, shootButtonsCheckmark;
        [SerializeField] Animator _transitionAnimator;
        #endregion

        #region RemoteConfig
        public struct UserAttributes { }
        public struct AppAttributes { }
        #endregion

        #region Global Notifications
        [SerializeField] GameObject globalVersionNotification;
        [SerializeField] TMP_Text globalVersionText;
        #endregion

        #region booleans
        bool isConnected;
        #endregion

        /*------------------------/EndOfVariables/--------------------------*/
        #endregion

        #region Initializing
        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(instance.gameObject);

            instance = this;
            DontDestroyOnLoad(gameObject);

            onPlayButtonWasPressed += JoinByMatchmaking;

            myProfile = Data.LoadProfile();

            PhotonNetwork.AutomaticallySyncScene = true;
            isConnected = false;

            //Remote configuration
            ConfigManager.FetchCompleted += VerifyConnection;
            ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());

            Invoke(nameof(OnConnectionFailedCheck), 5f);
        }
        void Start()
        {
            roomnameField.text = "";
            gameVersionText.text = $"Game version: {gameVersion}";
            currentGameMode = 0;
            InvokeRepeating(nameof(UpdatePlayerCount), 0f, 3f);

            LoadProfileData();
        }
        #endregion

        #region Tabs
        public void TabCloseAll()
        {
            _mainTab.SetActive(false);
            _roomsTab.SetActive(false);
            _createTab.SetActive(false);
            _matchmakingTab.SetActive(false);

            if (notification.activeInHierarchy)
                notification.gameObject.SetActive(false);
        }

        public void TabOpenMain()
        {
            TabCloseAll();
            _mainTab.SetActive(true);
        }

        public void TabOpenRooms()
        {
            TabCloseAll();
            _roomsTab.SetActive(true);
            ClearRoomList();

            Transform Content = _roomsTab.transform.Find("Scroll View/Viewport/Content");

            foreach (RoomInfo a in listOfRooms)
            {
                GameObject roomButton = Instantiate(_roomButton, Content);

                roomButton.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.Name;
                roomButton.transform.Find("Players").GetComponent<TMP_Text>().text = $"{a.PlayerCount} / {a.MaxPlayers}";
                //list

                roomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(roomButton.transform); });
            }
        }

        public void TabOpenCreate()
        {
            TabCloseAll();
            _createTab.SetActive(true);

            UpdateMode();
        }

        public void Reconnect()
        {
            RefetchRemoteConfigOnVersionFetchingFail();
            reconnectButton.SetActive(false);
        }

        public void UpdateMode()
        {
            _currentGamemodeName.text = $"GameMode: {gameModes[currentGameMode].MyType.TypeName}";
            _currentGameModeDescription.text = gameModes[currentGameMode].MyDescription;

            for (int i = 0; i < GameModesSprites.Length; i++)
                GameModesSprites[i].SetActive(i == currentGameMode);
        }

        public void TabOpenMatchmaking()
        {
            TabCloseAll();
            _matchmakingTab.SetActive(true);

            Transform buttonParent = _matchmakingTab.transform.Find("Scroll View/Viewport/Content");

            foreach (Transform a in buttonParent)
                Destroy(a.gameObject);

            for (int i = 0; i < gameModes.Length - 1; i++)
                if (gameModes[i].MyName != "Test")
                    Instantiate(gameModeHolderPrefab, buttonParent).Setup(gameModes[i], i);
        }

        #endregion

        #region Connection
        void VerifyConnection(ConfigResponse response)
        {
            string testVersion = ConfigManager.appConfig.GetString(REMOTE_TEST_VERSION);
            string minVersion = ConfigManager.appConfig.GetString(REMOTE_VERSION);
            _maxCCU = ConfigManager.appConfig.GetInt(REMOTE_MAX_CCU);

            if (string.IsNullOrEmpty(minVersion))
                RefetchRemoteConfigOnVersionFetchingFail();

            if (gameVersion == minVersion || gameVersion == testVersion)
            {
                if (!PhotonNetwork.IsConnected)
                    StartCoroutine(Connect());
            }
            else
            {
                globalVersionText.text = $"Current version {{{gameVersion}}}, latest version: {{{minVersion}}}.\nIf the latest version is empty, contact us";
                globalVersionNotification.SetActive(true);
            }
        }
        void RefetchRemoteConfigOnVersionFetchingFail() 
        {
            ConfigManager.FetchCompleted += VerifyConnection;
            ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());
        }
        public IEnumerator Connect()
        {
            Debug.Log("Trying To Connect...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;

            if (PhotonNetwork.IsConnected)
                yield return null;
            else
            {
                _connectionState.text = "Maximum player has been reached, try again later";
                _connectionState.fontSize = 20;
            }
        }

        void OnConnectionFailedCheck()
        {
            if (!PhotonNetwork.IsConnected)
                reconnectButton.SetActive(true);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            if (_connectionState)
                _connectionState.text = "Connected to master server";
            isConnected = true;

            if (PhotonNetwork.NetworkClientState != ClientState.JoiningLobby)
                PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("Joined Lobby Successfully");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            if (PhotonNetwork.IsMasterClient)
                StartGame();
        }
        public override void OnJoinRoomFailed(short returnCode, string message) => PopNotification("A room with this name was not found, recheck the name\nor check your connection");
        public override void OnJoinRandomFailed(short returnCode, string message) => base.OnJoinRandomFailed(returnCode, message);
        #endregion

        #region Create
        public void Create()
        {
            RoomOptions options = new RoomOptions
            {
                MaxPlayers = (byte)gameModes[currentGameMode].MyType.MaxPlayers,
                IsVisible = true,
            };

            options.CustomRoomPropertiesForLobby = new string[] { MODE,TYPE };

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { MODE, currentGameMode },
                {TYPE,currentGameModeType }
            };
            options.CustomRoomProperties = properties;

            if (!string.IsNullOrEmpty(roomnameField.text))
                CreateRoom(options);
            else
                PopNotification("Enter a name for your room");
        }
        public void CreateOnRandomMatchmaking(string roomname)
        {
            if (!PhotonNetwork.IsConnected)
            {
                PopNotification("Wait for server to connect.If failed check your connection or game version.");
                return;
            }
            RoomOptions options = new RoomOptions
            {
                MaxPlayers = (byte)gameModes[currentGameMode].MyType.MaxPlayers,
                IsVisible = true,
            };

            options.CustomRoomPropertiesForLobby = new string[] { MODE ,TYPE};

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { MODE, currentGameMode },
                { TYPE,currentGameModeType }
            };
            options.CustomRoomProperties = properties;

            PlayAnimation(MAIN_MENU);
            PhotonNetwork.CreateRoom(roomname, options);
        }
        public void ChangeMode(int value)
        {
            currentGameMode += value;
            currentGameMode.Portal(0, gameModes.Length - 1);
            UpdateMode();
        }
        public void ChangeGameModeType() 
        {
            int max = gameModes[currentGameMode].MyTypes.Length;
            currentGameModeType++;
            currentGameModeType.Portal(0, max);

        }
        #endregion
        
        #region Joining
        public void JoinPrivateRoom()
        {
            RoomOptions roomOptions = new RoomOptions
            {
                IsVisible = false
            };

            if (!string.IsNullOrEmpty(_roomNameField.text))
            {

                foreach (RoomInfo info in listOfRooms)
                {
                    if (info.Name == _roomNameField.text)
                    {
                        JoinMatch(info);
                        PopNotification("Trying To Join...", 100f);
                        return;

                    }
                    else
                        PopNotification("No room found with this Name. Try to make one or try again later");
                }
            }
            else
                PopNotification("Enter room's name To join");
        }
        public void JoinByMatchmaking(int gameModeIndex)
        {
            if (!isConnected)
            {
                PopNotification("Connecting... try again after couple of seconds");
                return;
            }

            bool foundRoom = false;

            foreach (RoomInfo info in listOfRooms)
                if ((int)info.CustomProperties[MODE] == gameModeIndex)
                {
                    foundRoom = true;
                    JoinMatch(info);
                }

            if (!foundRoom)
            {
                currentGameMode = gameModeIndex;
                CreateOnRandomMatchmaking($"{myProfile.userName + Random.Range(0,1000)}");
            }
        }
        public void JoinRoom(Transform _button)
        {
            string _roomName = _button.transform.Find(NAME).GetComponent<TextMeshProUGUI>().text;

            VerifyUsername();

            Debug.LogWarning("1");
            RoomInfo roomInfo = null;
            Transform buttonParent = _button.parent;
            for (int i = 0; i < buttonParent.childCount; i++)
                if (buttonParent.GetChild(i).Equals(_button))
                {
                    roomInfo = listOfRooms[i];
                    break;
                }

            if (roomInfo != null)
                JoinMatch(roomInfo);
        }
        void LoadGameSettings(RoomInfo roomInfo)
        {
            currentGameMode = (int)roomInfo.CustomProperties[MODE];
            currentGameModeType = (int)roomInfo.CustomProperties[TYPE];
        }
        void VerifyUsername()
        {
            if (_usernameField != null)
                myProfile.userName = string.IsNullOrEmpty(_usernameField.text) ? $"Player_{playersIndex}" : _usernameField.text;
        }
        #endregion

        #region Room Related
        void ClearRoomList()
        {
            if (_roomsTab != null)
            {
                Transform content = _roomsTab.transform.Find(ROOM_LIST);
                foreach (Transform transform in content)
                    Destroy(transform.gameObject);
            }
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList) => listOfRooms = roomList;
        #endregion

        #region Notification
        void PopNotification(string message, float delay = 1.5f)
        {
            notification.SetActive(true);
            notificationText.text = message;
            Invoke(nameof(TurnMeOff), delay);
        }
        void UpdatePlayerCount()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            if (_currentPlayersCount != null)
                _currentPlayersCount.text = $"Players online: {PhotonNetwork.CountOfPlayers}/{_maxCCU}";
        }
        void TurnMeOff()
        {
            if (notification != null)
                notification.SetActive(false);
        }
        #endregion

        #region On Join Events
        public void StartGame()
        {
            VerifyUsername();
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Saving();
                SceneManager.LoadScene(1);
                PhotonNetwork.LoadLevel(multiplayerScene);
            }
        }
        void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene.buildIndex;
            if (currentScene == multiplayerScene)
                CreatePlayer();
        }
        void CreatePlayer()
        {
            PhotonNetwork.Instantiate(NETWORK_PLAYER, transform.position, Quaternion.identity, 0);
            playersIndex++;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }
        #endregion

        #region Profile
        public void ChangeControlsLs(int ls)
        {
            myProfile.leftControls = (LSControls)ls;
            Saving();
        }
        public void ChangeControlsRs(int rs)
        {
            myProfile.rightControls = (RSControls)rs;
            Saving();
        }
        void Saving()
        {
            if (_usernameField != null)
                myProfile.userName = _usernameField.text;
            Data.SaveProfile(myProfile);
        }
        void LoadProfileData()
        {
            _usernameField.text = myProfile.userName;

            bool isZero1 = myProfile.leftControls == LSControls.Joystick;
            movementAnalogCheckmark.SetActive(isZero1);
            movementArrowCheckmark.SetActive(!isZero1);

            bool isZero2 = myProfile.rightControls == RSControls.ThreeJoysticks;
            shootButtonsCheckmark.SetActive(isZero2);
            shootAnalogCheckmark.SetActive(!isZero2);
        }
        #endregion

        #region Final Logic
        void JoinMatch(RoomInfo info)
        {
            if (!PhotonNetwork.IsConnected)
            {
                PopNotification("Wait for server to connect.\nIf failed check your connection or game version.");
                return;
            }
            PlayAnimation(MAIN_MENU);
            LoadGameSettings(info);
            PhotonNetwork.JoinRoom(info.Name);
        }
        void CreateRoom(RoomOptions options)
        {
            if (!PhotonNetwork.IsConnected)
            {
                PopNotification("Wait for server to connect.\nIf failed check your connection or game version.");
                return;
            }
            PlayAnimation(MAIN_MENU);
            PhotonNetwork.CreateRoom(roomnameField.text, options);
        }

        void PlayAnimation(string sm)
        {
            _transitionAnimator.enabled = true;
            _transitionAnimator.SetTrigger(sm);
        }
        #endregion
    }
}