using System;
using System.ComponentModel;
using FakeViewModels.Core;
using FakeViewModels.Interfaces;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace FakeViewModels.ViewModels
{
    public class Person
    {
        [FakeName]
        public string Name { get; set; }

        [FakeImage(200, 200, FakeImageType.People)]
        public string ImageUrl { get; set; }
    }

    public class CompanyViewModel : IViewModel
    {
        [FakeName("Microsoft")]
        public string CompanyName { get; set; }

        [FakeName]
        public string PhoneNumber { get; set; }

        [FakeName]
        public string Description { get; set; }

        [FakeDate(FakeDateType.Birthday)]
        public DateTime Birthday { get; set; }

        [FakeImage(200,200, FakeImageType.City)]
        public string ImageUrl { get; set; }

        [FakeCollection(5)]
        public ObservableCollection<Person> People { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Dispose()
        {
            
        }
    }
}
