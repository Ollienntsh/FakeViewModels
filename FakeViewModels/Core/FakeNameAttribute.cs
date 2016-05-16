namespace FakeViewModels.Core
{
    public class FakeNameAttribute : FakeDataAttribute
    {
        #region properties

        public string Name { get; private set; }

        #endregion

        #region constructors

        public FakeNameAttribute()
        {
        }

        public FakeNameAttribute(string name)
        {
            this.Name = name;
        } 

        #endregion
    }
}
