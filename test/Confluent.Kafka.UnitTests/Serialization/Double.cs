// Copyright 2016-2017 Confluent Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Refer to LICENSE for more information.

using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using Xunit;


namespace Confluent.Kafka.UnitTests.Serialization
{
    public class DoubleTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void CanReconstruct(double value)
        {
            Assert.Equal(value, new DoubleDeserializer().Deserialize(null, new DoubleSerializer().Serialize(null, value)));
        }

        [Fact]
        public void IsBigEndian()
        {
            var buffer = new byte[] { 23, 0, 0, 0, 0, 0, 0, 0 };
            var value = BitConverter.ToDouble(buffer, 0);
            var data = new DoubleSerializer().Serialize(null, value);
            Assert.Equal(23, data[7]);
            Assert.Equal(0, data[0]);
        }

        [Fact]
        public void DeserializeArgNullThrow()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => new DoubleDeserializer().Deserialize(null, null));
        }

        [Fact]
        public void DeserializeArgLengthNotEqual8Throw()
        {
            Assert.ThrowsAny<ArgumentException>(() => new DoubleDeserializer().Deserialize(null, new byte[0]));
            Assert.ThrowsAny<ArgumentException>(() => new DoubleDeserializer().Deserialize(null, new byte[7]));
            Assert.ThrowsAny<ArgumentException>(() => new DoubleDeserializer().Deserialize(null, new byte[9]));
        }

        public static IEnumerable<object[]> TestData()
        {
            double[] testData = new double[]
            {
                0, 1, -1, 42, -42, 127, 128, 129, -127, -128,
                -129,254, 255, 256, 257, -254, -255, -256, -257,
                short.MinValue-1, short.MinValue, short.MinValue+1,
                short.MaxValue-1, short.MaxValue,short.MaxValue+1,
                int.MaxValue-1, int.MaxValue, int.MinValue, int.MinValue + 1,
                double.MaxValue-1,double.MaxValue,double.MinValue,double.MinValue+1,
                double.NaN,double.PositiveInfinity,double.NegativeInfinity,double.Epsilon,-double.Epsilon
            };

            foreach (var v in testData)
            {
                yield return new object[] { v };
            }
        }
    }
}
