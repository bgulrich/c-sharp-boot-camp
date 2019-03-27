using System;
using System.Linq;
using Xunit;

namespace AttributeTests
{
    public class AttributesShould
    {
        #region Helpers
        [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
        internal class DescriptionAttribute : Attribute
        {
            public string Description { get; private set; }

            public DescriptionAttribute(string description)
            {
                Description = description;
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true,  Inherited = false)]
        internal class TagAttribute : Attribute
        {
            public string Tag { get; private set; }

            public TagAttribute(string tag)
            {
                Tag = tag;
            }
        }

        [Description("Spicy and delicious")]
        [Tag("Meat")]
        [Tag("Cured")]
        [Tag("Spicy")]
        internal class Pepperoni { }

        [Description("Let me tell you about a fun-gi")]
        [Tag("Vegetarian")]
        internal struct Mushroom { }
        #endregion

        [Fact]
        public void BeDiscoverableByReflectingOnType()
        {
            var attributes = Attribute.GetCustomAttributes(typeof(Pepperoni));

            Assert.Contains(attributes, a => a is DescriptionAttribute da && da.Description.Contains("delicious"));
            Assert.Contains(attributes, a => a is TagAttribute ta && ta.Tag == "Meat");
            Assert.Contains(attributes, a => a is TagAttribute ta && ta.Tag == "Cured");
            Assert.Contains(attributes, a => a is TagAttribute ta && ta.Tag == "Spicy");
        }

        [Fact]
        public void BeDiscoverableInAssemblyTypes()
        {
            var assembly = this.GetType().Assembly;

            var types = assembly.GetTypes();

            var taggedTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(TagAttribute), false).Any());

            Assert.Equal(2, taggedTypes.Count());
            Assert.Contains(taggedTypes, t => t == typeof(Pepperoni));
            Assert.Contains(taggedTypes, t => t == typeof(Mushroom));
        }
    }
}
