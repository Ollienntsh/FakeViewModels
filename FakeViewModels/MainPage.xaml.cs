using FakeViewModels.Core;
using FakeViewModels.ViewModels;

namespace FakeViewModels
{
    public class MainPageBase : PageBase<CompanyViewModel>
    {

    }

    public sealed partial class MainPage : MainPageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
