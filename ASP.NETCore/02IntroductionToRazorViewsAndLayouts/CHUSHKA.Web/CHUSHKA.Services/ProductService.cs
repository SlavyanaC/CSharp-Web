namespace CHUSHKA.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class ProductService : IProductService
    {
        private readonly ChushkaDbContext dbContext;
        private readonly IMapper mapper;

        public ProductService(ChushkaDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public string Create(string name, decimal price, string description, string type)
        {
            if (!Enum.TryParse(type, out ProductType productType))
            {
                return null;
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                Description = description,
                Type = productType,
            };

            this.dbContext.Products.Add(product);
            this.dbContext.SaveChanges();

            return product.Id;
        }

        public T Details<T>(string id)
        {
            var product = this.dbContext.Products.Find(id);
            if (product == null)
            {
                return default(T);
            }

            var model = mapper.Map<T>(product);
            return model;
        }

        public ICollection<T> GetAll<T>()
        {
            var products = this.dbContext.Products
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return products;
        }

        public bool Order(string id, string username)
        {
            var product = this.dbContext.Products.Find(id);
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (product == null || user == null)
            {
                return false;
            }

            var order = new Order
            {
                Client =user,
                Product = product,
                OrederedOn = DateTime.UtcNow,
            };

            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool Edit(string id, string name, decimal price, string description, string type)
        {
            if (!Enum.TryParse(type, out ProductType productType))
            {
                return false;
            }

            var product = this.dbContext.Products.Find(id);
            if (product == null)
            {
                return false;
            }

            product.Name = name;
            product.Price = price;
            product.Description = description;
            product.Type = productType;

            this.dbContext.Products.Update(product);
            this.dbContext.SaveChanges();

            return true;
        }

        public void Delete(string id)
        {
            var product = this.dbContext.Products.Find(id);
            if (product == null)
            {
                return;
            }

            this.dbContext.Products.Remove(product);
            this.dbContext.SaveChanges();
        }

    }
}
