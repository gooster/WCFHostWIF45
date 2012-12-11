using System.ServiceModel;

namespace WCFHostWIF45
{
    [ServiceContract]
    public interface IGradeService
    {
        [OperationContract]
        string GetGrade(int value);

        [OperationContract]
        void WriteGrade(int value);
    }
}
