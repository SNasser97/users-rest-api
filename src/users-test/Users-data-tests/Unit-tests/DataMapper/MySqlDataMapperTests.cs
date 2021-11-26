namespace users_test.Users_data_tests.Unit_tests.DataMapper
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Reflection;
    using Moq;
    using users_data.Entities;
    using users_data.Repositories.MySQL.MySqlDataMapper;
    using users_data.Repositories.MySQL.MySqlDataMapper.Setters;
    using users_test.Helper;
    using Xunit;

    public class MySqlDataMapperTests
    {
        public Mock<IDataReader> MockDataReader { get; }

        public MySqlDataToTypeSetter MySqlDataToTypeSetter { get; }

        public MySqlDataMapper<User> MySqlUserDataMapper { get; }

        public MySqlDataMapper<AnyPOCO> MySqlAnyPocoDataMapper { get; }

        public MySqlDataMapper<UnregisteredLongPOCO> MySqlUnregisteredLongPocoDataMapper { get; }

        public static IEnumerable<object[]> InvalidColumnData => new List<object[]>
        {
            new object[]
            {
                new object[]
                {
                    Guid.NewGuid().ToByteArray(),
                    "Bob",
                    "Doe",
                    "bob.doe@mail.com",
                    DateTime.ParseExact("20111998", "ddMMyyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact("20111998", "ddMMyyyy", CultureInfo.InvariantCulture),
                }
            },
            new object[]
            {
               new object[]
               {
                    Guid.NewGuid().ToByteArray(),
                    Guid.NewGuid(),
                    "Doe",
                    "bob.doe@mail.com",
                    DateTime.ParseExact("20111998", "ddMMyyyy", CultureInfo.InvariantCulture),
                    23
               }
            },
            new object[]
            {
               new object[]
               {
                    Guid.NewGuid().ToByteArray(),
                    "Bob",
                    "Doe",
                    "bob.doe@mail.com",
                    DateTime.ParseExact("20111998", "ddMMyyyy", CultureInfo.InvariantCulture),
                    long.MaxValue
               }
            }
        };

        public MySqlDataMapperTests()
        {
            this.MockDataReader = new Mock<IDataReader>();
            this.MySqlDataToTypeSetter = new MySqlDataToTypeSetter();
            this.MySqlUserDataMapper = new MySqlDataMapper<User>(this.MySqlDataToTypeSetter);
            this.MySqlAnyPocoDataMapper = new MySqlDataMapper<AnyPOCO>(this.MySqlDataToTypeSetter);
            this.MySqlUnregisteredLongPocoDataMapper = new MySqlDataMapper<UnregisteredLongPOCO>(this.MySqlDataToTypeSetter);
        }

        [Theory]
        [MemberData(nameof(InvalidColumnData))]
        public void MysqlDataMapperTakesDataReaderAndMapsDataToUserEntityThrowsException(object[] expectedColumnData)
        {
            //Given
            Guid id = Guid.NewGuid();

            this.SetupMockDataReaderProperties<User>(expectedColumnData);

            // When
            // Then
            Exceptions<Exception>.Handle(() => this.MySqlUserDataMapper.MapDataToEntity(this.MockDataReader.Object),
                (ex) => Assert.True(ex.Message != string.Empty));
        }

        [Fact]
        public void MysqlDataMapperTakesDataReaderAndMapsDataToUserEntity()
        {
            //Given
            Guid id = Guid.NewGuid();

            var expectedColumnData = new object[]
            {
                id.ToByteArray(),
                "Bob",
                "Doe",
                "bob.doe@mail.com",
                DateTime.ParseExact("20111998", "ddMMyyyy", CultureInfo.InvariantCulture),
                23
            };

            this.SetupMockDataReaderProperties<User>(expectedColumnData);

            // When
            User actualMappedUser = this.MySqlUserDataMapper.MapDataToEntity(this.MockDataReader.Object);

            // Then
            Assert.NotNull(actualMappedUser);
            Assert.Equal(id, actualMappedUser.Id);
            Assert.Equal(expectedColumnData[1], actualMappedUser.Firstname);
            Assert.Equal(expectedColumnData[2], actualMappedUser.Lastname);
            Assert.Equal(expectedColumnData[3], actualMappedUser.Email);
            Assert.Equal(expectedColumnData[4], actualMappedUser.DateOfBirth);
            Assert.Equal(expectedColumnData[5], actualMappedUser.Age);
            this.MockDataReader.Verify(s => s.GetName(It.IsAny<int>()), Times.Exactly(6));
            this.MockDataReader.Verify(s => s.GetValue(It.IsAny<int>()), Times.Exactly(6));
        }

        [Fact]
        public void MysqlDataMapperTakesDataReaderAndThrowsOnUnregisteredTypeLongPoco()
        {
            //Given
            Guid id = Guid.NewGuid();

            var expectedColumnData = new object[]
            {
                long.MaxValue
            };

            this.SetupMockDataReaderProperties<UnregisteredLongPOCO>(expectedColumnData);

            // When
            // Then
            Exceptions<Exception>.Handle(() => this.MySqlUnregisteredLongPocoDataMapper.MapDataToEntity(this.MockDataReader.Object),
                (ex) => Assert.Equal("The given key 'Int64' was not present in the dictionary.", ex.Message));

            this.MockDataReader.Verify(s => s.GetName(It.IsAny<int>()), Times.Exactly(1));
            this.MockDataReader.Verify(s => s.GetValue(It.IsAny<int>()), Times.Exactly(1));
        }

        [Fact]
        public void MysqlDataMapperTakesDataReaderAndMapsDataToAnyPocoEntity()
        {
            //Given
            Guid guidOne = Guid.NewGuid();
            Guid guidTwo = Guid.NewGuid();
            Guid guidThree = Guid.NewGuid();

            var expectedColumnData = new object[]
            {
                "string_data",
                new Random().Next(int.MinValue, int.MaxValue),
                DateTime.ParseExact("20111998", "ddMMyyyy", CultureInfo.InvariantCulture),
                guidOne.ToByteArray(),
                guidTwo.ToByteArray(),
                guidThree.ToByteArray()
            };

            this.SetupMockDataReaderProperties<AnyPOCO>(expectedColumnData);

            // When
            AnyPOCO actualMappedUser = this.MySqlAnyPocoDataMapper.MapDataToEntity(this.MockDataReader.Object);

            // Then
            Assert.NotNull(actualMappedUser);
            Assert.Equal(expectedColumnData[0], actualMappedUser.MyPropertyString);
            Assert.Equal(expectedColumnData[1], actualMappedUser.MyPropertyInt);
            Assert.Equal(expectedColumnData[2], actualMappedUser.MyPropertyDateTime);
            Assert.Equal(guidOne, actualMappedUser.MyPropertyGuid);
            Assert.Equal(guidTwo, actualMappedUser.MyPropertyGuid2);
            Assert.Equal(guidThree, actualMappedUser.MyPropertyGuid3);

            this.MockDataReader.Verify(s => s.GetName(It.IsAny<int>()), Times.Exactly(6));
            this.MockDataReader.Verify(s => s.GetValue(It.IsAny<int>()), Times.Exactly(6));
        }

        private void SetupMockDataReaderProperties<TEntity>(object[] expectedColumnData, int ordinalValue = 0)
        {
            PropertyInfo[] propertyInfos = typeof(TEntity).GetProperties();
            this.MockDataReader.SetupGet(s => s.FieldCount).Returns(propertyInfos.Length);

            for (; ordinalValue < this.MockDataReader.Object.FieldCount; ordinalValue++)
            {
                object rowValue = expectedColumnData[ordinalValue];
                this.MockDataReader.Setup(s => s.GetValue(ordinalValue)).Returns(rowValue);
                this.MockDataReader.Setup(s => s.GetName(ordinalValue)).Returns(propertyInfos[ordinalValue].Name);
            }
        }
    }
}