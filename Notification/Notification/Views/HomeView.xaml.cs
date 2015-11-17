using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Notification.Views {
    public partial class HomeView : MasterDetailPage {
        public HomeView() {
            InitializeComponent();


            Label header = new Label {
                Text = "MasterDetailPage",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            // Assemble an array of NamedColor objects.
            NamedColor[] namedColors = 
                {
                    new NamedColor("Aqua", Color.Aqua),
                    new NamedColor("Black", Color.Black),
                    new NamedColor("Blue", Color.Blue),
                    new NamedColor("Fuschia", Color.Fuschia),
                    new NamedColor("Gray", Color.Gray),
                    new NamedColor("Green", Color.Green),
                    new NamedColor("Lime", Color.Lime),
                    new NamedColor("Maroon", Color.Maroon),
                    new NamedColor("Navy", Color.Navy),
                    new NamedColor("Olive", Color.Olive),
                    new NamedColor("Purple", Color.Purple),
                    new NamedColor("Red", Color.Red),
                    new NamedColor("Silver", Color.Silver),
                    new NamedColor("Teal", Color.Teal),
                    new NamedColor("White", Color.White),
                    new NamedColor("Yellow", Color.Yellow)
                };

            // Create ListView for the master page.
            ListView listView = new ListView {
                ItemsSource = namedColors
            };

            // Create the master page with the ListView.
            this.Master = new ContentPage {
                Title = header.Text,
                Content = new StackLayout {
                    Children = 
                    {
                        header, 
                        listView
                    }
                }
            };

            // Create the detail page using NamedColorPage and wrap it in a
            // navigation page to provide a NavigationBar and Toggle button
            this.Detail = new NavigationPage(new NamedColorPage(true));

            // For Windows Phone, provide a way to get back to the master page.
            //if (Device.OS == TargetPlatform.WinPhone) {
            //    (this.Detail as ContentPage).Content.GestureRecognizers.Add(
            //        new TapGestureRecognizer((view) => {
            //            this.IsPresented = true;
            //        }));
            //}

            // Define a selected handler for the ListView.
            listView.ItemSelected += (sender, args) => {
                // Set the BindingContext of the detail page.
                this.Detail.BindingContext = args.SelectedItem;

                // Show the detail page.
                this.IsPresented = false;
            };

            // Initialize the ListView selection.
            listView.SelectedItem = namedColors[0];
        }
    }

    class NamedColor {
        public NamedColor(string name, Color color) {
            this.Name = name;
            this.Color = color;
        }

        public string Name {
            private set;
            get;
        }

        public Color Color {
            private set;
            get;
        }

        public override string ToString() {
            return Name;
        }
    }






    class NamedColorPage : ContentPage {
        public NamedColorPage(bool includeBigLabel) {
            // This binding is necessary to label the tabs in 
            //      the TabbedPage.
            this.SetBinding(ContentPage.TitleProperty, "Name");

            // BoxView to show the color.
            BoxView boxView = new BoxView {
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center
            };
            boxView.SetBinding(BoxView.ColorProperty, "Color");

            // Function to create six Labels.
            Func<string, string, Label> CreateLabel = (string source, string fmt) => {
                Label label = new Label {
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    XAlign = TextAlignment.End
                };
                label.SetBinding(Label.TextProperty,
                    new Binding(source, BindingMode.OneWay, null, null, fmt));

                return label;
            };

            // Build the page
            this.Content = new StackLayout {
                Children = 
                {
                    new StackLayout
                    {   
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Children = 
                        {
                            CreateLabel("Color.R", "R = {0:F2}"),
                            CreateLabel("Color.G", "G = {0:F2}"),
                            CreateLabel("Color.B", "B = {0:F2}"),
                        }
                    },
                    boxView,
                    new StackLayout
                    {   
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Children = 
                        {
                            CreateLabel("Color.Hue", "Hue = {0:F2}"),
                            CreateLabel("Color.Saturation", "Saturation = {0:F2}"),
                            CreateLabel("Color.Luminosity", "Luminosity = {0:F2}")
                        }
                    }
                }
            };

            // Add in the big Label at top for CarouselPage.
            if (includeBigLabel) {
                Label bigLabel = new Label {
                    FontSize = 50,
                    HorizontalOptions = LayoutOptions.Center
                };
                bigLabel.SetBinding(Label.TextProperty, "Name");

                (this.Content as StackLayout).Children.Insert(0, bigLabel);
            }
        }
    }
}
