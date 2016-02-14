using System;
using System.ComponentModel;

namespace FakeViewModels.Interfaces
{
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
    }
}
