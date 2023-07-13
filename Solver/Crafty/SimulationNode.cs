using Craftimizer.Simulator;
using Craftimizer.Simulator.Actions;
using System.Runtime.InteropServices;

namespace Craftimizer.Solver.Crafty;

[StructLayout(LayoutKind.Auto)]
public struct SimulationNode
{
    public readonly SimulationState State;
    public readonly ActionType? Action;
    public readonly CompletionState SimulationCompletionState;

    public ActionSet AvailableActions;

    public readonly CompletionState CompletionState => GetCompletionState(SimulationCompletionState, AvailableActions);

    public readonly bool IsComplete => CompletionState != CompletionState.Incomplete;

    public SimulationNode(SimulationState state, ActionType? action, CompletionState completionState, ActionSet actions)
    {
        State = state;
        Action = action;
        SimulationCompletionState = completionState;
        AvailableActions = actions;
    }

    public static CompletionState GetCompletionState(CompletionState simCompletionState, ActionSet actions) =>
        actions.IsEmpty && simCompletionState == CompletionState.Incomplete ?
        CompletionState.NoMoreActions :
        simCompletionState;

    public readonly float? CalculateScore(SolverConfig config) =>
        CalculateScoreForState(State, SimulationCompletionState, config);

    private static bool CanByregot(SimulationState state)
    {
        if (state.ActiveEffects.InnerQuiet == 0)
            return false;

        return BaseAction.VerifyDurability2(state, 10);
    }

    public static float? CalculateScoreForState(SimulationState state, CompletionState completionState, SolverConfig config)
    {
        if (completionState != CompletionState.ProgressComplete)
            return null;

        static float Apply(float bonus, float value, float target) =>
            bonus * Math.Min(1f, value / target);

        var progressScore = Apply(
            config.ScoreProgressBonus,
            state.Progress,
            state.Input.Recipe.MaxProgress
        );

        var byregotBonus = CanByregot(state) ? (state.ActiveEffects.InnerQuiet * .2f + 1) * state.Input.BaseQualityGain : 0;
        var qualityScore = Apply(
            config.ScoreQualityBonus,
            state.Quality + byregotBonus,
            state.Input.Recipe.MaxQuality
        );

        var durabilityScore = Apply(
            config.ScoreDurabilityBonus,
            state.Durability,
            state.Input.Recipe.MaxDurability
        );

        var cpScore = Apply(
            config.ScoreCPBonus,
            state.CP,
            state.Input.Stats.CP
        );

        var fewerStepsScore =
            config.ScoreFewerStepsBonus * (1f - ((float)(state.ActionCount + 1) / config.MaxStepCount));

        return progressScore + qualityScore + durabilityScore + cpScore + fewerStepsScore;
    }
}
