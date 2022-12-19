//using Android.App;
//using Android.App;
//using Android.Provider;
//using Java.Util.Concurrent;
using Microsoft.Maui.Controls;

namespace Kalender;

public partial class CalendarView : ContentPage
{
	public CalendarView()
	{
		InitializeComponent();
        SessionData.monthcounter = 0;

        CreateGrid();

        FillBasicData();
    }

    private void FillBasicData()
    {
        SessionData.editorname = "rudolf";
        SessionData.editorsurname = "kevin";
    }

    private HorizontalStackLayout CreateArrows()
    {
        HorizontalStackLayout hstl = new HorizontalStackLayout();

        var pic_tap_left = new TapGestureRecognizer();
        pic_tap_left.Tapped += (s, e) =>
        {
            SessionData.monthcounter--;
            CreateGrid();
            FillBasicData();
        };

        var pic_tap_right = new TapGestureRecognizer();
        pic_tap_right.Tapped += (s, e) =>
        {
            SessionData.monthcounter++;
            CreateGrid();
            FillBasicData();
        };

        Image image = new Image();
        image.Source = "arrow_left.png";
        image.Bounds.Inflate(10, 10);
        image.HeightRequest = 50;
        image.WidthRequest = 50;
        image.GestureRecognizers.Add(pic_tap_left);

        Image image2 = new Image();
        image2.Source = "arrow_right.png";
        image2.Bounds.Inflate(10, 10);
        image2.HeightRequest = 50;
        image2.WidthRequest = 50;
        image2.GestureRecognizers.Add(pic_tap_right);

        Label lblmonth = new Label();
        lblmonth.Text = basicdate.ToString("MMMM")+" "+basicdate.Year.ToString();
        lblmonth.VerticalTextAlignment = TextAlignment.Center;
        lblmonth.VerticalOptions = LayoutOptions.Center;
        lblmonth.FontAttributes = FontAttributes.Bold;

        hstl.Children.Add(image);
        hstl.Children.Add(lblmonth);
        hstl.Children.Add(image2);

        hstl.Margin = 10;
        hstl.Spacing = 300;
        hstl.HorizontalOptions = LayoutOptions.Center;
        hstl.VerticalOptions = LayoutOptions.Center;

        return hstl;
    }

    private DateTime basicdate = new DateTime();
    private HorizontalStackLayout hzl = new HorizontalStackLayout();

