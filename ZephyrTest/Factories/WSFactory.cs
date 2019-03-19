using ZephyrTest.Models.WebServices;

namespace ZephyrTest.Factories
{
    class WSFactory<T> where T: IWebServiceEntity
    {
        private static T E;
        public static T GenerateEntity<T2>() where T2 : T, new() => SetEntity(new T2());
        public static T GetEntity() => E;
        public static T SetEntity(T e) => E = e;
        public static T UpdateEntity() => SetEntity((T)E.Update());
        public static T CloneEntity() => (T)E.Clone();
        public static T InvalidateEntity() => (T)CloneEntity().Invalidate();
    }
}
