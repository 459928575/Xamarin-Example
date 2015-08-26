using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMTest.ViewModels {
    public class UserInfoViewModel : Screen {

        private string _account = "";
        public string Account {
            get {
                return this._account;
            }
            set {
                this._account = value;
                this.NotifyOfPropertyChange(() => this._account);
            }
        }

        public DateTime LoginedOn {
            get;
            set;
        }

        private string _tag = "aaa";
        public string Tag {
            get {
                return this._tag;
            }
            set {
                this.Tag = value;
            }
        }


        private int I = 0;

        public Screen VM {
            get;
            set;
        }



        public UserInfoViewModel() {
            this.ChangeVM();
        }

        private void ChangeVM() {
            Task.Delay(2000)
                .ContinueWith(t => {
                    this.VM = this.I++ % 2 == 0 ? (Screen)new ControlViewModel() : new StringViewModel() {
                        Ctx = this.I.ToString()
                    };
                    this.NotifyOfPropertyChange(() => this.VM);
                })
            .ContinueWith(t => {
                ChangeVM();
            });
        }
    }
}
