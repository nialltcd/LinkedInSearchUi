using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Model;

namespace LinkedInSearchUi.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Model.Model _model;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _model = new Model.Model();
            var data = _model.ParseRawHtmlFilesFromDirectory();
            SearchData = new ObservableCollection<Person>(data);
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Person> _searchData; 
        public ObservableCollection<Person> SearchData{
            get{ return _searchData; }
            set
            {
                _searchData = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SearchButton { get {  return new RelayCommand(SearchAction,()=>true);} }

        private void SearchAction()
        {
            SearchData = new ObservableCollection<Person>(_model.LuceneSearch(SearchText));
        }
    }
}