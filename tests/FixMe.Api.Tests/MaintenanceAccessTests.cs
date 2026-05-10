using FixMe.Access.Maintenance.Interface;
using FixMe.Access.Maintenance.Service;

namespace FixMe.Api.Tests;

public sealed class MaintenanceAccessTests
{
    [Fact]
    public async Task MaintenancePlanOfferingFilterReturnsMatchingOfferingAndNoResultWhenMissing()
    {
        MaintenanceAccess access = new(new MaintenanceResource([
            new MaintenancePlanOffering
            {
                OfferingId = "offering-1",
                EquipmentType = "heat-pump",
                Market = "north",
                IsAvailable = true,
                Price = 129.99m
            }
        ]));

        MaintenancePlanOffering? match = await access.MaintenancePlanOfferingFilter(new MaintenancePlanOfferingCriteria
        {
            EquipmentType = "heat-pump",
            Market = "north",
            IsAvailable = true
        });
        MaintenancePlanOffering? missing = await access.MaintenancePlanOfferingFilter(new MaintenancePlanOfferingCriteria
        {
            OfferingId = "missing"
        });

        Assert.Equal("offering-1", match?.OfferingId);
        Assert.Null(missing);
    }

    [Fact]
    public async Task MaintenancePlanStorePersistsValidPlanAndRejectsPartialPlan()
    {
        MaintenanceAccess access = NewAccess();
        MaintenancePlan plan = new()
        {
            PlanId = "plan-1",
            EquipmentId = "equipment-1",
            OfferingId = "offering-1",
            CustomerId = "customer-1",
            Status = "pending",
            LockedPrice = 129.99m
        };

        MaintenancePlan stored = await access.MaintenancePlanStore(plan);
        MaintenancePlan? match = await access.MaintenancePlanFilter(new MaintenancePlanCriteria
        {
            EquipmentId = "equipment-1",
            Status = "pending"
        });
        await Assert.ThrowsAsync<ArgumentException>(() => access.MaintenancePlanStore(new MaintenancePlan
        {
            PlanId = "plan-partial",
            EquipmentId = "equipment-1",
            OfferingId = "offering-1",
            CustomerId = "customer-1",
            Status = "active"
        }));
        MaintenancePlan? rejected = await access.MaintenancePlanFilter(new MaintenancePlanCriteria
        {
            PlanId = "plan-partial"
        });

        Assert.Same(plan, stored);
        Assert.Equal("plan-1", match?.PlanId);
        Assert.Null(rejected);
    }

    [Fact]
    public async Task MaintenanceJobSlotStorePersistsValidSlotAndRejectsPartialSlot()
    {
        MaintenanceAccess access = NewAccess();
        DateTimeOffset start = new(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);
        MaintenanceJobSlot slot = new()
        {
            SlotId = "slot-1",
            ProviderId = "provider-1",
            Status = "reserved",
            ScheduledStart = start,
            ScheduledEnd = start.AddHours(2),
            ReservationId = "reservation-1",
            ProposalId = "proposal-1"
        };

        MaintenanceJobSlot stored = await access.MaintenanceJobSlotStore(slot);
        MaintenanceJobSlot? match = await access.MaintenanceJobSlotFilter(new MaintenanceJobSlotCriteria
        {
            ProviderId = "provider-1",
            Status = "reserved",
            StartsOnOrAfter = start.AddMinutes(-1),
            StartsBefore = start.AddMinutes(1)
        });
        await Assert.ThrowsAsync<ArgumentException>(() => access.MaintenanceJobSlotStore(new MaintenanceJobSlot
        {
            SlotId = "slot-partial",
            ProviderId = "provider-1",
            Status = "reserved",
            ScheduledStart = start,
            ScheduledEnd = start.AddHours(2)
        }));
        MaintenanceJobSlot? rejected = await access.MaintenanceJobSlotFilter(new MaintenanceJobSlotCriteria
        {
            SlotId = "slot-partial"
        });

        Assert.Same(slot, stored);
        Assert.Equal("slot-1", match?.SlotId);
        Assert.Null(rejected);
    }

    [Fact]
    public async Task MaintenanceJobSlotsProposalStorePersistsValidProposalAndRejectsPartialProposal()
    {
        MaintenanceAccess access = NewAccess();
        MaintenanceJobSlotsProposal proposal = new()
        {
            ProposalId = "proposal-1",
            MaintenancePlanId = "plan-1",
            CustomerId = "customer-1",
            Status = "confirmed",
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(24),
            ConfirmedAt = DateTimeOffset.UtcNow,
            SlotIds = ["slot-1", "slot-2", "slot-3"]
        };

        MaintenanceJobSlotsProposal stored = await access.MaintenanceJobSlotsProposalStore(proposal);
        MaintenanceJobSlotsProposal? match = await access.MaintenanceJobSlotsProposalFilter(new MaintenanceJobSlotsProposalCriteria
        {
            MaintenancePlanId = "plan-1",
            SlotId = "slot-2"
        });
        await Assert.ThrowsAsync<ArgumentException>(() => access.MaintenanceJobSlotsProposalStore(new MaintenanceJobSlotsProposal
        {
            ProposalId = "proposal-partial",
            MaintenancePlanId = "plan-1",
            CustomerId = "customer-1",
            Status = "created",
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(24)
        }));
        MaintenanceJobSlotsProposal? rejected = await access.MaintenanceJobSlotsProposalFilter(new MaintenanceJobSlotsProposalCriteria
        {
            ProposalId = "proposal-partial"
        });

        Assert.Same(proposal, stored);
        Assert.Equal("proposal-1", match?.ProposalId);
        Assert.Null(rejected);
    }

    [Fact]
    public async Task MaintenanceJobStorePersistsValidJobAndRejectsPartialJob()
    {
        MaintenanceAccess access = NewAccess();
        DateTimeOffset start = new(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);
        MaintenanceJob job = new()
        {
            JobId = "job-1",
            MaintenancePlanId = "plan-1",
            EquipmentId = "equipment-1",
            SelectedSlotId = "slot-1",
            Status = "scheduled",
            ScheduledStart = start
        };

        MaintenanceJob stored = await access.MaintenanceJobStore(job);
        MaintenanceJob? match = await access.MaintenanceJobFilter(new MaintenanceJobCriteria
        {
            EquipmentId = "equipment-1",
            Status = "scheduled"
        });
        await Assert.ThrowsAsync<ArgumentException>(() => access.MaintenanceJobStore(new MaintenanceJob
        {
            JobId = "job-partial",
            MaintenancePlanId = "plan-1",
            EquipmentId = "equipment-1",
            SelectedSlotId = "slot-1",
            Status = "cancelled",
            ScheduledStart = start
        }));
        MaintenanceJob? rejected = await access.MaintenanceJobFilter(new MaintenanceJobCriteria
        {
            JobId = "job-partial"
        });

        Assert.Same(job, stored);
        Assert.Equal("job-1", match?.JobId);
        Assert.Null(rejected);
    }

    private static MaintenanceAccess NewAccess() => new(new MaintenanceResource());
}
