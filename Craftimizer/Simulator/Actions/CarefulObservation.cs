namespace Craftimizer.Simulator.Actions;

internal class CarefulObservation : BaseAction
{
    public override ActionCategory Category => ActionCategory.Other;
    public override int Level => 55;
    public override uint ActionId => 100395;

    public override int CPCost => 0;
    public override int DurabilityCost => 0;
    public override bool IncreasesStepCount => false;

    public override bool CanUse => Simulation.Stats.IsSpecialist && Simulation.CountPreviousAction(ActionType.CarefulObservation) < 3;

    public override void UseSuccess() =>
        Simulation.StepCondition();
}