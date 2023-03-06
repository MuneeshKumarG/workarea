
using System.Collections.ObjectModel;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public class CartesianBubbleViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> GDPGrowthCollection { get; set; }
        public ObservableCollection<ChartDataModel> ActionData { get; set; }
        public ObservableCollection<ChartDataModel> HorrorData { get; set; }
        public ObservableCollection<ChartDataModel> ScienceFictionData { get; set; }
        public ObservableCollection<ChartDataModel> ThrillerData { get; set; }

        public CartesianBubbleViewModel()
        {

            GDPGrowthCollection = new ObservableCollection<ChartDataModel>()
        {
         new ChartDataModel("China",92.2,7.8,1.347),
         new ChartDataModel("India",74,6.5,1.241),
         new ChartDataModel( "Indonesia", 90.4, 6.0, 0.238),
         new ChartDataModel( "US", 99.4, 2.2, 0.312),
         new ChartDataModel( "Germany", 99, 0.7, 0.0818),
         new ChartDataModel( "Egypt", 72, 2.0, 0.0826),
         new ChartDataModel( "Russia", 99.6, 3.4, 0.143),
         new ChartDataModel( "Mexico", 86.1, 4.0, 0.115),
         new ChartDataModel( "Philippines", 92.6, 6.6, 0.096),
         new ChartDataModel( "Nigeria", 61.3, 1.45, 0.162),
         new ChartDataModel( "Hong Kong", 82.2, 3.97, 0.7),
         new ChartDataModel( "Netherland", 79.2, 3.9, 0.162),
         new  ChartDataModel( "Jordan", 72.5, 4.5, 0.7),
         new ChartDataModel( "Australia", 81, 3.5, 0.21),
         new ChartDataModel( "Mongolia", 66.8, 3.9, 0.028),
         new  ChartDataModel( "Taiwan", 78.4, 2.9, 0.231)
        };

            ActionData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Transformers I",150,836,369,6),
                new ChartDataModel("Skyfall",200,1109,599,7.7),
                new ChartDataModel("The Avengers",220,1520,1205,8),
                new ChartDataModel("Spider-Man 3",258,891,471,6.2),
                new ChartDataModel("Ninja Turtles",250,1085,80,6.8),
                new ChartDataModel("Transformers II",195,1124,371,6.2),
                new ChartDataModel("The Dark Knight Rises",250,1215,1418,8.4),
                new ChartDataModel("The Dark Knight",185,1005,2127,9),
                new ChartDataModel("Inception",16,826,1888,8.8),

        };

            HorrorData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Interview with the Vampire",60,224,83, 6.9),
                new ChartDataModel("Scream",14,173,268,7.2),
                new ChartDataModel("I Know What You Did Last Summer", 17,176, 126,5.7),
                new ChartDataModel("The Ring", 48,249, 303,7.1),
                new ChartDataModel("Van Helsing", 160,300,233,6.1),               
                new ChartDataModel("Scream 2", 24,172, 6.2,6.2),
                new ChartDataModel("The Blair Witch Project", 60,248,220,6.5),
                new ChartDataModel("The Conjuring", 13,318,410,7.5),
                new ChartDataModel("Flatliners",26,237,76,6.6),

        };

            ScienceFictionData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Armageddon",140,554,377,6.7),
                new ChartDataModel("Star Wars: Episode I",115,924,667,6.5),
                new ChartDataModel("Star Wars: Episode II",120,649,587,6.6),
                new ChartDataModel("The Matrix Reloaded", 150,739,487,7.2),
                new ChartDataModel("Star Wars: Episode III", 113,850,654,7.5),
                new ChartDataModel("War of the Worlds", 132,592,394,6.5),
                new ChartDataModel("World War Z", 200,532, 7,7),
                new ChartDataModel("Planet of the Apes", 170,711,395,7.6),
        };

            ThrillerData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Men in Black",90,98,487,7.3),
                new ChartDataModel("Godzilla",130,379,175,6.4),
                new ChartDataModel("The Sixth Sense",40,673,860,8.1),
                new ChartDataModel("Ocean's Eleven",85,451,50,7.8),
                new ChartDataModel("Terminator 3",200,435,357,6.3),
                new ChartDataModel("Casino Royale",150,599,547,8),
                new ChartDataModel("Live Free or Die Hard",110,384,375,7.1),
                new ChartDataModel("Clash of the Titans",125,493,260,5.8),
                new ChartDataModel("Mission: Impossible",145,695,435,7.4),
                new ChartDataModel("Godzilla",160,529,359,6.4),
        };
        }

    }
}
