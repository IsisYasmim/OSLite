using Xunit;
using System;
using OSLite.Domain;

namespace OSLite.Domain.Tests
{
    public class MoneyTests
    {
        [Fact]
        public void Money_nao_aceita_negativo()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Money(-1m));
        }
    }
}