using TaxiApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

//using TaxiApp.Core.Messages;
//using GalaSoft.MvvmLight.Messaging;
//using GalaSoft.MvvmLight.Ioc;

// Документацию по шаблону элемента "Основная страница" см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace TaxiApp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;

        //private TaxiApp.ViewModel.EditOrderViewModel editOrderViewModel = ViewModel.ViewModelFactory.Instance.GetViewOrderModel();

        public MainPage()
        {
            this.InitializeComponent();

            //editOrderViewModel.Pivot = this.pivot;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            //SimpleIoc.Default.Register<TaxiApp.Core.Managers.IMenu, TaxiApp.Core.u>();
        }

        /// <summary>
        /// Получает объект <see cref="NavigationHelper"/>, связанный с данным объектом <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации.  Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Это состояние будет равно NULL при первом посещении страницы.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //this.editOrderViewModel.LoadMyOrders();
        }

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации.  Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">Источник события; как правило, <see cref="NavigationHelper"/></param>
        /// <param name="e">Данные события, которые предоставляют пустой словарь для заполнения
        /// сериализуемым состоянием.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region Регистрация NavigationHelper

        /// <summary>
        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// <para>
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="NavigationHelper.LoadState"/>
        /// и <see cref="NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.
        /// </para>
        /// </summary>
        /// <param name="e">Предоставляет данные для методов навигации и обработчики
        /// событий, которые не могут отменить запрос навигации.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SocketPgBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(SocketPage)); 
        }

        private bool _triggerCompleted;
        private const double SideMenuCollapsedLeft = 0;
        private const double SideMenuExpandedLeft = 300;

        private void Sidebar_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //_triggerCompleted = true;

            //double finalLeft = Canvas.GetLeft(Sidebar) + e.Delta.Translation.X;
            //if (finalLeft > 300 || finalLeft < 0)
            //    return;

            //Canvas.SetLeft(Sidebar, finalLeft);

            //if (e.IsInertial && e.Velocities.Linear.X > 1)
            //{
            //    _triggerCompleted = false;
            //    e.Complete();
            //    MoveLeft(SideMenuExpandedLeft);
            //}

            //if (e.IsInertial && e.Velocities.Linear.X < -1)
            //{
            //    _triggerCompleted = false;
            //    e.Complete();
            //    MoveLeft(SideMenuCollapsedLeft);
            //}

            //if (e.IsInertial && Math.Abs(e.Velocities.Linear.X) <= 1)
            //    e.Complete();
        }

        private void Sidebar_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //if (_triggerCompleted == false)
            //    return;

            //double finalLeft = Canvas.GetLeft(Sidebar);

            //if (finalLeft > 170)
            //    MoveLeft(SideMenuExpandedLeft);
            //else
            //    MoveLeft(SideMenuCollapsedLeft);
        }

        private void MoveLeft(double left)
        {
            //double finalLeft = Canvas.GetLeft(Sidebar);

            //Storyboard moveAnivation = ((Storyboard)RootCanvas.Resources["MoveAnimation"]);
            //DoubleAnimation direction = ((DoubleAnimation)((Storyboard)RootCanvas.Resources["MoveAnimation"]).Children[0]);

            //direction.From = finalLeft;

            //moveAnivation.SkipToFill();

            //direction.To = left;

            //moveAnivation.Begin();
        }

    }
}
