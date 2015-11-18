using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Notification.ViewModels {
    public class MasterViewModel : Screen {

        public Dictionary<string, Color> Colors {
            get; set;
        }

        public string Title { get; set; }

        public ICommand TestCmd { get; set; }

        public MasterViewModel() {
            this.TestCmd = new Command(() => {

            });

            this.Colors = new Dictionary<string, Color>() {
                {"Aqua", Color.Aqua},
                {"Black", Color.Black},
                {"Blue", Color.Blue},
                {"Fuchsia", Color.Fuchsia},
                {"Gray", Color.Gray},
                {"Green", Color.Green},
                {"Lime", Color.Lime},
                {"Maroon", Color.Maroon},
                {"Navy", Color.Navy},
                {"Olive", Color.Olive},
                {"Purple", Color.Purple},
                {"Red", Color.Red},
                {"Silver", Color.Silver},
                {"Teal", Color.Teal},
                {"White", Color.White},
                {"Yellow", Color.Yellow}
            };
            this.Title = "AAA";
            this.NotifyOfPropertyChange(() => this.Colors);
            this.NotifyOfPropertyChange(() => this.Title);
            this.NotifyOfPropertyChange(() => this.TestCmd);
        }

    }
}
