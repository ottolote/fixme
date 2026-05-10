using FixMe.Access.Task.Interface;
using DomainTask = FixMe.Access.Task.Interface.Task;

namespace FixMe.Access.Task.Service
{
    public class TaskAccess : ITaskAccess
    {
        private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
        {
            "open",
            "completed",
            "cancelled"
        };

        private readonly TaskResource _resource;

        public TaskAccess()
            : this(TaskResource.Shared)
        {
        }

        public TaskAccess(TaskResource resource)
        {
            _resource = resource;
        }

        public System.Threading.Tasks.Task<DomainTask?> Filter(TaskCriteria request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return System.Threading.Tasks.Task.FromResult(_resource.Filter(request));
        }

        public System.Threading.Tasks.Task<DomainTask> Store(DomainTask request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ValidateRequiredFields(request);
            ValidateStatusTransition(request);

            return System.Threading.Tasks.Task.FromResult(_resource.Store(request));
        }

        private static void ValidateRequiredFields(DomainTask request)
        {
            if (string.IsNullOrWhiteSpace(request.TaskType))
            {
                throw new ArgumentException("Task type is required.", nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.SubjectReference))
            {
                throw new ArgumentException("Subject reference is required.", nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.RoutingTarget))
            {
                throw new ArgumentException("Routing target is required.", nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Status) || !ValidStatuses.Contains(request.Status))
            {
                throw new ArgumentException("Status must be open, completed, or cancelled.", nameof(request));
            }
        }

        private void ValidateStatusTransition(DomainTask request)
        {
            if (string.IsNullOrWhiteSpace(request.Identifier))
            {
                return;
            }

            DomainTask? existing = _resource.Filter(new TaskCriteria { Identifier = request.Identifier });
            if (existing is null || string.Equals(existing.Status, "open", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            throw new ArgumentException("Closed tasks cannot be updated.", nameof(request));
        }
    }
}
