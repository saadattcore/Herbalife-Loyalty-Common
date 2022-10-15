using HL.Loyalty.Common;
using HL.Loyalty.Models;
using HL.Loyalty.RulesEngine.Interfaces;
using HL.Loyalty.RulesEngine.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HL.Loyalty.RulesEngine.Processors
{
    public class TierRulesProcessorV01 : ITierRulesProcessor
    {
        private ITierConfigurationProvider _tierConfigurationProvider;
        private IList<TierConfiguration> _tiersConfiguration;
        private int _maxMonthsCalculation;

        public TierRulesProcessorV01(ITierConfigurationProvider tierConfigurationProvider)
        {
            if (tierConfigurationProvider == null)
            {
                throw new ArgumentException("Tiers Configuration Provider can't be null", nameof(tierConfigurationProvider));
            }

            _tierConfigurationProvider = tierConfigurationProvider;
        }

        public ProcessTierRulesResponse Process(ProcessTierRulesRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request can't be null", nameof(request));
            }

            if (request.Orders == null)
            {
                throw new ArgumentException("Orders can't be null", nameof(request.Orders));
            }

            if (string.IsNullOrWhiteSpace(request.CountryCode))
            {
                throw new ArgumentException("CountryCode can't be null", nameof(request.CountryCode));
            }

            var currentGoals = 0;
            var tierIndex = 0;
            var currentPoints = 0f;

            var response = new ProcessTierRulesResponse
            {
                Status = LoyaltyProcessStatus.InProgress
            };

            if (request.Orders.Any())
            {
                var tiersConfiguration = _tierConfigurationProvider.Load(PointsCategoryType.Product, request.CountryCode);
                if (tiersConfiguration == null || !tiersConfiguration.Any())
                {
                    throw new ArgumentException("Tiers Configuration can't be null", nameof(tiersConfiguration));
                }

                foreach (var configuration in tiersConfiguration)
                {
                    if (configuration == null)
                    {
                        throw new ArgumentException("Tiers Configuration can't be null", nameof(configuration));
                    }

                    if (configuration.Tier == null)
                    {
                        throw new ArgumentException("Tiers Configuration can't be null", nameof(configuration.Tier));
                    }

                    _maxMonthsCalculation += configuration.AchievementsGoal;
                }

                _tiersConfiguration = tiersConfiguration.OrderBy(tc => tc.Tier.Level).ToList();

                try
                {
                    for (var i = _maxMonthsCalculation; tierIndex < _tiersConfiguration.Count && i >= 0; i--)
                    {
                        var minInvoiceDate = DateTime.Now.AddMonths(i * -1);
                        for (var j = 0; j < request.Orders.Count; j++)
                        {
                            if (request.Orders[j].InvoiceDate.Month == minInvoiceDate.Month && request.Orders[j].InvoiceDate.Year == minInvoiceDate.Year && request.Orders[j].CountryCode == request.CountryCode)
                            {
                                currentPoints += request.Orders[j].Points;
                                request.Orders.Remove(request.Orders[j]);
                                j--;
                            }
                        }

                        if (currentPoints >= _tiersConfiguration[tierIndex].GoalPoints)
                        {
                            currentGoals++;
                            if (currentGoals >= _tiersConfiguration[tierIndex].AchievementsGoal)
                            {
                                response.Level = _tiersConfiguration[tierIndex].Tier.Level;
                                tierIndex++;
                                currentGoals = 0;
                            }
                        }
                        else
                        {
                            if (DateTime.Now.Month == minInvoiceDate.Month && DateTime.Now.Year == minInvoiceDate.Year)
                            {
                                continue;
                            }

                            response.Level = tierIndex = currentGoals = 0;
                        }

                        response.LevelPoints = currentPoints;
                        currentPoints = 0;
                    }

                    response.Status = LoyaltyProcessStatus.Success;
                }
                catch
                {
                    response.Level = 0;
                    response.Status = LoyaltyProcessStatus.Failed;
                }
            }
            else
            {
                response.Status = LoyaltyProcessStatus.Success;
            }

            return response;
        }
    }
}
