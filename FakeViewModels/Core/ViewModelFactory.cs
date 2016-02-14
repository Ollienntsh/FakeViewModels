using FakeViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FakeViewModels.Core
{
    public static class ViewModelFactory
    {
        public static T CreateFakeViewModel<T>() where T : IViewModel, new()
        {
            T fakeViewModel = new T();
            fakeViewModel.SetFakeData();
            return fakeViewModel;
        }

        private static void SetFakeData(this object instance)
        {
            Type viewModelType = instance.GetType();

            foreach (PropertyInfo propertyInfo in viewModelType.GetProperties())
            {
                Type propertyType = propertyInfo.PropertyType;

                if (propertyType == typeof(string))
                {
                    instance.SetFakeStringProperty(propertyInfo);
                }
                else if (propertyType == typeof(DateTime))
                {
                    instance.SetFakeDateProperty(propertyInfo);
                }
                else
                {
                    Type genericCollectionType = propertyType.GetInterfaces()
                                                             .Where(interfaceType => interfaceType.GetTypeInfo().IsGenericType)
                                                             .FirstOrDefault(interfaceType => (interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>)));

                    if (genericCollectionType != null)
                    {
                        Type itemType = propertyType.GetGenericArguments().First();
                        ConstructorInfo itemConstructorInfo = itemType.GetConstructor(Type.EmptyTypes);
                        MethodInfo addMethodInfo = propertyType.GetMethod("Add");
                        int itemCount = propertyInfo.GetCustomAttribute<FakeCollectionAttribute>()?.ItemCount ?? 10;

                        if ((itemConstructorInfo) != null && (addMethodInfo != null))
                        {
                            object collection = Activator.CreateInstance(propertyType);

                            for (int i = 0; i < itemCount; i++)
                            {
                                object item = Activator.CreateInstance(itemType);
                                item.SetFakeData();
                                addMethodInfo.Invoke(collection, new[] { item });
                            }

                            propertyInfo.SetValue(instance, collection);
                        }
                    }
                }
            }
        }

        private static void SetFakeDateProperty(this object instance, PropertyInfo propertyInfo)
        {
            FakeDateAttribute fakeDateAttribute = propertyInfo.GetCustomAttribute<FakeDateAttribute>();

            if (fakeDateAttribute != null)
            {
                FakeDateType dateType = fakeDateAttribute.DateType;

                switch (dateType)
                {
                    case FakeDateType.Birthday:
                    {
                        propertyInfo.SetValue(instance, Faker.Date.Birthday());
                        break;
                    }
                    case FakeDateType.Future:
                    {
                        propertyInfo.SetValue(instance, Faker.Date.Between(DateTime.Now, DateTime.MaxValue));
                        break;
                    }
                    case FakeDateType.Past:
                    {
                        propertyInfo.SetValue(instance, Faker.Date.Between(DateTime.MinValue, DateTime.Now));
                        break;
                    }
                }
            }
            else
            {
                propertyInfo.SetValue(instance, Faker.Date.Between(DateTime.MinValue, DateTime.Now));
            }
        }

        public static void SetFakeStringProperty(this object instance, PropertyInfo propertyInfo)
        {
            FakeDataAttribute fakeDataAttribute = propertyInfo.GetCustomAttributes<FakeDataAttribute>().FirstOrDefault();

            if (fakeDataAttribute is FakeImageAttribute)
            {
                FakeImageAttribute fakeImageAttribute = fakeDataAttribute as FakeImageAttribute;
                string imageUrl = String.Format("http://lorempixel.com/{0}/{1}/{2}", 
                    fakeImageAttribute.Width ?? 500, fakeImageAttribute.Height ?? 500, fakeImageAttribute.ImageType.ToString().ToLower());

                propertyInfo.SetValue(instance, imageUrl);
            }
            else if (fakeDataAttribute is FakeNameAttribute)
            {
                FakeNameAttribute fakeNameAttribute = fakeDataAttribute as FakeNameAttribute;
                propertyInfo.SetValue(instance, fakeNameAttribute.Name ?? Faker.Name.FullName());
            }
            else
            {
                propertyInfo.SetValue(instance, Faker.Lorem.Sentence());
            }
        }
    }
}
