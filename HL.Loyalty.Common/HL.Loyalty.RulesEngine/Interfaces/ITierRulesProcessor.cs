using HL.Loyalty.RulesEngine.ValueObjects;

namespace HL.Loyalty.RulesEngine
{
    public interface ITierRulesProcessor
    {
        ProcessTierRulesResponse Process(ProcessTierRulesRequest request);
    }
}
