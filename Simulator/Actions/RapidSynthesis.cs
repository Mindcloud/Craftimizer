namespace Craftimizer.Simulator.Actions;

internal sealed class RapidSynthesis : BaseAction
{
    public override ActionCategory Category => ActionCategory.Synthesis;
    public override int Level => 9;
    public override uint ActionId => 100363;

    public override int CPCost => 0;
    // Rapid Synthesis Mastery Trait
    public override float Efficiency => Simulation.Input.Stats.Level >= 63 ? 5.00f : 2.50f;
    public override bool IncreasesProgress => true;
    public override float SuccessRate => 0.50f;
}