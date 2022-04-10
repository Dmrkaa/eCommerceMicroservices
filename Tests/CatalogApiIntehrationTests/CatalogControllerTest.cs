using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using CatalogAPI.Data;
using CatalogAPI.Data.Interfaces;
using CatalogAPI.Entities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;
using MongoDB.Driver;
using System.Threading;
using System.Net;
using System;
using System.IO;
namespace CatalogApiIntehrationTests
{
    [TestFixture]
    public class CatalogControllerTest
    {
        //private MongoDBSettings settings;
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;
        private Mock<IMongoCollection<Product>> productCollection;
        private List<Product> productList;
        private Mock<IAsyncCursor<Product>> productCursor;

        Mock<IAsyncCursor<Product>> _productCursor = new Mock<IAsyncCursor<Product>>();


        public CatalogControllerTest()
        {
            // this.settings = new MongoDBSettings("ecommerce-db");
            this.mongoClient = new Mock<IMongoClient>();
            this.productCollection = new Mock<IMongoCollection<Product>>();
            this.mongodb = new Mock<IMongoDatabase>();
            this.productCursor = new Mock<IAsyncCursor<Product>>();
            productList = CatalogContextSeed.PreConf().ToList();

            _productCursor.Setup(_ => _.Current).Returns(productList);
            _productCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                    .Returns(true)
                    .Returns(false);
            _productCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(true))
                    .Returns(Task.FromResult(false));
        }

        private void InitializeMongoDb()
        {
            this.mongodb.Setup(x => x.GetCollection<Product>("Products",
                default)).Returns(this.productCollection.Object);
            this.mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(),
                default)).Returns(this.mongodb.Object);
        }
        private void InitializeMongoProductCollection()
        {
            this.productCursor.Setup(_ => _.Current).Returns(this.productList);
            this.productCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            this.productCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true)).Returns(Task.FromResult(false));
            this.productCollection.Setup(x => x.AggregateAsync(It.IsAny<PipelineDefinition<Product, Product>>(), It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(this.productCursor.Object);
            this.InitializeMongoDb();
        }
        [Test]
        public async Task GetProducts_SendReq_ShouldReturnOK()
        {
            this.InitializeMongoProductCollection();

            var mongoDB = mongoClient.Object.GetDatabase("databaseName");
            var mongoColl = mongoDB.GetCollection<Product>("Products");

            productCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                                                       It.IsAny<FindOptions<Product, Product>>(),
                                                       It.IsAny<CancellationToken>())).ReturnsAsync(_productCursor.Object);

            //Arragne
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICatalogContext));
                    services.Remove(dbContextDescriptor);

                    var moqService = new Mock<ICatalogContext>();
                    moqService.Setup(m => m.Products).Returns(() => mongoColl);
                    services.AddScoped<ICatalogContext>(_ => moqService.Object);
                });
            });

            HttpClient httpClient = webHost.CreateClient();

            //Act
            HttpResponseMessage responseMessage = await httpClient.GetAsync("api/Catalog");


            //Assert
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Test]
        public async Task GetProductsByName_SendReq_ShouldReturnOK()
        {
            this.InitializeMongoProductCollection();

            var mongoDB = mongoClient.Object.GetDatabase("databaseName");
            var mongoColl = mongoDB.GetCollection<Product>("Products");
            string productName = "Jacket";
            _productCursor.Setup(_ => _.Current).Returns(productList.FindAll(x => x.Name == productName));
            productCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                                                       It.IsAny<FindOptions<Product, Product>>(),
                                                       It.IsAny<CancellationToken>())).ReturnsAsync(_productCursor.Object);

            //Arragne
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICatalogContext));
                    services.Remove(dbContextDescriptor);

                    var moqService = new Mock<ICatalogContext>();
                    moqService.Setup(m => m.Products).Returns(() => mongoColl);
                    services.AddScoped<ICatalogContext>(_ => moqService.Object);
                });
            });

            HttpClient httpClient = webHost.CreateClient();

            //Act
            HttpResponseMessage responseMessage = await httpClient.GetAsync($"api/Catalog/ByName/{productName}");


            //Assert
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Test]
        public async Task GetProductsByName_SendReq_ShouldReturnNotFound()
        {
            this.InitializeMongoProductCollection();

            var mongoDB = mongoClient.Object.GetDatabase("databaseName");
            var mongoColl = mongoDB.GetCollection<Product>("Products");
            string productName = "Jacket";
            _productCursor.Setup(_ => _.Current).Returns(productList.FindAll(x => x.Name == productName));
            productCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                                                       It.IsAny<FindOptions<Product, Product>>(),
                                                       It.IsAny<CancellationToken>())).ReturnsAsync(_productCursor.Object);

            //Arragne
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICatalogContext));
                    services.Remove(dbContextDescriptor);

                    var moqService = new Mock<ICatalogContext>();
                    moqService.Setup(m => m.Products).Returns(() => mongoColl);
                    services.AddScoped<ICatalogContext>(_ => moqService.Object);
                });
            });

            HttpClient httpClient = webHost.CreateClient();

            //Act
            HttpResponseMessage responseMessage = await httpClient.GetAsync($"api/Catalog/ByName/{productName}");

            /*  try
              {
                  List<Product> custs = await responseMessage.Content.ReadAsAsync<List<Product>>();
              }
              catch(Exception e)
              {

              }*/



            //Assert
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}
