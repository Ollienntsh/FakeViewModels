namespace FakeViewModels.Core
{
    public class FakeNameAttribute : FakeDataAttribute
    {
        public string Name { get; private set; }

        public FakeNameAttribute()
        {
        }

        public FakeNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
