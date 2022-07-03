using Satchel.BetterMenus;
namespace EmoteWheel
{
    public class KeyBinds : PlayerActionSet
    {
        public PlayerAction Next;
        public PlayerAction Prev;
        public PlayerAction Send;
        public PlayerAction Cancel;
        public KeyBinds()
        {
            Next = CreatePlayerAction("Next");
            Prev = CreatePlayerAction("Prev");
            Send = CreatePlayerAction("Send");
            Cancel = CreatePlayerAction("Cancel");
            DefaultBinds();
        }

        private void DefaultBinds()
        {
            Next.AddDefaultBinding(Key.Key0);
            Prev.AddDefaultBinding(Key.Key9);
            Send.AddDefaultBinding(Key.Minus);
            Cancel.AddDefaultBinding(Key.Equals);
        }
    }

    public class ButtonBinds : PlayerActionSet
    {
        public PlayerAction Next;
        public PlayerAction Prev;
        public PlayerAction Send;
        public PlayerAction Cancel;
        public ButtonBinds()
        {
            Next = CreatePlayerAction("Next");
            Prev = CreatePlayerAction("Prev");
            Send = CreatePlayerAction("Send");
            Cancel = CreatePlayerAction("Cancel");

            DefaultBinds();
        }

        private void DefaultBinds()
        {
            Next.AddDefaultBinding(InputControlType.RightStickRight);
            Prev.AddDefaultBinding(InputControlType.RightStickLeft);
            Send.AddDefaultBinding(InputControlType.RightStickUp);
            Cancel.AddDefaultBinding(InputControlType.RightStickDown);
        }
    }
    public static class BetterMenu
    {
        public static Menu MenuRef;
        public static Menu PrepareMenu()
        {
            return new Menu("Emotes Mod", new Element[]{
                Blueprints.KeyAndButtonBind("Next Emote",EmoteWheel.settings.Keybinds.Next,EmoteWheel.settings.ButtonBinds.Next),
                Blueprints.KeyAndButtonBind("Prev Emote",EmoteWheel.settings.Keybinds.Prev,EmoteWheel.settings.ButtonBinds.Prev),
                Blueprints.KeyAndButtonBind("Send Emote",EmoteWheel.settings.Keybinds.Send,EmoteWheel.settings.ButtonBinds.Send),
                Blueprints.KeyAndButtonBind("Cancel Emote",EmoteWheel.settings.Keybinds.Cancel,EmoteWheel.settings.ButtonBinds.Cancel)
            });
        }
        public static MenuScreen GetMenu(MenuScreen lastMenu)
        {
            if (MenuRef == null)
            {
                MenuRef = PrepareMenu();
            }
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}
