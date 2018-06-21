﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Autofac.Configuration.Test.Core
{
    public class ConfigurationExtensions_EnumerableParametersFixture
    {
        public class A
        {
            public IList<string> List { get; set; }
        }

        [Fact]
        public void PropertyStringListInjection()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<A>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal("Val1", poco.List[0]);
            Assert.Equal("Val2", poco.List[1]);
        }

        public class B
        {
            public IList<int> List { get; set; }
        }

        [Fact]
        public void ConvertsTypeInList()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<B>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal(1, poco.List[0]);
            Assert.Equal(2, poco.List[1]);
        }

        public class C
        {
            public IList List { get; set; }
        }

        [Fact]
        public void FillsNonGenericListWithString()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<C>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal("1", poco.List[0]);
            Assert.Equal("2", poco.List[1]);
        }

        public class D
        {
            public int Num { get; set; }
        }

        [Fact]
        public void InjectsSingleValueWithConversion()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<D>();

            Assert.True(poco.Num == 123);
        }

        public class E
        {
            public IList<int> List { get; set; }

            public E(IList<int> list)
            {
                List = list;
            }
        }

        [Fact]
        public void InjectsConstructorParameter()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<E>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal(1, poco.List[0]);
            Assert.Equal(2, poco.List[1]);
        }

        public class G
        {
            public IEnumerable Enumerable { get; set; }
        }

        [Fact]
        public void InjectsIEnumerable()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<G>();

            Assert.NotNull(poco.Enumerable);
            var enumerable = poco.Enumerable.Cast<string>().ToList();
            Assert.True(enumerable.Count == 2);
            Assert.Equal("Val1", enumerable[0]);
            Assert.Equal("Val2", enumerable[1]);
        }

        public class H
        {
            public IEnumerable<int> Enumerable { get; set; }
        }

        [Fact]
        public void InjectsGenericIEnumerable()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<H>();

            Assert.NotNull(poco.Enumerable);
            var enumerable = poco.Enumerable.ToList();
            Assert.True(enumerable.Count == 2);
            Assert.Equal(1, enumerable[0]);
            Assert.Equal(2, enumerable[1]);
        }

        public class I
        {
            public ICollection<int> Collection { get; set; }
        }

        [Fact]
        public void InjectsGenericCollection()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<I>();

            Assert.NotNull(poco.Collection);
            Assert.True(poco.Collection.Count == 2);
            Assert.Equal(1, poco.Collection.First());
            Assert.Equal(2, poco.Collection.Last());
        }

        public class J
        {
            public J(IList<string> list)
            {
                this.List = list;
            }

            public IList<string> List { get; private set; }
        }

        [Fact]
        public void ParameterStringListInjection()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<J>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal("Val1", poco.List[0]);
            Assert.Equal("Val2", poco.List[1]);
        }

        public class K
        {
            public K(IList<string> list = null)
            {
                this.List = list;
            }

            public IList<string> List { get; private set; }
        }

        [Fact]
        public void ParameterStringListInjectionOptionalParameter()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<K>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal("Val1", poco.List[0]);
            Assert.Equal("Val2", poco.List[1]);
        }

        public class L
        {
            public L()
            {
                this.List = new List<string>();
            }

            public L(IList<string> list = null)
            {
                this.List = list;
            }

            public IList<string> List { get; private set; }
        }

        [Fact]
        public void ParameterStringListInjectionMultipleConstructors()
        {
            var container = EmbeddedConfiguration.ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<L>();

            Assert.True(poco.List.Count == 2);
            Assert.Equal("Val1", poco.List[0]);
            Assert.Equal("Val2", poco.List[1]);
        }

        public class M
        {
            public M(IList<string> list) => this.List = list;

            public IList<string> List { get; }
        }

        /// <summary>
        /// A characterization test, not intended to express desired behaviour, but to capture the current behaviour.
        /// </summary>
        [Fact]
        public void ParameterStringListInjectionSecondElementHasNoName()
        {
            var container = EmbeddedConfiguration
                .ConfigureContainerWithXml("ConfigurationExtensions_EnumerableParameters.xml").Build();

            var poco = container.Resolve<M>();

            // Val2 is dropped from the configuration when it's parsed.
            Assert.Collection(poco.List, v => Assert.Equal("Val1", v));
        }
    }
}
