using FixMe.Access.Task.Interface;
using DomainTask = FixMe.Access.Task.Interface.Task;

namespace FixMe.Access.Task.Service
{
    public class TaskResource
    {
        public static TaskResource Shared { get; } = new();

        private readonly List<DomainTask> _tasks = [];
        private readonly object _syncRoot = new();

        public DomainTask? Filter(TaskCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            lock (_syncRoot)
            {
                return _tasks.FirstOrDefault(task => Matches(task, criteria));
            }
        }

        public DomainTask Store(DomainTask task)
        {
            ArgumentNullException.ThrowIfNull(task);

            lock (_syncRoot)
            {
                task.Identifier = string.IsNullOrWhiteSpace(task.Identifier)
                    ? Guid.NewGuid().ToString("N")
                    : task.Identifier;

                int existingIndex = _tasks.FindIndex(existing => string.Equals(existing.Identifier, task.Identifier, StringComparison.Ordinal));
                if (existingIndex >= 0)
                {
                    _tasks[existingIndex] = task;
                }
                else
                {
                    _tasks.Add(task);
                }

                return task;
            }
        }

        private static bool Matches(DomainTask task, TaskCriteria criteria)
        {
            return HasAnyCriterion(criteria)
                && Matches(criteria.Identifier, task.Identifier)
                && Matches(criteria.TaskType, task.TaskType)
                && Matches(criteria.SubjectReference, task.SubjectReference)
                && Matches(criteria.Status, task.Status);
        }

        private static bool HasAnyCriterion(TaskCriteria criteria)
        {
            return !string.IsNullOrWhiteSpace(criteria.Identifier)
                || !string.IsNullOrWhiteSpace(criteria.TaskType)
                || !string.IsNullOrWhiteSpace(criteria.SubjectReference)
                || !string.IsNullOrWhiteSpace(criteria.Status);
        }

        private static bool Matches(string? criterion, string? value)
        {
            return string.IsNullOrWhiteSpace(criterion)
                || string.Equals(criterion, value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
