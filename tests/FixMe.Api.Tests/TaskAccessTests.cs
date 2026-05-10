using FixMe.Access.Task.Interface;
using FixMe.Access.Task.Service;
using DomainTask = FixMe.Access.Task.Interface.Task;

namespace FixMe.Api.Tests;

public sealed class TaskAccessTests
{
    [Fact]
    public async System.Threading.Tasks.Task FilterReturnsMatchingTask()
    {
        TaskAccess access = new(new TaskResource());
        DomainTask stored = await access.Store(new DomainTask
        {
            TaskType = "maintenance_slots_proposal_confirmation",
            SubjectReference = "proposal-123",
            RoutingTarget = "provider-456",
            Status = "open"
        });

        DomainTask? bySubject = await access.Filter(new TaskCriteria { SubjectReference = "proposal-123" });
        DomainTask? byType = await access.Filter(new TaskCriteria { TaskType = "maintenance_slots_proposal_confirmation" });
        DomainTask? byStatus = await access.Filter(new TaskCriteria { Status = "open" });
        DomainTask? byIdentifier = await access.Filter(new TaskCriteria { Identifier = stored.Identifier });

        Assert.Same(stored, bySubject);
        Assert.Same(stored, byType);
        Assert.Same(stored, byStatus);
        Assert.Same(stored, byIdentifier);
    }

    [Fact]
    public async System.Threading.Tasks.Task FilterReturnsNullWhenNoTaskMatches()
    {
        TaskAccess access = new(new TaskResource());
        await access.Store(new DomainTask
        {
            TaskType = "equipment_registration_review",
            SubjectReference = "registration-123",
            RoutingTarget = "backoffice",
            Status = "open"
        });

        DomainTask? result = await access.Filter(new TaskCriteria { SubjectReference = "proposal-123" });

        Assert.Null(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task StorePersistsAndReturnsTaskState()
    {
        TaskAccess access = new(new TaskResource());
        DomainTask task = new()
        {
            TaskType = "maintenance_plan_review",
            SubjectReference = "plan-123",
            RoutingTarget = "backoffice",
            Status = "open"
        };

        DomainTask stored = await access.Store(task);
        DomainTask? loaded = await access.Filter(new TaskCriteria { Identifier = stored.Identifier });

        Assert.False(string.IsNullOrWhiteSpace(stored.Identifier));
        Assert.Same(stored, loaded);
    }

    [Theory]
    [InlineData(null, "subject", "route", "open")]
    [InlineData("", "subject", "route", "open")]
    [InlineData("type", null, "route", "open")]
    [InlineData("type", "", "route", "open")]
    [InlineData("type", "subject", null, "open")]
    [InlineData("type", "subject", "", "open")]
    [InlineData("type", "subject", "route", null)]
    [InlineData("type", "subject", "route", "invalid")]
    public async System.Threading.Tasks.Task StoreRejectsInvalidTaskWithoutPersistingPartialTask(
        string? taskType,
        string? subjectReference,
        string? routingTarget,
        string? status)
    {
        TaskAccess access = new(new TaskResource());

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(new DomainTask
        {
            TaskType = taskType,
            SubjectReference = subjectReference,
            RoutingTarget = routingTarget,
            Status = status
        }));

        DomainTask? result = await access.Filter(new TaskCriteria { TaskType = taskType });
        Assert.Null(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task StoreRejectsClosedTaskUpdateWithoutPersistingPartialTask()
    {
        TaskAccess access = new(new TaskResource());
        DomainTask openTask = await access.Store(new DomainTask
        {
            TaskType = "maintenance_slots_proposal_confirmation",
            SubjectReference = "proposal-123",
            RoutingTarget = "provider-456",
            Status = "open"
        });

        DomainTask completedTask = await access.Store(new DomainTask
        {
            Identifier = openTask.Identifier,
            TaskType = openTask.TaskType,
            SubjectReference = openTask.SubjectReference,
            RoutingTarget = openTask.RoutingTarget,
            Status = "completed"
        });

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(new DomainTask
        {
            Identifier = openTask.Identifier,
            TaskType = openTask.TaskType,
            SubjectReference = openTask.SubjectReference,
            RoutingTarget = openTask.RoutingTarget,
            Status = "open"
        }));

        DomainTask? loaded = await access.Filter(new TaskCriteria { Identifier = openTask.Identifier });
        Assert.Same(completedTask, loaded);
        Assert.Equal("completed", loaded?.Status);
    }
}
