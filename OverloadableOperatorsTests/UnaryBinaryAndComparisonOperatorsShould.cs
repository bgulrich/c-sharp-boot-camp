using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OverloadableOperatorsTests
{
    public class UnaryBinaryAndComparisonOperatorsShould
    {
        #region Helpers

        private class Meal : ICollection<MealComponent>
        {
            private readonly List<MealComponent> _items = new List<MealComponent>();

            public NutritionInfo NutritionInfo { get; private set; }

            public void Add(MealComponent component)
            {
                _items.Add(component);
                NutritionInfo += component;
            }

            public bool Remove(MealComponent component)
            {
                var result = _items.Remove(component);

                if (result)
                    NutritionInfo -= component;

                return result;
            }

            public void Clear()
            {
                _items.Clear();
            }

            #region Other ICollection stuff
            public IEnumerator<MealComponent> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public bool Contains(MealComponent component) => _items.Contains(component);

            public void CopyTo(MealComponent[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
       

            public int Count => _items.Count;
            public bool IsReadOnly => false;
            #endregion
        }

        /// <summary>
        /// Can have meal items added to it to accumulate nutrition info
        /// </summary>
        private struct NutritionInfo
        {
            public uint Calories { get; private set; }
            public uint SodiumMilligrams { get; private set; }
            public uint ProteinGrams { get; private set; }
            public CarbohydrateInfo Carbohydrates { get; private set; }
            public FatInfo Fat { get; private set; }

            public static NutritionInfo operator +(NutritionInfo ni, MealComponent mc)
            {
                return new NutritionInfo
                {
                    Calories = ni.Calories + mc.Calories,
                    Carbohydrates = ni.Carbohydrates + mc.Carbohydrates,
                    Fat = ni.Fat + mc.Fat,
                    ProteinGrams = ni.ProteinGrams + mc.ProteinGrams,
                    SodiumMilligrams = ni.SodiumMilligrams + mc.SodiumMilligrams
                };
            }

            public static NutritionInfo operator -(NutritionInfo mc, MealComponent mi)
            {
                return new NutritionInfo
                {
                    Calories = mc.Calories - mi.Calories,
                    Carbohydrates = mc.Carbohydrates - mi.Carbohydrates,
                    Fat = mc.Fat - mi.Fat,
                    ProteinGrams = mc.ProteinGrams - mi.ProteinGrams,
                    SodiumMilligrams = mc.SodiumMilligrams - mi.SodiumMilligrams
                };
            }
        }

        private struct MealComponent
        {
            public string Name { get; set; }
            public uint Calories { get; set; }
            public uint SodiumMilligrams { get; set; }
            public uint ProteinGrams { get; set; }
            public CarbohydrateInfo Carbohydrates { get; set; }
            public FatInfo Fat { get; set; }
        }

        public struct CarbohydrateInfo
        {
            public uint TotalGrams { get; }
            public uint SugarGrams { get; }
            public uint FiberGrams { get; }

            public CarbohydrateInfo(uint total, uint sugar, uint fiber)
            {
                if (sugar + fiber > total)
                    throw new ArgumentException("Total can't be less than fiber and sugar!");

                TotalGrams = total;
                SugarGrams = sugar;
                FiberGrams = fiber;
            }

            public static CarbohydrateInfo operator +(CarbohydrateInfo left, CarbohydrateInfo right)
            {
                return new CarbohydrateInfo(left.TotalGrams + right.TotalGrams,
                                            left.SugarGrams + right.SugarGrams,
                                            left.FiberGrams + right.FiberGrams);
            }

            public static CarbohydrateInfo operator -(CarbohydrateInfo left, CarbohydrateInfo right)
            {
                return new CarbohydrateInfo(left.TotalGrams - right.TotalGrams,
                                            left.SugarGrams - right.SugarGrams,
                                            left.FiberGrams - right.FiberGrams);
            }
        }

        public struct FatInfo
        {
            public uint TotalGrams { get; }
            public uint SaturatedGrams { get; }

            public FatInfo(uint total, uint saturated)
            {
                if (saturated > total)
                    throw new ArgumentException("Saturated cannot be more than total!");

                TotalGrams = total;
                SaturatedGrams = saturated;
            }

            #region arithmetic
            public static FatInfo operator +(FatInfo left, FatInfo right)
            {
                return new FatInfo(left.TotalGrams + right.TotalGrams,
                                   left.SaturatedGrams + right.SaturatedGrams);
            }

            public static FatInfo operator -(FatInfo left, FatInfo right)
            {
                return new FatInfo(left.TotalGrams - right.TotalGrams,
                                   left.SaturatedGrams - right.SaturatedGrams);
            }
            #endregion

            #region comparison

            // just use total to compare
            public static bool operator >(FatInfo left, FatInfo right)
            {
                return left.TotalGrams > right.TotalGrams;
            }

            public static bool operator <(FatInfo left, FatInfo right)
            {
                return left.TotalGrams < right.TotalGrams;
            }

            public static bool operator >=(FatInfo left, FatInfo right)
            {
                return left.TotalGrams >= right.TotalGrams;
            }

            public static bool operator <=(FatInfo left, FatInfo right)
            {
                return left.TotalGrams <= right.TotalGrams;
            }
            #endregion

            #region unary
            // this may be a little silly but it illustrates how to override (false == "fat free")
            public static bool operator true(FatInfo info)
            {
                return info.TotalGrams > 0;
            }

            public static bool operator false(FatInfo info)
            {
                return info.TotalGrams == 0;
            }
            #endregion
        }

        #endregion

        [Fact]
        public void WorkAsExpected()
        {
            var cobbSalad = new Meal
            {
                new MealComponent
                {
                    Name = "Lettuce",
                    Carbohydrates = new CarbohydrateInfo(1, 0, 1),
                },

                new MealComponent
                {
                    Name = "Hard Boiled Egg",
                    Calories = 50,
                    ProteinGrams = 15,
                    Fat = new FatInfo(3, 2),
                },

                new MealComponent
                {
                    Name = "Bacon",
                    Calories = 150,
                    ProteinGrams = 10,
                    Fat = new FatInfo(12, 9),
                    SodiumMilligrams = 600
                },

                new MealComponent
                {
                    Name = "Tomato",
                    Calories = 10,
                    Carbohydrates = new CarbohydrateInfo(3, 1, 2)
                },

                new MealComponent
                {
                    Name = "Dressing",
                    Calories = 200,
                    Carbohydrates = new CarbohydrateInfo(1, 0, 0),
                    SodiumMilligrams = 100,
                    Fat = new FatInfo(20, 10)
                }
            };

            Assert.Equal(410u, cobbSalad.NutritionInfo.Calories);
            Assert.Equal(25u, cobbSalad.NutritionInfo.ProteinGrams);
            Assert.Equal(35u, cobbSalad.NutritionInfo.Fat.TotalGrams);
            Assert.Equal(21u, cobbSalad.NutritionInfo.Fat.SaturatedGrams);
            Assert.Equal(5u, cobbSalad.NutritionInfo.Carbohydrates.TotalGrams);
            Assert.Equal(1u, cobbSalad.NutritionInfo.Carbohydrates.SugarGrams);
            Assert.Equal(3u, cobbSalad.NutritionInfo.Carbohydrates.FiberGrams);
            Assert.Equal(700u, cobbSalad.NutritionInfo.SodiumMilligrams);

            // hold the egg
            cobbSalad.Remove(cobbSalad.First(i => i.Name == "Hard Boiled Egg"));

            Assert.Equal(360u, cobbSalad.NutritionInfo.Calories);
            Assert.Equal(10u, cobbSalad.NutritionInfo.ProteinGrams);
            Assert.Equal(32u, cobbSalad.NutritionInfo.Fat.TotalGrams);
            Assert.Equal(19u, cobbSalad.NutritionInfo.Fat.SaturatedGrams);
            Assert.Equal(5u, cobbSalad.NutritionInfo.Carbohydrates.TotalGrams);
            Assert.Equal(1u, cobbSalad.NutritionInfo.Carbohydrates.SugarGrams);
            Assert.Equal(3u, cobbSalad.NutritionInfo.Carbohydrates.FiberGrams);
            Assert.Equal(700u, cobbSalad.NutritionInfo.SodiumMilligrams);

            // let's make this fat free
            // we can while over fat as though it were a boolean
            while (cobbSalad.NutritionInfo.Fat)
            {
                // remove fatty components until we're fat free
                cobbSalad.Remove(cobbSalad.First(c => c.Fat.TotalGrams > 0));
            }

            // congratulations, you're eating lettuce and tomato
            Assert.Equal(2, cobbSalad.Count());
            Assert.Contains(cobbSalad, c => c.Name == "Lettuce");
            Assert.Contains(cobbSalad, c => c.Name == "Tomato");
        }
    }
}
