//FlyPassword
//Copyright(C) yinyue200.com 

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, version 3 of the License.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see https://github.com/yinyue200/FlyPassword/blob/master/LICENSE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FlyPassword.UWP.ViewModels;
using Windows.UI.Xaml.Media.Animation;
using CorePasswordKeeper;
using FlyPassword.UWP.Core;
using System.Collections.ObjectModel;
using Windows.Globalization.Collation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyPassword.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterPage : Page,IRefreshable
    {
        public MasterPage()
        {
            this.InitializeComponent();
        }
        private PasswordRecordViewModel _lastSelectedItem;
        Func<object> recordsAction;
        IEnumerable<Record> records;


        public RangedObservableCollection<PasswordRecordViewModelGroup> observablerecords
        {
            get { return (RangedObservableCollection<PasswordRecordViewModelGroup>)GetValue(observablerecordsProperty); }
            set { SetValue(observablerecordsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty observablerecordsProperty =
            DependencyProperty.Register("observablerecords", typeof(RangedObservableCollection<PasswordRecordViewModelGroup>), typeof(MasterPage), new PropertyMetadata(null));

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            records = (recordsAction = (e.Parameter as Func<object>) ?? (() => null))() as IEnumerable<Record>;
            Load();
        }

        public void Refresh()
        {
            records = recordsAction() as IEnumerable<Record>;
            Load();
            //if(_lastSelectedItem!=null&& !string.IsNullOrWhiteSpace(_lastSelectedItem.ItemId))
            //{
                
            //}
        }
        private void Load()
        {
            if (records == null)
            {
                records = TmpData.PasswordKeeper.Records;
            }
            CharacterGroupings groupings = new CharacterGroupings();

            observablerecords = new RangedObservableCollection<PasswordRecordViewModelGroup>(records.Select(a => PasswordRecordViewModel.CreateFromRecord(a))
                .OrderBy(a => a.DisplayName).GroupBy((a) => groupings.Lookup(a.DisplayName))
                .Select(a => new PasswordRecordViewModelGroup(a.Key, ct(a))));

            UpdateForVisualState(AdaptiveStates.CurrentState);

            // Don't play a content transition for first item load.
            // Sometimes, this content will be animated as part of the page transition.
            DisableContentTransitions();
        }

        RangedObservableCollection<T> ct<T>(IEnumerable<T> ts)
        {
            var a = new RangedObservableCollection<T>();
            a.AddRange(ts);
            return a;
        }
        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState = null)
        {
            var isNarrow = newState == NarrowState;

            if (isNarrow && oldState == DefaultState && _lastSelectedItem != null)
            {
                // Resize down to the detail item. Don't play a transition.
                Frame.Navigate(typeof(RecordDetailPage), _lastSelectedItem, new SuppressNavigationTransitionInfo());
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (PasswordRecordViewModel)e.ClickedItem;
            _lastSelectedItem = clickedItem;

            if (AdaptiveStates.CurrentState == NarrowState)
            {
                // Use "drill in" transition for navigating from master list to detail view
                Frame.Navigate(typeof(RecordDetailPage), clickedItem.ItemId, new DrillInNavigationTransitionInfo());
            }
            else
            {
                // Play a refresh animation when the user switches detail items.
                EnableContentTransitions();

            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            // Assure we are displaying the correct item. This is necessary in certain adaptive cases.
            MasterListView.SelectedItem = _lastSelectedItem;
        }

        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        private void DisableContentTransitions()
        {
            if (DetailContentPresenter != null)
            {
                DetailContentPresenter.ContentTransitions.Clear();
            }
        }

    }
}
