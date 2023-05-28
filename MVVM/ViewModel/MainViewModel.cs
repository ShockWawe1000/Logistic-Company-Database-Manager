using K_project.Core;
using System;

namespace K_project.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }

        public RelayCommand DatabaseViewCommand { get; set; }


        public HomeViewModel HomeVM { get; set; }
        public DiscoveryViewModel DiscoveryVM { get; set; }
        public DatabaseViewModel DatabaseVM { get; set; }

        private object _currentView;


        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnProperyChanged();
            }
        }

        public MainViewModel() 
        {
        HomeVM = new HomeViewModel();  
        DiscoveryVM = new DiscoveryViewModel() ;
            DatabaseVM = new DatabaseViewModel();

            CurrentView = HomeVM;

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = DiscoveryVM;
            });

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            DatabaseViewCommand = new RelayCommand(o =>
            {
                CurrentView = DatabaseVM;
            });
        }  
    }
}
