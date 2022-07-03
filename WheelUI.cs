namespace EmoteWheel
{
    internal static class WheelUI
    {
        public static int Index = 0;

        public static GameObject wheel;
        public static GameObject wheel1;
        public static GameObject wheel2;
        public static List<Satchel.Animation> optionSprites;

        public static void ChoiceMade()
        {
            EmoteWheel.Instance.ChoiceMade(Index);
        }

        public static void setChoices(List<Satchel.Animation> options)
        {
            optionSprites = options;
        }
        public static void CreateWheel()
        {
            if (wheel != null) { return; }
            wheel = new GameObject("Emote Wheel");
            var wheelRenderer = wheel.AddComponent<SpriteRenderer>();
            Color color = wheelRenderer.material.color;
            color.a = 0.5f;
            wheelRenderer.material.color = color;
            wheelRenderer.sortingOrder = 0;

            wheel1 = new GameObject("Emote Wheel");
            wheelRenderer = wheel1.AddComponent<SpriteRenderer>();
            color = wheelRenderer.material.color;
            color.a = 0.5f;
            wheelRenderer.material.color = color;
            wheelRenderer.sortingOrder = 0;

            wheel2 = new GameObject("Emote Wheel");
            wheelRenderer = wheel2.AddComponent<SpriteRenderer>();
            color = wheelRenderer.material.color;
            color.a = 0.5f;
            wheelRenderer.material.color = color;
            wheelRenderer.sortingOrder = 0;
        }

        public static void currentChoice(int index)
        {
            CreateWheel();
            wheel.transform.position = HeroController.instance.gameObject.transform.position + new Vector3(-1f, 2f, 0);
            wheel1.transform.position = HeroController.instance.gameObject.transform.position + new Vector3(0, 2f, 0);
            wheel2.transform.position = HeroController.instance.gameObject.transform.position + new Vector3(+1f, 2f, 0);

            CustomAnimationController anim;
            if (index - 1 >= 0)
            {
                anim = wheel.GetAddComponent<CustomAnimationController>();
                if (anim.anim != optionSprites[index - 1])
                {
                    anim.Init(optionSprites[index - 1]);
                }
                wheel.SetActive(true);
            } else {
                anim = wheel.GetAddComponent<CustomAnimationController>();
                if (anim.anim != optionSprites[optionSprites.Count - 1])
                {
                    anim.Init(optionSprites[optionSprites.Count - 1]);
                }
                wheel.SetActive(true);
            }

            anim = wheel1.GetAddComponent<CustomAnimationController>();
            if (anim.anim != optionSprites[index])
            {
                anim.Init(optionSprites[index]);
            }
            wheel1.SetActive(true);


            if (index + 1 < optionSprites.Count) { 
                anim = wheel2.GetAddComponent<CustomAnimationController>();
                if (anim.anim != optionSprites[index + 1])
                {
                    anim.Init(optionSprites[index + 1]);
                }
                wheel2.SetActive(true);
            } else {
                anim = wheel2.GetAddComponent<CustomAnimationController>();
                if (anim.anim != optionSprites[0])
                {
                    anim.Init(optionSprites[0]);
                }
                wheel2.SetActive(true);
            }
        }

        public static bool choosing = false;
        public static DateTime lastInput = DateTime.MinValue;
        public static void listenForInput()
        {
            InputManager.ActiveDevice.RightStick.LowerDeadZone = 0.2f;

            if ((DateTime.Now - lastInput).TotalMilliseconds >  200)
            {
                if (EmoteWheel.settings.Keybinds.Send.IsPressed || EmoteWheel.settings.ButtonBinds.Send.IsPressed)
                {
                    ChoiceMade();
                    choosing = false;
                    lastInput = DateTime.Now;
                } else if (EmoteWheel.settings.Keybinds.Cancel.IsPressed || EmoteWheel.settings.ButtonBinds.Cancel.IsPressed)
                {
                    Index = 0;
                    choosing = false;
                    lastInput = DateTime.Now;
                } else if (EmoteWheel.settings.Keybinds.Next.IsPressed || EmoteWheel.settings.ButtonBinds.Next.IsPressed)
                {
                    Index += 1;
                    choosing = true;
                    lastInput = DateTime.Now;
                } else if (EmoteWheel.settings.Keybinds.Prev.IsPressed || EmoteWheel.settings.ButtonBinds.Prev.IsPressed)
                {
                    Index -= 1;
                    choosing = true;
                    lastInput = DateTime.Now;
                }

            }

            if (Index >= optionSprites.Count) {
                Index = 0;
            } 
            if (Index < 0)
            {
                Index = optionSprites.Count - 1;
            }
            if (choosing)
            {
                currentChoice(Index);
            } else {
                if(wheel != null || wheel1 != null || wheel2 != null){
                    wheel?.SetActive(false);
                    wheel1?.SetActive(false);
                    wheel2?.SetActive(false);
                }
            }

        }
    }
}
