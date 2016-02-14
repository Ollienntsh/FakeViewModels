namespace FakeViewModels.Interfaces
{
    public interface IPage<T> where T : IViewModel
    {
        T ViewModel { get; set; }
    }
}
