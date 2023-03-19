using Kalender.Models;
using Kalender.Utils;
using System;
using System.IO;

namespace Kalender;

public partial class CalendarView : ContentPage
{
    public CalendarView()
    {
        InitializeComponent();

        List<CalendarModel> temp = DBHandler.GetAllAssignments();

        foreach(var assignment in temp)
        {
            try
            {
                SessionData.datadic.Add(assignment.date.Date, assignment.titel);
                SessionData.dataimp.Add(assignment.date.Date, assignment.importance);
            }
            catch
            {   }
        }

        SessionData.monthcounter = 0;

        CreateGrid();

        MessagingCenter.Subscribe<Application>(Application.Current, "RefreshMainPage", (sender) =>
        {
            SessionData.datadic.Clear();
            SessionData.dataimp.Clear();

            List<CalendarModel> temp = DBHandler.GetAllAssignments();

            foreach (var assignment in temp)
            {
                try
                {
                    SessionData.datadic.Add(assignment.date.Date, assignment.titel);
                    SessionData.dataimp.Add(assignment.date.Date, assignment.importance);
                }
                catch
                {
                }
            }

            CreateGrid();
        });
    }

    private HorizontalStackLayout CreateArrows()
    {
        //basic creation of arrows and month --> header
        HorizontalStackLayout hstl = new HorizontalStackLayout();

        var pic_tap_left = new TapGestureRecognizer();
        pic_tap_left.Tapped += (s, e) =>
        {
            SessionData.monthcounter--;
            CreateGrid();
        };

        var pic_tap_right = new TapGestureRecognizer();
        pic_tap_right.Tapped += (s, e) =>
        {
            SessionData.monthcounter++;
            CreateGrid();
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
        lblmonth.Text = basicdate.ToString("MMMM") + " " + basicdate.Year.ToString();
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
        //click on any calendar frames
        var label_tap = new TapGestureRecognizer();
        label_tap.Tapped += (s, e) =>
        {
            Frame f = (Frame)s;
            VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
            verticalStackLayout = f.Content as VerticalStackLayout;
            Label l = (Label)verticalStackLayout.Children[0];
            Label l2 = (Label)verticalStackLayout.Children[1];

            SessionData.CurrentDate = Convert.ToDateTime(l.Text);

            Navigation.PopAsync();
            Navigation.PushAsync(new AddAssignment());
        };

        var label_movefrom = new DragGestureRecognizer();

        label_movefrom.DragStarting += (object s, DragStartingEventArgs e) =>
        {
            //var label = (s as Element)?.Parent as Label;
            //e.Data.Properties.Add("Text", label.Text);



            Frame data = s as Frame;

            VerticalStackLayout items = (VerticalStackLayout)data.Content;

            var item = items[0];


            //e.Data.Properties.Add("Text", (VerticalStackLayout)(data.Content).Ch;

            var f = (s as Element)?.Parent as Frame;
            VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
            verticalStackLayout = f.Content as VerticalStackLayout;
            Label l = (Label)verticalStackLayout.Children[0];
            //e.Data.Properties.Add("Text", l.Text);
        };

        var label_moveto = new DropGestureRecognizer();
        label_moveto.Drop += (object s, DropEventArgs e) =>
        {
            try
            {
                var data = e.Data.Properties["Text"].ToString();

                CalendarModel model = new CalendarModel();
                model = DBHandler.GetAssignmentDateTime(Convert.ToDateTime(SessionData.DragTemp));
                model.date = Convert.ToDateTime(data);
                DBHandler.ModifyAssignment(model);

                SessionData.dataimp.Clear();

                List<CalendarModel> temp = DBHandler.GetAllAssignments();

                foreach (var assignment in temp)
                {
                    try
                    {
                        SessionData.datadic.Add(assignment.date.Date, assignment.titel);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {   }
        };

        //Grid creation 
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

        //filling basic vars 
        basicdate = DateTime.Now.AddMonths(SessionData.monthcounter);
        DateTime realdate = new DateTime();
        var firstday = new DateTime(basicdate.Year, basicdate.Month, 1);
        var lastDayOfMonth = firstday.AddMonths(1).AddDays(-1);
        //how many fields of the pre-month are shown?
        int grayfields = (((int)firstday.DayOfWeek) - 1) * -1;
        if (grayfields == 1)
        {
            grayfields = -6;
        }

        //first day of pre-Month
        var fDoLM = new DateTime(basicdate.Year, basicdate.AddMonths(-1).Month, 1);
        //last day of pre-Month
        var lDoLM = new DateTime(fDoLM.Year, fDoLM.Month, DateTime.DaysInMonth(fDoLM.Year, fDoLM.Month));

        //last Day of pre-Month - the hidden fields = the monday-date
        realdate = lDoLM.AddDays(grayfields + 1);

        int dayscounter = 0;
        int dayscounter_nextmonth = 1;

        //if first day of month isn't monday
        if (((int)firstday.DayOfWeek) != 1)
        {
            //fill until pre-Month is over
            for (var k = 0; k < grayfields * -1; k++)
            {
                //field creation
                Frame f = new Frame();
                f.BackgroundColor = Colors.LightGray;

                f.GestureRecognizers.Add(label_tap);
                f.GestureRecognizers.Add(label_moveto);
                f.GestureRecognizers.Add(label_movefrom);


                Label l = new Label();
                l.Text = realdate.AddDays(dayscounter).Day.ToString() + "." + realdate.AddDays(dayscounter).Month.ToString() + "." + realdate.AddDays(dayscounter).Year.ToString();
                l.HorizontalOptions = LayoutOptions.Center;
                l.VerticalOptions = LayoutOptions.Center;
                l.TextColor = Color.Parse("Black");


                Label l2 = new Label();
                l2.HorizontalOptions = LayoutOptions.Center;
                l2.VerticalOptions = LayoutOptions.Center;

                DateTime dt = new DateTime(Convert.ToInt32(realdate.AddDays(dayscounter).Year.ToString()), Convert.ToInt32(realdate.AddDays(dayscounter).Month.ToString()), Convert.ToInt32(realdate.AddDays(dayscounter).Day.ToString()));

                try
                {
                    if (SessionData.datadic[dt] != "")
                    {
                        l2.Text = SessionData.datadic[dt];

                        if (SessionData.dataimp[dt] == 2)
                        {
                            l2.TextColor = Colors.Orange;
                        }
                        else if (SessionData.dataimp[dt] == 3)
                        {
                            l2.TextColor = Colors.Red;
                        }
                    }
                    else
                    {
                        l2.Text = "-";
                    }
                }
                catch (Exception ex)
                {
                    //if date is not in dictionray skip
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

                //fill today -- different color
                if (firstday.AddDays(dayscounter).ToString("dd.MM.yyyy") == DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    Frame f = new Frame();
                    f.BackgroundColor = Colors.Gray;

                    f.GestureRecognizers.Add(label_tap);
                    f.GestureRecognizers.Add(label_moveto);
                    f.GestureRecognizers.Add(label_movefrom);


                    Label l = new Label();
                    l.Text = firstday.AddDays(dayscounter).Day.ToString() + "." + firstday.AddDays(dayscounter).Month.ToString() + "." + firstday.AddDays(dayscounter).Year.ToString();
                    l.HorizontalOptions = LayoutOptions.Center;
                    l.VerticalOptions = LayoutOptions.Center;
                    l.TextColor = Color.Parse("DarkRed");
                    l.FontAttributes = FontAttributes.Bold;

                    Label l2 = new Label();
                    l2.HorizontalOptions = LayoutOptions.Center;
                    l2.VerticalOptions = LayoutOptions.Center;

                    DateTime dt = new DateTime(Convert.ToInt32(firstday.AddDays(dayscounter).Year.ToString()), Convert.ToInt32(firstday.AddDays(dayscounter).Month.ToString()), Convert.ToInt32(firstday.AddDays(dayscounter).Day.ToString()));
                    try
                    {
                        if (SessionData.datadic[dt] != "")
                        {
                            l2.Text = SessionData.datadic[dt];

                            if (SessionData.dataimp[dt] == 2)
                            {
                                l2.TextColor = Colors.Orange;
                            }
                            else if (SessionData.dataimp[dt] == 3)
                            {
                                l2.TextColor = Colors.Red;
                            }
                        }
                        else
                        {
                            l2.Text = "-";
                        }
                    }
                    catch (Exception ex)
                    {
                        //if date is not in Dictionary skip
                    }

                    VerticalStackLayout vsl = new VerticalStackLayout();

                    vsl.Children.Add(l);
                    vsl.Children.Add(l2);

                    f.Content = vsl;

                    grid.Add(f, j, i);

                    dayscounter++;
                }
                //all fields pre / past hidden-fields
                else if (firstday.AddDays(dayscounter).Month > lastDayOfMonth.Month || firstday.AddDays(dayscounter).Year > lastDayOfMonth.Year)
                {
                    //if fields are hidden
                    if (firstday.AddDays(dayscounter).Year > lastDayOfMonth.Year || firstday.AddDays(dayscounter).Month > lastDayOfMonth.Month)
                    {
                        Frame f = new Frame();
                        f.BackgroundColor = Colors.LightGray;

                        f.GestureRecognizers.Add(label_tap);
                        f.GestureRecognizers.Add(label_moveto);
                        f.GestureRecognizers.Add(label_movefrom);


                        Label l = new Label();
                        l.Text = lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString() + "." + lastDayOfMonth.AddDays(1).Month.ToString() + "." + lastDayOfMonth.AddDays(1).Year.ToString();
                        l.HorizontalOptions = LayoutOptions.Center;
                        l.VerticalOptions = LayoutOptions.Center;
                        l.TextColor = Color.Parse("Black");

                        Label l2 = new Label();
                        l2.HorizontalOptions = LayoutOptions.Center;
                        l2.VerticalOptions = LayoutOptions.Center;

                        DateTime dt = new DateTime(Convert.ToInt32(lastDayOfMonth.AddDays(1).Year.ToString()), Convert.ToInt32(lastDayOfMonth.AddDays(1).Month.ToString()), Convert.ToInt32(lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString()));
                        try
                        {
                            if (SessionData.datadic[dt] != "")
                            {
                                l2.Text = SessionData.datadic[dt];

                                if (SessionData.dataimp[dt] == 2)
                                {
                                    l2.TextColor = Colors.Orange;
                                }
                                else if (SessionData.dataimp[dt] == 3)
                                {
                                    l2.TextColor = Colors.Red;
                                }
                            }
                            else
                            {
                                l2.Text = "-";
                            }
                        }
                        catch (Exception ex)
                        {
                            //if date is not in Dictionary skip
                        }

                        VerticalStackLayout vsl = new VerticalStackLayout();

                        vsl.Children.Add(l);
                        vsl.Children.Add(l2);
                        f.Content = vsl;

                        grid.Add(f, j, i);

                        dayscounter_nextmonth++;
                    }
                    //if fields are located bevore the hidden part
                    else
                    {
                        Frame f = new Frame();
                        f.BackgroundColor = Colors.LightGray;

                        f.GestureRecognizers.Add(label_tap);
                        f.GestureRecognizers.Add(label_moveto);
                        f.GestureRecognizers.Add(label_movefrom);

                        Label l = new Label();
                        l.Text = lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString() + "." + lastDayOfMonth.AddDays(1).Month.ToString() + "." + lastDayOfMonth.AddDays(1).Year.ToString();
                        l.HorizontalOptions = LayoutOptions.Center;
                        l.VerticalOptions = LayoutOptions.Center;
                        l.TextColor = Color.Parse("Black");

                        Label l2 = new Label();
                        l2.HorizontalOptions = LayoutOptions.Center;
                        l2.VerticalOptions = LayoutOptions.Center;

                        DateTime dt = new DateTime(Convert.ToInt32(lastDayOfMonth.AddDays(1).Year.ToString()), Convert.ToInt32(lastDayOfMonth.AddDays(1).Month.ToString()), Convert.ToInt32(lastDayOfMonth.AddDays(dayscounter_nextmonth).Day.ToString()));
                        try
                        {
                            if (SessionData.datadic[dt] != "")
                            {
                                l2.Text = SessionData.datadic[dt];

                                if (SessionData.dataimp[dt] == 2)
                                {
                                    l2.TextColor = Colors.Orange;
                                }
                                else if (SessionData.dataimp[dt] == 3)
                                {
                                    l2.TextColor = Colors.Red;
                                }
                            }
                            else
                            {
                                l2.Text = "-";
                            }
                        }
                        catch (Exception ex)
                        {
                            //if date is not in Dictionary skip
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
                    //every field which is just a normal day in the current month (not past, not pre, not today)
                    Frame f = new Frame();
                    f.BackgroundColor = Colors.Gray;

                    f.GestureRecognizers.Add(label_tap);
                    f.GestureRecognizers.Add(label_moveto);
                    f.GestureRecognizers.Add(label_movefrom);


                    Label l = new Label();
                    l.Text = firstday.AddDays(dayscounter).Day.ToString() + "." + firstday.AddDays(dayscounter).Month.ToString() + "." + firstday.AddDays(dayscounter).Year.ToString();
                    l.HorizontalOptions = LayoutOptions.Center;
                    l.VerticalOptions = LayoutOptions.Center;

                    Label l2 = new Label();
                    l2.HorizontalOptions = LayoutOptions.Center;
                    l2.VerticalOptions = LayoutOptions.Center;

                    DateTime dt = new DateTime(Convert.ToInt32(firstday.AddDays(dayscounter).Year.ToString()), Convert.ToInt32(firstday.AddDays(dayscounter).Month.ToString()), Convert.ToInt32(firstday.AddDays(dayscounter).Day.ToString()));
                    try
                    {
                        if (SessionData.datadic[dt] != "")
                        {
                            l2.Text = SessionData.datadic[dt];

                            if (SessionData.dataimp[dt] == 2) 
                            {
                                l2.TextColor = Colors.Orange;
                            }
                            else if (SessionData.dataimp[dt] == 3)
                            {
                                l2.TextColor = Colors.Red;
                            }
                        }
                        else
                        {
                            l2.Text = "-";
                        }
                    }
                    catch (Exception ex)
                    {
                        //if date is not in Dictionary skip
                    }

                    VerticalStackLayout vsl = new VerticalStackLayout();

                    vsl.Children.Add(l);
                    vsl.Children.Add(l2);

                    f.Content = vsl;

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