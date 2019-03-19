namespace ZephyrTest.Models.WebServices
{
    interface IWebServiceEntity
    {
        void AssertEquals(IWebServiceEntity other);
        IWebServiceEntity Clone();
        IWebServiceEntity Invalidate();
        IWebServiceEntity Update();
        string GetName();
        string GetId();
        string ToString();
    }
}
