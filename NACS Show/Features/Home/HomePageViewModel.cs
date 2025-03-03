namespace NACSShow.Features.Home
{
    public class HomePageViewModel
    {
        public string Title { get; init; }
        //public string HomeHeader { get; init; }
        //public string HomeTextHeading { get; init; }
        //public string HomeText { get; init; }

        public HomePageViewModel(NACSShow.Home home)
        {
            Title = home.Title;
            //HomeHeader = home.HomeHeader;
            //HomeTextHeading = home.HomeTextHeading;
            //HomeText = home.HomeText;
        }
    }
}
