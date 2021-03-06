﻿using System;
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

        [FakeImage(50, 50, FakeImageType.People)]
        public string ImageUrl { get; set; }
    }

    public class ComplexObject
    {
        public Person Person { get; set; }
        public decimal Salary { get; set; }
    }

    public class CompanyViewModel : IViewModel
    {
        [FakeName("Microsoft")]
        public string CompanyName { get; set; }

        public ComplexObject ComplexObject { get; set; }

        [FakeName]
        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        [FakeDate(FakeDateType.Birthday)]
        public DateTime Birthday { get; set; }

        [FakeImage(200,200, FakeImageType.Cats)]
        public string ImageUrl { get; set; }

        [FakeCollection(5)]
        public ObservableCollection<Person> People { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
        }
    }
}
