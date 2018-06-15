//--------------------------------------------------
// <copyright file="DatabaseSQLiteUnitTestsWithWrapper.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit tests for SQLITE provider</summary>
//--------------------------------------------------

using System;
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using DatabaseUnitTests.Models;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
    public class DatabaseSQLiteUnitTestsWithWrapper : BaseDatabaseTest
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyOrdersHasCorrectNumberOfRecordsSqlite()
        {
            var orders = this.DatabaseWrapper.Query("select * from orders").ToList();
            
            Assert.AreEqual(11, orders.Count);
        }

        /// <summary>
        /// Check if we get the expected number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyOrdersMapIsCorrectSqlite()
        {
            var orders = this.DatabaseWrapper.Query<Orders>("select * from orders").ToList();

            // Our database has 11 orders
            Assert.AreEqual(11, orders.Count());
        }

        /// <summary>
        /// Check if we get the expected number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyInsertOrdersIsCorrectSqlite()
        {
            this.DatabaseWrapper.Connection.Close();

            // Get original sqlite file
            var originalPath = this.GetDByPath();
            var originalDatabase = File.ReadAllBytes(originalPath);

            // Insert new order record
            var newOrder = CreateNewOrder();

            try
            {
                this.DatabaseWrapper.Connection.Open();
                var count = this.DatabaseWrapper.Insert(newOrder);

                // Our database has 12 orders
                Assert.AreEqual(12, count);
            }
            catch (Exception)
            {
                // Rewrite the SQLite file
                if (this.DatabaseWrapper.Connection.State.Equals(ConnectionState.Open))
                {
                    this.DatabaseWrapper.Connection.Close();
                }

                File.WriteAllBytes(originalPath, originalDatabase);

                throw;
            }
            finally
            {
                // Rewrite the SQLite file again
                if (this.DatabaseWrapper.Connection.State.Equals(ConnectionState.Open))
                {
                    this.DatabaseWrapper.Connection.Close();
                }

                File.WriteAllBytes(originalPath, originalDatabase);
            }
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyDeleteOrdersIsCorrectSqlite()
        {
            this.DatabaseWrapper.Connection.Close();

            // Get original sqlite file
            var originalPath = this.GetDByPath();
            var originalDatabase = File.ReadAllBytes(originalPath);

            // Insert new order record
            var newOrder = CreateNewOrder();

            try
            {
                this.DatabaseWrapper.Connection.Open();
                var count = this.DatabaseWrapper.Insert(newOrder);

                // Our database has 12 orders
                Assert.AreEqual(12, count);

                var isDeleted = this.DatabaseWrapper.Delete(newOrder);
                var orders = this.DatabaseWrapper.Query<Orders>("select * from orders").ToList();
                
                // Our database has 11 orders
                Assert.IsTrue(isDeleted);
                Assert.AreEqual(11, orders.Count);
            }
            catch (Exception)
            {
                // Rewrite the SQLite file again
                if (this.DatabaseWrapper.Connection.State.Equals(ConnectionState.Open))
                {
                    this.DatabaseWrapper.Connection.Close();
                }

                File.WriteAllBytes(originalPath, originalDatabase);

                throw;
            }
            finally
            {
                // Rewrite the SQLite file again
                if (this.DatabaseWrapper.Connection.State.Equals(ConnectionState.Open))
                {
                    this.DatabaseWrapper.Connection.Close();
                }

                File.WriteAllBytes(originalPath, originalDatabase);
            }
        }

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyUpdateOrdersIsCorrectSqlite()
        {
            this.DatabaseWrapper.Connection.Close();

            // Get original sqlite file
            var originalPath = this.GetDByPath();
            var originalDatabase = File.ReadAllBytes(originalPath);
            var newOrder = CreateNewOrder();

            try
            {
                this.DatabaseWrapper.Connection.Open();
                var count = this.DatabaseWrapper.Insert(newOrder);

                // Our database has 12 orders
                Assert.AreEqual(12, count);

                newOrder.OrderName = "updated";

                var deletedCount = this.DatabaseWrapper.Update(newOrder);
                var order = this.DatabaseWrapper.Query<Orders>($"select * from orders where Id = {newOrder.Id}").FirstOrDefault();

                Assert.IsTrue(deletedCount);
                Assert.AreEqual(newOrder.Id, order.Id);
            }
            catch (Exception)
            {
                // Rewrite the SQLite file again
                if (this.DatabaseWrapper.Connection.State.Equals(ConnectionState.Open))
                {
                    this.DatabaseWrapper.Connection.Close();
                }

                File.WriteAllBytes(originalPath, originalDatabase);

                throw;
            }
            finally
            {
                // Rewrite the SQLite file again
                if (this.DatabaseWrapper.Connection.State.Equals(ConnectionState.Open))
                {
                    this.DatabaseWrapper.Connection.Close();
                }

                File.WriteAllBytes(originalPath, originalDatabase);
            }
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        protected override IDbConnection GetDataBaseConnection()
        {
            return DatabaseConfig.GetOpenConnection("SQLITE", $"Data Source={GetDByPath()}");
        }

        /// <summary>
        /// Used to create a new order with random id's
        /// </summary>
        /// <returns> The <see cref="Orders"/> with generated id's </returns>
        private static Orders CreateNewOrder()
        {
            // Insert new order record
            return new Orders()
            {
                OrderId = new Random().Next(100, 10000),
                OrderName = $"Order_{ Guid.NewGuid()}",
                ProductId = 1,
                UserId = 1
            };
        }

        /// <summary>
        /// Gets the path of the SQLITE file
        /// </summary>
        /// <returns>string path of the file</returns>
        private string GetDByPath()
        {
            UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().Location);
            return $"{Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path))}\\MyDatabase.sqlite";
        }
    }
}
