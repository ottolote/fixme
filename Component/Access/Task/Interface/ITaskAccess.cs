using System.Threading.Tasks;

namespace FixMe.Access.Task.Interface
{
    public interface ITaskAccess
    {
        Task<Task> Filter(TaskCriteria request);
        Task<Task> Store(Task request);
    }
}
