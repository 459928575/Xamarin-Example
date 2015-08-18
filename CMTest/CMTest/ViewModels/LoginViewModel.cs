using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMTest.ViewModels {
    public class LoginViewModel : Screen {

        private INavigationService NS = null;
        private SimpleContainer CN = null;

        private string _account = "";
        public string Account {
            get {
                return this._account;
            }
            set {
                this._account = value;
                this.NotifyOfPropertyChange(() => this.Account);
                this.NotifyOfPropertyChange(() => this.CanLogin);
            }
        }

        private string _pwd = "";
        public string Pwd {
            get {
                return this._pwd;
            }
            set {
                this._pwd = value;
                this.NotifyOfPropertyChange(() => this.Pwd);
                this.NotifyOfPropertyChange(() => this.CanLogin);
            }
        }


        public bool CanLogin {
            get {
                return !string.IsNullOrWhiteSpace(this.Account)
                    && !string.IsNullOrWhiteSpace(this.Pwd);
            }
        }


        public LoginViewModel(INavigationService ns, SimpleContainer continer) {
            this.NS = ns;
            this.CN = continer;
        }


        public void Login() {

            //var u = this.CN.GetInstance<UserInfoViewModel>();

            //如果不在 App.cs 中显示注册为 PerRequest 或者 Singleton, 
            //以下代码不会实例 UserInfoViewModel, 只是把它对应的 View 给实例出来。
            this.NS.For<UserInfoViewModel>()
                .WithParam(v => v.Account, this.Account)
                .WithParam(v => v.LoginedOn, DateTime.Now)
                .Navigate();
        }

    }
}
