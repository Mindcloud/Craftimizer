namespace Craftimizer.Simulator.Actions;

internal sealed class ByregotsBlessing : BaseAction
{
    public override ActionCategory Category => ActionCategory.Quality;
    public override int Level => 50;
    public override uint ActionId => 100339;

    public override bool IncreasesQuality => true;

    public override int CPCost(Simulator s) => 24;
    public override float Efficiency(Simulator s) => 1.00f + (0.20f * s.GetEffectStrength(EffectType.InnerQuiet));

    public override bool CanUse(Simulator s) => s.HasEffect(EffectType.InnerQuiet) && base.CanUse(s);

    public override void UseSuccess(Simulator s)
    {
        base.UseSuccess(s);
        s.RemoveEffect(EffectType.InnerQuiet);
    }
}
