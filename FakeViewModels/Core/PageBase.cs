using FakeViewModels.Interfaces;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace FakeViewModels.Core
{
    public class PageBase<T> : Page, IPage<T> where T : IViewModel, new()
    {
        public T ViewModel { get; set; }

        public PageBase()
        {
            if (DesignMode.DesignModeEnabled)
            {
                this.ViewModel = ViewModelFactory.CreateFakeViewModel<T>();
                this.DataContext = this;
            }
        }
    }
}
