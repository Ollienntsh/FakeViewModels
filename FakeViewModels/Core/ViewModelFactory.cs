using FakeViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FakeViewModels.Core
{
    public static class ViewModelFactory
    {
        #region public methods

        public static T CreateFakeViewModel<T>() where T : IViewModel, new()
        {
            T fakeViewModel = new T();
            fakeViewModel.SetFakeData();
            return fakeViewModel;
        }

        #endregion

        #region non-public methods

        private static string CreateLoremPixelUrl(FakeDataAttribute fakeDataAttribute)
        {
            FakeImageAttribute fakeImageAttribute = fakeDataAttribute as FakeImageAttribute;
            string imageUrl = null;

            if (fakeDataAttribute != null)
            {
                int imageWidth = fakeImageAttribute.Width ?? 500;
                int imageHeight = fakeImageAttribute.Height ?? 500;
                imageUrl = GetLoremPixelImageUrl(fakeImageAttribute.ImageType, imageWidth, imageHeight);
            }

            return imageUrl;
        }

        private static string GetLoremPixelImageUrl(FakeImageType fakeImageType, int imageWidth, int imageHeight)
        {
            Guid guid = Guid.NewGuid();
            string loremPixelBaseUrl = "http://lorempixel.com";
            string imageTypeString = fakeImageType.ToString().ToLower();
            string imageUrl = $"{loremPixelBaseUrl}/{imageWidth}/{imageHeight}/{imageTypeString}?ignore={guid}";
            return imageUrl;
        }

        private static bool HasDefaultConstructor(this Type type)
        {
            bool hasDefaultConstructor = type.GetTypeInfo().IsValueType || (type.GetConstructor(Type.EmptyTypes) != null);
            return hasDefaultConstructor;
        }

        private static bool IsGenericCollectionType(this Type type)
        {
            Type genericCollectionType = (from interfaceType in type.GetInterfaces()
                                          where interfaceType.GetTypeInfo().IsGenericType && (interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>))
                                          select interfaceType).FirstOrDefault();
            bool isGenericCollectionType = genericCollectionType != null;

            return isGenericCollectionType;
        }

        private static void SetFakeData(this object instance, int level = 0)
        {
            Type viewModelType = instance.GetType();

            foreach (PropertyInfo propertyInfo in viewModelType.GetProperties())
            {
                Type propertyType = propertyInfo.PropertyType;
                TypeInfo propertyTypeInfo = propertyInfo.PropertyType.GetTypeInfo();

                if (propertyType == typeof(string))
                {
                    instance.SetFakeStringProperty(propertyInfo);
                }
                else if (propertyTypeInfo.IsValueType)
                {
                    instance.SetFakeValueProperty(propertyInfo);
                }
                else if (propertyType.IsGenericCollectionType())
                {
                    instance.SetFakeCollectionProperty(propertyInfo);
                }
                else if (propertyType.HasDefaultConstructor() && (level < 2))
                {
                    object innerInstance = Activator.CreateInstance(propertyType);
                    innerInstance.SetFakeData(level + 1);
                }
            }
        }

        private static void SetFakeCollectionProperty(this object instance, PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            Type itemType = propertyType.GetGenericArguments().First();
            MethodInfo addMethodInfo = propertyType.GetMethod(nameof(ICollection<object>.Add));
            int itemCount = propertyInfo.GetCustomAttribute<FakeCollectionAttribute>()?.ItemCount ?? 10;

            if (propertyType.HasDefaultConstructor() && itemType.HasDefaultConstructor() && (addMethodInfo != null))
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
                string imageUrl = CreateLoremPixelUrl(fakeDataAttribute);
                propertyInfo.SetValue(instance, imageUrl);
            }
            else if (fakeDataAttribute is FakeNameAttribute)
            {
                FakeNameAttribute fakeNameAttribute = fakeDataAttribute as FakeNameAttribute;
                propertyInfo.SetValue(instance, fakeNameAttribute.Name ?? Faker.Name.FullName());
            }
            else
            {
                propertyInfo.SetValue(instance, String.Join(Environment.NewLine, Faker.Lorem.Paragraphs(5)));
            }
        } 

        public static void SetFakeValueProperty(this object instance, PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;

            if (propertyType == typeof(decimal))
            {
                propertyInfo.SetValue(instance, (decimal)Faker.RandomNumber.NextDouble());
            }
            else if (propertyType == typeof(DateTime))
            {
                instance.SetFakeDateProperty(propertyInfo);
            }
        }

        #endregion
    }
}
