namespace EmoteWheel
{
    internal class EmoteBehaviour : MonoBehaviour
    {
        private float scale = 1.0f;
        private float opacity = 0.5f;
        private bool hiding = false;
        private SpriteRenderer renderer;
        private GameObject playerObject;
        private DateTime emitTime;
        public bool isSprite = true;

        public ObjectPoolManager<GameObject> pool;

        public void setPlayer(GameObject po)
        {
            playerObject = po;
        }
        void Reset()
        {
            scale = 1.0f;
            opacity = 0.5f;
            hiding = false;
            playerObject = null;
        }

        void OnDisable()
        {
            Reset();
            pool?.RecycleToPool(this.gameObject);
        }

        void OnEnable()
        {
            if (playerObject == null) { return; }
            transform.position = playerObject.transform.position + new Vector3(0, 4f, 0);
            transform.localScale = new Vector2(1f, 1f);
            if(isSprite){
                renderer = gameObject.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 5;
            }
            emitTime = DateTime.Now;
        }

        void Start()
        {
            if (playerObject == null) { return; }
            transform.position = playerObject.transform.position + new Vector3(0, 4f, 0);
            transform.localScale = new Vector2(1f, 1f);
            if(isSprite){
                renderer = gameObject.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 5;
            }
            emitTime = DateTime.Now;
        }
        void Update()
        {
            
            if ((DateTime.Now - emitTime).TotalMilliseconds > 1000)
            {
                transform.Translate(0, Time.deltaTime * 3f, 0);
                if (hiding)
                {
                    if(isSprite){
                        renderer.sortingOrder = 3;
                    }
                    opacity -= 3f * Time.deltaTime;
                    scale += 3f * Time.deltaTime;
                }
            }
            else
            {
                if ((DateTime.Now - emitTime).TotalMilliseconds > 300 && isSprite)
                {
                    renderer.sortingOrder = 4;
                }
                transform.position = playerObject.transform.position + new Vector3(0, 2f, 0);
            }

            transform.localScale = new Vector3(scale, scale, 1f);
            if(isSprite){
                Color color = renderer.material.color;
                color.a = opacity;
                renderer.material.color = color;
            }

            if (scale < 2.0f)
            {
                scale += 3f * Time.deltaTime;
            }


            if (!hiding)
            {
                opacity += 3f * Time.deltaTime;
                if (scale > 2.0f)
                {
                    scale = 2.0f;
                }
            }

            if (opacity >= 1f)
            {
                hiding = true;
                opacity = 1f;
            }
            if (opacity <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
