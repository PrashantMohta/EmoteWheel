using HkmpPouch;
using System.IO;
using TMPro;

namespace EmoteWheel
{
    public class EmoteWheel : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
    {
        internal static EmoteWheel Instance;
        internal static PipeClient HkmpPipe;

        internal ObjectPoolManager<GameObject> EmotePool;
        internal ObjectPoolManager<GameObject> TextPool;

        private DateTime lastEmoteTime = DateTime.Now;

        public Dictionary<string, Satchel.Animation> Emotes = new();
        public List<string> EmoteNames = new();
        public List<Satchel.Animation> EmoteAnims = new();

        public static GlobalSettings settings { get; set; } = new GlobalSettings();

        public bool ToggleButtonInsideMenu => false;

        public void OnLoadGlobal(GlobalSettings s) => settings = s;
        public GlobalSettings OnSaveGlobal() => settings;

        private string getVersionSafely(){
            if(typeof(HkmpPouch.PipeClient) != null){
                return Satchel.AssemblyUtils.GetAssemblyVersionHash();
            }
            return "Install HkmpPouch";
        }
        public override string GetVersion()
        {
            var version = "Install Satchel & HkmpPouch";
            try{
                version = getVersionSafely();
            } catch(Exception e){
                Modding.Logger.Log(e.ToString());
            }
            return version;
        }
        public EmoteWheel()
        {
            LoadEmotes();
        }
        public GameObject EmoteGenerator()
        {
            var emote = new GameObject("Emote", typeof(SpriteRenderer),typeof(CustomAnimationController),typeof(EmoteBehaviour));
            UnityEngine.Object.DontDestroyOnLoad(emote);
            var rend = emote.GetComponent<SpriteRenderer>();
            rend.sortingOrder = 5;

            EmoteBehaviour es = emote.GetComponent<EmoteBehaviour>();
            es.isSprite = false;

            emote.SetActive(false);
            return emote;
        }

        public GameObject TextGenerator()
        {
            var emote = new GameObject("TextEmote", typeof(TextMeshPro));
            UnityEngine.Object.DontDestroyOnLoad(emote);
            emote.SetActive(false);

            var textMeshObject = emote.GetAddComponent<TextMeshPro>();
            textMeshObject.text = ":EMOTE:";
            textMeshObject.alignment = TextAlignmentOptions.Center;
            textMeshObject.fontSize = 10;
            textMeshObject.outlineWidth = 0.2f;
            textMeshObject.outlineColor = Color.black;

            EmoteBehaviour es = emote.GetAddComponent<EmoteBehaviour>();
            es.isSprite = false;

            return emote;
        }

        public void showEmote(string name, GameObject playerObject)
        {
            if(Emotes.TryGetValue(name, out var emoteSprite))
            { 
                GameObject go = EmotePool.PickFromPool();
                CustomAnimationController anim = go.GetComponent<CustomAnimationController>();
                EmoteBehaviour es = go.GetComponent<EmoteBehaviour>();
                es.pool = EmotePool;
                es.setPlayer(playerObject);
                anim.Init(emoteSprite);
                go.SetActive(true);
            } else {
                GameObject go = TextPool.PickFromPool();
                go.GetAddComponent<TextMeshPro>().text = $":{name.Replace(".json","")}:";
                EmoteBehaviour es = go.GetComponent<EmoteBehaviour>();
                es.pool = TextPool;
                es.setPlayer(playerObject);
                go.SetActive(true);
            }
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            
            EmotePool = new ObjectPoolManager<GameObject>(20, EmoteGenerator);
            TextPool = new ObjectPoolManager<GameObject>(20, TextGenerator);
            ModHooks.HeroUpdateHook += WheelUI.listenForInput;
            Instance = this;
            HkmpPipe = new PipeClient("EmoteWheel");
            HkmpPipe.OnRecieve += (_, R) =>
                {
                    var p = R.Data;
                    LogDebug($"{p.FromPlayer}:{p.EventData}");
                    var player = HkmpPipe.ClientApi.ClientManager.GetPlayer(p.FromPlayer);
                    if (player != null) {
                        showEmote(p.EventData, player.PlayerContainer);
                    }
                };
            }

        public void LoadEmotes()
        {
            var currentDir = AssemblyUtils.getCurrentDirectory();
            var emoteDir = Path.Combine(currentDir, "Emotes");
            IoUtils.EnsureDirectory(emoteDir);
            var jsons = Directory.EnumerateFiles(emoteDir, "*.json", SearchOption.AllDirectories);
            foreach (var json in jsons)
            {
                var fileInfo = new FileInfo(json);
                LogDebug(json+"|"+ fileInfo.Name);
                Emotes[new FileInfo(json).Name] = CustomAnimation.LoadAnimation(json);
            }
            var anim = new Satchel.Animation()
            {
                frames = new string[1] { "x" },
                fps = 1,
                loop = false
            };
            EmoteNames = new() { "Cancel"};
            EmoteAnims = new() { anim };
            foreach (var kvp in Emotes)
            {
                EmoteNames.Add(kvp.Key);
                EmoteAnims.Add(kvp.Value);
            }
            Emotes["Cancel"] = CustomAnimation.LoadAnimation(
               anim,
               new Sprite[1] {
                    AssemblyUtils.GetSpriteFromResources("cancel.png")
                }
            );
            WheelUI.setChoices(EmoteAnims);
        }

        public void ChoiceMade(int Index)
        {
            if (lastEmoteTime != null)
            {
                var ms = (DateTime.Now - lastEmoteTime).TotalMilliseconds;
                if (ms < 500)
                {
                    //Dont spam emotes
                    return;
                }
            }
            var name = EmoteNames[Index];
            // broadcast emote to everyone
            HkmpPipe.Broadcast("", name, true, false);
            // show emote to current user
            showEmote(name, HeroController.instance.gameObject);
            lastEmoteTime = DateTime.Now;   
        }

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return BetterMenu.GetMenu(modListMenu);
        }
    }
}