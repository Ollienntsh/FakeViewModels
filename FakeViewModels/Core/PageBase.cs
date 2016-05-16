using FakeViewModels.Interfaces;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace FakeViewModels.Core
{
    public abstract class PageBase<T> : Page, IPage<T> where T : IViewModel, new()
    {
        #region properties

        public T ViewModel { get; set; }

        #endregion

        #region constructors

        public PageBase()
        {
            if (DesignMode.DesignModeEnabled)
            {
                this.ViewModel = ViewModelFactory.CreateFakeViewModel<T>();
                this.DataContext = this;
            }
        } 

        #endregion
    }
}