    public void CreateGrid()
    {
        var label_tap = new TapGestureRecognizer();
        label_tap.Tapped += (s, e) =>
        {
            Frame f = (Frame)s;
            VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
            verticalStackLayout = f.Content as VerticalStackLayout;
            Label l = (Label)verticalStackLayout.Children[0];
            Label l2 = (Label)verticalStackLayout.Children[1];

            CalendarModel.CurrentDate = Convert.ToDateTime(l.Text);

            Navigation.PopAsync();
            Navigation.PushAsync(new AddAssignment());
        };

        Grid grid = new Grid
        {
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

        basicdate = DateTime.Now.AddMonths(SessionData.monthcounter);
        //basicdate = DateTime.Now.AddMonths(1);
        DateTime realdate = new DateTime();
        var firstday = new DateTime(basicdate.Year, basicdate.Month, 1);
        var lastDayOfMonth = firstday.AddMonths(1).AddDays(-1);
        //Wie viele Felder vom Vormonat müssen ausgeblendet werden bis heute ist
        int grayfields = (((int)firstday.DayOfWeek) - 1) * -1;
        if(grayfields == 1)
        {
            grayfields = -6;
        }

        //1. Tag vom Vormonat
        var fDoLM = new DateTime(basicdate.Year, basicdate.AddMonths(-1).Month, 1);
        //Letzter Tag vom Vormonat
        var lDoLM = new DateTime(fDoLM.Year, fDoLM.Month, DateTime.DaysInMonth(fDoLM.Year, fDoLM.Month));

        //letzter Tag vom Vormonat - die auszublendenten Tage vom Vormonat = Datum wo Montag sein müsste
        realdate = lDoLM.AddDays(grayfields+1);

        int dayscounter = 0;
        int dayscounter_nextmonth = 1;

        //DisplayAlert(grayfields.ToString(), "OK", "OK");
        //DisplayAlert(Convert.ToString((int)firstday.DayOfWeek), "OK", "OK");

        //wenn der 1. Tag des Monats nicht Montag ist
        if (((int)firstday.DayOfWeek) != 1)
        {
            //fülle so lange bis Vormonat letzten erreicht ist
                for (var k = 0; k < grayfields * -1; k++)
                {
                    Frame f = new Frame();
                    f.BackgroundColor = Colors.LightGray;

                    f.GestureRecognizers.Add(label_tap);


                    Label l = new Label();
                    l.Text = realdate.AddDays(dayscounter).Day.ToString() + "." + realdate.AddDays(dayscounter).Month.ToString() + "." + realdate.AddDays(dayscounter).Year.ToString();
                    l.HorizontalOptions = LayoutOptions.Center;
                    l.VerticalOptions = LayoutOptions.Center;
                    l.TextColor = Color.Parse("Black");

                    Label l2 = new Label();
                    l2.HorizontalOptions = LayoutOptions.Center;
                    l2.VerticalOptions = LayoutOptions.Center;
                    if (SessionData.assdate == l.Text && SessionData.titel != "")
                    {
                        l2.Text = SessionData.titel;
                    }
                    else
                    {
                        l2.Text = "-";
                    }
                    
                    VerticalStackLayout vsl = new VerticalStackLayout();

                    vsl.Children.Add(l);
                    vsl.Children.Add(l2);

                    f.Content = vsl;

                    grid.Add(f, k, 0);

                    dayscounter++;
                }
                dayscounter = 0;
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (i == 0 && j < grayfields * -1) continue;

                //Heute Rot
                if (firstday.AddDays(dayscounter).ToString("dd.MM.yyyy") == DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    Frame f = new Frame();
                    f.BackgroundColor = Colors.Gray;

                    f.GestureRecognizers.Add(label_tap);


                    Label l = new Label();
                    l.Text = firstday.AddDays(dayscounter).Day.ToString() + "." + firstday.AddDays(dayscounter).Month.ToString() + "." + firstday.AddDays(dayscounter).Year.ToString();
                    l.HorizontalOptions = LayoutOptions.Center;
                    l.VerticalOptions = LayoutOptions.Center;
                    l.TextColor = Color.Parse("DarkRed");
                    l.FontAttributes = FontAttributes.Bold;

                    Label l2 = new Label();
                    l2.HorizontalOptions = LayoutOptions.Center;
                    l2.VerticalOptions = LayoutOptions.Center;
                    if (SessionData.assdate == l.Text && SessionData.titel != "")
                    {
                        l2.Text = SessionData.titel;
                    }
                    else
                    {
                        l2.Text = "-";
                    }

                    VerticalStackLayout vsl = new VerticalStackLayout();

                    vsl.Children.Add(l);
                    vsl.Children.Add(l2);

                    f.Content = vsl;

                    grid.Add(f, j, i);

                    dayscounter++;
                }
                //alle felder vor / nach Graubereich
                else if (firstday.AddDays(dayscounter).Month > lastDayOfMonth.Month || firstday.AddDays(dayscounter).Year > lastDayOfMonth.Year)
                {
                    //wenn Felder nach Graubereich sind
                    if (firstday.AddDays(dayscounter).Year > lastDayOfMonth.Year || firstday.AddDays(dayscounter).Month > lastDayOfMonth.Month)
                    {
                        Frame f = new Frame();
                        f.BackgroundColor = Colors.LightGray;

                        f.GestureRecognizers.Add(label_tap);


                        Label l = new Label();
                        l.Text = lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString() + "." + lastDayOfMonth.AddDays(1).Month.ToString() + "." + lastDayOfMonth.AddDays(1).Year.ToString();
                        l.HorizontalOptions = LayoutOptions.Center;
                        l.VerticalOptions = LayoutOptions.Center;
                        l.TextColor = Color.Parse("Black");

                        Label l2 = new Label();
                        l2.HorizontalOptions = LayoutOptions.Center;
                        l2.VerticalOptions = LayoutOptions.Center;
                        if (SessionData.assdate == l.Text && SessionData.titel != "")
                        {
                            l2.Text = SessionData.titel;
                        }
                        else
                        {
                            l2.Text = "-";
                        }

                        VerticalStackLayout vsl = new VerticalStackLayout();

                        vsl.Children.Add(l);
                        vsl.Children.Add(l2);

                        f.Content = vsl;

                        grid.Add(f, j, i);

                        dayscounter_nextmonth++;
                    }
                    //wenn Felder vor Graubereich sind
                    else
                    {
                        Frame f = new Frame();
                        f.BackgroundColor = Colors.LightGray;

                        f.GestureRecognizers.Add(label_tap);


                        Label l = new Label();
                        l.Text = lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString() + "." + lastDayOfMonth.AddDays(1).Month.ToString() + "." + lastDayOfMonth.AddDays(1).Year.ToString();
                        l.HorizontalOptions = LayoutOptions.Center;
                        l.VerticalOptions = LayoutOptions.Center;
                        l.TextColor = Color.Parse("Black");

                        Label l2 = new Label();
                        l2.HorizontalOptions = LayoutOptions.Center;
                        l2.VerticalOptions = LayoutOptions.Center;
                        if (SessionData.assdate == l.Text && SessionData.titel != "")
                        {
                            l2.Text = SessionData.titel;
                        }
                        else
                        {
                            l2.Text = "-";
                        }

                        VerticalStackLayout vsl = new VerticalStackLayout();

                        vsl.Children.Add(l);
                        vsl.Children.Add(l2);

                        f.Content = vsl;

                        grid.Add(f, j, i);

                        dayscounter_nextmonth++;
                    }
                }
                else
                {

                    Frame f = new Frame();
                    f.BackgroundColor = Colors.Gray;
                    
                    f.GestureRecognizers.Add(label_tap);
                    

                    Label l = new Label();
                    l.Text = firstday.AddDays(dayscounter).Day.ToString()+"."+ firstday.AddDays(dayscounter).Month.ToString()+"."+ firstday.AddDays(dayscounter).Year.ToString();
                    l.HorizontalOptions = LayoutOptions.Center;
                    l.VerticalOptions = LayoutOptions.Center;

                    Label l2 = new Label();
                    l2.HorizontalOptions = LayoutOptions.Center;
                    l2.VerticalOptions = LayoutOptions.Center;
                    if (SessionData.assdate == l.Text && SessionData.titel != "")
                    {
                        l2.Text = SessionData.titel;
                        l2.FontAttributes = FontAttributes.Bold;
                    }
                    else
                    {
                        l2.Text = "-";
                    }

                    VerticalStackLayout vsl = new VerticalStackLayout();

                    vsl.Children.Add(l);
                    vsl.Children.Add(l2);

                    f.Content = vsl;

                    //f.Content = l;
                    
                    grid.Add(f, j, i);
                    

                    dayscounter++;
                }
            }
        }

        StackLayout stl = new StackLayout();
        stl.Children.Add(CreateArrows());
        stl.Children.Add(grid);

        mainpage.Content = stl;

        

    }

}