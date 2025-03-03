namespace Convenience.org.Models
{
    public class ButtonsViewModel
    {
        public string ButtonText { get; set; }
        public string ButtonTextColor { get; set; }
        public string ButtonURL { get; set; }
        public ButtonTypeEnum ButtonType { get; set; }
        public string LeftIconType { get; set; }
        public string LeftIconName { get; set; }
        public string LeftIconColor { get; set; }
        public string RightIconType { get; set; }
        public string RightIconName { get; set; }
        public string RightIconColor { get; set; }
        public string ButtonBGColor { get; set; }
    }
    public enum ButtonTypeEnum
    {
        TextLinkButton,
        BackToLog,
        GreyButton,
        InvertedButton,
        SideRoundButton,
        DoubleIconNoBGButton
    }
}
