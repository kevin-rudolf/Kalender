//using Android.App;
using System.Diagnostics.Metrics;

namespace Kalender;

public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();

        CreateGrid();

        FillBasicData();
    }

    private void FillBasicData()
    {
        ShellContent1.Title = basicdate.Month + " " + basicdate.Year.ToString();
    }

    private DateTime basicdate = new DateTime();

    private void CreateGrid() //AND additional line (including arrows)
    {
        //double CPHeight = Frame.Height;

        Grid grid = new Grid
        {
            //HeightRequest = CPHeight,
            Margin = new Thickness(40, 20, 40, 0),
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(6, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(6, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(6, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(6, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(6, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(6, GridUnitType.Star) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) }
            }
        };

        basicdate = DateTime.Now;
        DateTime realdate = new DateTime();
        var firstday = new DateTime(basicdate.Year, basicdate.Month, 1);
        var lastDayOfMonth = firstday.AddMonths(1).AddDays(-1);
        //Wie viele Felder vom Vormonat müssen ausgeblendet werden bis heute ist
        int grayfields = (((int)firstday.DayOfWeek) - 1) * -1;

        //1. Tag vom Vormonat
        var fDoLM = new DateTime(basicdate.Year, basicdate.Month - 1, 1);
        //Letzter Tag vom Vormonat
        var lDoLM = fDoLM.AddMonths(1).AddDays(-1);

        //letzter Tag vom Vormonat - die auszublendenten Tage vom Vormonat = Datum wo Montag sein müsste
        realdate = lDoLM.AddDays(grayfields+1);

        int dayscounter = 0;
        int dayscounter_nextmonth = 1;

        if (((int)firstday.DayOfWeek) != 1)
        {
            //fülle so lange bis Vormonat letzten erreicht ist
            for (var k = 0; k < grayfields * -1; k++)
            {
                grid.Add(new Frame
                {
                    CornerRadius = 25,
                    BackgroundColor = Colors.LightGray
                }, k, 0);
                grid.Add(new Label
                {
                    Text = realdate.AddDays(dayscounter).Day.ToString()+"."+realdate.AddDays(dayscounter).Month.ToString()+".",
                    TextColor = Color.Parse("LightGray"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }, k, 0);
                dayscounter++;
            }
            dayscounter = 0;
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (i == 0 && j < grayfields * -1) continue;

                if(firstday.AddDays(dayscounter).ToString("dd.MM.yyyy") == DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    grid.Add(new Frame
                    {
                        CornerRadius = 25,
                        BackgroundColor = Colors.LightGray
                    }, j, i);
                    grid.Add(new Label
                    {
                        Text = firstday.AddDays(dayscounter).Day.ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Parse("Red")
                    }, j, i);

                    dayscounter++;
                }
                else if(firstday.AddDays(dayscounter).Month > lastDayOfMonth.Month || firstday.AddDays(dayscounter).Year > lastDayOfMonth.Year)
                {
                    if(firstday.AddDays(dayscounter).Year > lastDayOfMonth.Year)
                    {
                        grid.Add(new Frame
                        {
                            CornerRadius = 25,
                            BackgroundColor = Colors.LightGray
                        }, j, i);
                        grid.Add(new Label
                        {
                            Text = lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString() + "." + lastDayOfMonth.AddDays(1).Month.ToString() + "."+lastDayOfMonth.AddDays(1).Year.ToString(),
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            TextColor = Color.Parse("LightGray")
                        }, j, i);

                        dayscounter_nextmonth++;
                    }
                    else
                    {
                        grid.Add(new Frame
                        {
                            CornerRadius = 25,
                            BackgroundColor = Colors.LightGray
                        }, j, i);
                        grid.Add(new Label
                        {
                            Text = lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString() + "." + lastDayOfMonth.AddDays(1).Month.ToString() + ".",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            TextColor = Color.Parse("LightGray")
                        }, j, i);

                        dayscounter_nextmonth++;
                    }
                }
                else
                {
                    grid.Add(new Frame
                    {
                        CornerRadius = 25,
                        BackgroundColor = Colors.LightGray
                    }, j, i);
                    grid.Add(new Label
                    {
                        Text = firstday.AddDays(dayscounter).Day.ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }, j, i);

                    dayscounter++;
                }
            }
        }

        
        cp1.Content = grid;
        
    }
}

