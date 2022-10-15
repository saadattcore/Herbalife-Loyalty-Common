using HL.Loyalty.Common;
using HL.Loyalty.Models;
using HL.Loyalty.RulesEngine.Tests.Providers;
using HL.Loyalty.RulesEngine.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HL.Loyalty.RulesEngine.Processors.Tests
{
    [TestClass()]
    public class TierRulesProcessorV01Tests
    {
        private ITierRulesProcessor _processor;

        private List<TierConfiguration> ConfigurationWithOneTiersConfiguration()
        {
            return new List<TierConfiguration>
            {
                new TierConfiguration
                {
                    PointsType = PointsCategoryType.Product,
                    CountryCode = "US",
                    GoalPoints = 100,
                    Tier = new Tier
                    {
                        Id = Guid.NewGuid(),
                        Level = 1
                    },
                    ValidFrom = DateTime.Now,
                    ValidTo =  DateTime.Now.AddDays(30),
                    AchievementsGoal = 2
                }
            };
        }

        private List<TierConfiguration> ConfigurationWithTwoTiersConfiguration()
        {
            return new List<TierConfiguration>
            {
                new TierConfiguration
                {
                    PointsType = PointsCategoryType.Product,
                    CountryCode = "US",
                    GoalPoints = 100,
                    Tier = new Tier
                    {
                        Id = Guid.NewGuid(),
                        Level = 1
                    },
                    ValidFrom = DateTime.Now,
                    ValidTo =  DateTime.Now.AddDays(30),
                    AchievementsGoal = 2
                },
                new TierConfiguration
                {
                    PointsType = PointsCategoryType.Product,
                    CountryCode = "US",
                    GoalPoints = 100,
                    Tier = new Tier
                    {
                        Id = Guid.NewGuid(),
                        Level = 2
                    },
                    ValidFrom = DateTime.Now,
                    ValidTo =  DateTime.Now.AddDays(30),
                    AchievementsGoal = 1
                }
            };
        }

        private List<TierConfiguration> ConfigurationWithTreeTiersConfiguration()
        {
            return new List<TierConfiguration>
            {
                new TierConfiguration
                {
                    PointsType = PointsCategoryType.Product,
                    CountryCode = "US",
                    GoalPoints = 100,
                    Tier = new Tier
                    {
                        Id = Guid.NewGuid(),
                        Level = 1
                    },
                    ValidFrom = DateTime.Now,
                    ValidTo =  DateTime.Now.AddDays(30),
                    AchievementsGoal = 2
                },
                new TierConfiguration
                {
                    PointsType = PointsCategoryType.Product,
                    CountryCode = "US",
                    GoalPoints = 100,
                    Tier = new Tier
                    {
                        Id = Guid.NewGuid(),
                        Level = 2
                    },
                    ValidFrom = DateTime.Now,
                    ValidTo =  DateTime.Now.AddDays(30),
                    AchievementsGoal = 1
                },
                new TierConfiguration
                {
                    PointsType = PointsCategoryType.Product,
                    CountryCode = "US",
                    GoalPoints = 100,
                    Tier = new Tier
                    {
                        Id = Guid.NewGuid(),
                        Level = 3
                    },
                    ValidFrom = DateTime.Now,
                    ValidTo =  DateTime.Now.AddDays(30),
                    AchievementsGoal = 1
                },
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            var provider = new MockupTierConfigurationProvider(ConfigurationWithOneTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NullTierConfiguration_ArgumentException()
        {
            var processor = new TierRulesProcessorV01(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Process_EmptyTierConfiguration_ArgumentException()
        {
            var provider = new MockupTierConfigurationProvider(new List<TierConfiguration>());
            var processor = new TierRulesProcessorV01(provider);
            processor.Process(new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order()
                },
                CountryCode = "US"
            });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Process_InvalidTierConfiguration_ArgumentException()
        {
            var provider = new MockupTierConfigurationProvider(new List<TierConfiguration> {
                null,
                null
            });
            var processor = new TierRulesProcessorV01(provider);
            processor.Process(new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order()
                },
                CountryCode = "US"
            });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Process_InvalidTierConfiguration2_ArgumentException()
        {
            var provider = new MockupTierConfigurationProvider(new List<TierConfiguration> {
                new TierConfiguration {
                    Tier = null
                }
            });
            var processor = new TierRulesProcessorV01(provider);
            processor.Process(new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order()
                },
                CountryCode = "US"
            });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Process_NullRequest_ArgumentException()
        {
            var response = _processor.Process(null);
            Assert.IsNull(response);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Process_NullOrders_ArgumentException()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = null
            };

            var response = _processor.Process(request);
            Assert.IsNull(response);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Process_NullCountryCode_ArgumentException()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>(),
                CountryCode = null
            };

            var response = _processor.Process(request);
            Assert.IsNull(response);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_EmptyOrders_LevelZero()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>(),
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_OneAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                        CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 40,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                        CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_TwoAchievement_Level1()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 40,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 1);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_NoAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 10,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                        CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.LevelPoints == 10);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_OneAchievement_NoAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 40,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_NoAchievement_OneAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 190,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_OneTierConfiguration_NoAchievement_TwoAchievements_Level1()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 130,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 190,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 1);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_TwoTiersConfiguration_TwoAchievements_NoAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 40,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 0,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTwoTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_TwoTiersConfiguration_TwoAchievements_NoAchievementCurrentMonth_Level1()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 90,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 40,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 0,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTwoTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 1);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_TwoTiersConfiguration_TwoAchievement_NoAchievement_OneAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 10,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 1210,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 1,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 105,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 196,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTwoTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_TwoTiersConfiguration_OneAchievement_NoAchievement_TwoAchievements_Level1()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 10,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 100,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 5,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 96,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };
            var provider = new MockupTierConfigurationProvider(ConfigurationWithTwoTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 1);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_TwoTiersConfiguration_ThreeAchievements_NoAchievementInCurrentMonth_Level2()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 102,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 50,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 60,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };
            var provider = new MockupTierConfigurationProvider(ConfigurationWithTwoTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 2);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_TwoTiersConfiguration_ThreeAchievements_AchievementInCurrentMonth_Level2()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 102,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 50,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 60,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };
            var provider = new MockupTierConfigurationProvider(ConfigurationWithTwoTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 2);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_ThreeAchievements_NoAcheivement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 102,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 50,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };
            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_TwoAchievements_NoAcheivement_OneAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 20,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 502,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_TwoAchievements_NoAcheivement_TwoAchievements_AchievementInCurrentMonth_Level1()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 20,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 502,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 222,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 1);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_OneAchievement_TwoNoAcheivements_TwoAchievements_AchievementInCurrentMonth_Level1()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 20,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 502,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 222,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 1);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_NoAchievement_TwoAcheivements_TwoNoAchievementIncludingCurrentMonth_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_NoAchievement_ThreeAcheivements_NoAchievementInCurrentMonth_Level2()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 100,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 2);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_TwoNoAchievements_ThreeAcheivementsIncludingCurrentMonth_Level2()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 100,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 2);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_NoAchievement_FourAcheivementsIncludingCurrentMonth_Level3()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 100,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 3);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_NoAchievement_FourAcheivements_NoAchievementInCurrentMonth_Level3()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 3);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_FourAcheivements_NoAchievement_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-5),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 120,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_FiveAcheivements_IncludingCurrentMonth_Level3()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-4),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-3),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 2,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 98,
                        InvoiceDate = DateTime.Now.AddMonths(-2),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now.AddMonths(-1),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 125,
                        InvoiceDate = DateTime.Now,
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 3);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }

        [TestMethod()]
        public void Process_ThreeTiersConfiguration_NoOrdersInPeriod_Level0()
        {
            var request = new ProcessTierRulesRequest
            {
                Orders = new List<Order>
                {
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-14),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 110,
                        InvoiceDate = DateTime.Now.AddMonths(-13),
                CountryCode = "US"
                    },
                    new Order
                    {
                        Points = 2,
                        InvoiceDate = DateTime.Now.AddMonths(-12),
                CountryCode = "US"
                    }
                },
                CountryCode = "US"
            };

            var provider = new MockupTierConfigurationProvider(ConfigurationWithTreeTiersConfiguration());
            _processor = new TierRulesProcessorV01(provider);
            var response = _processor.Process(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Level == 0);
            Assert.IsTrue(response.Status == LoyaltyProcessStatus.Success);
        }
    }
}