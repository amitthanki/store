using CartMicroservice.Controllers;
using CartMicroservice.Model;
using CartMicroservice.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CartMicroservice.UnitTests
{
    public class CartControllerTest
    {
        private readonly CartController _controller;
        private readonly List<Cart> _carts = new List<Cart>
        {
            new Cart
            {
                Id = new Guid("6bd19484-2237-4319-a690-510e4f2258d8"),
                UserId = new Guid("f5dd5ea6-ac9c-4dc3-86c3-090853945167"),
                CartItems = new List<CartItem>
                {
                    new CartItem{ CatalogItemId= new Guid("ce2dbb82-6689-487b-9691-0a05ebabce4a"), CatalogItemName = "Samsung Galaxy S10", CatalogItemPrice = 1000, Quantity = 1}
                }
            }
        };

        public CartControllerTest()
        {
            var mockRepo = new Mock<ICartRepository>();
            mockRepo.Setup(repo => repo.GetCartItems(It.IsAny<Guid>())).Returns<Guid>(userId =>
            {
                var cart = _carts.FirstOrDefault(c => c.UserId == userId);
                if (cart != null)
                {
                    return cart.CartItems;
                }
                return new List<CartItem>();
            });
            mockRepo.Setup(repo => repo.InsertCartItem(It.IsAny<Guid>(), It.IsAny<CartItem>())).Callback<Guid, CartItem>((userId, cartItem) =>
            {
                var cart = _carts.FirstOrDefault(c => c.UserId == userId);
                if (cart != null)
                {
                    var ci = cart.CartItems.FirstOrDefault(i => i.CatalogItemId == cartItem.CatalogItemId);
                    if (ci == null)
                    {
                        cart.CartItems.Add(cartItem);
                    }
                    else
                    {
                        ci.Quantity++;
                    }
                }
            });
            mockRepo.Setup(repo => repo.UpdateCartItem(It.IsAny<Guid>(), It.IsAny<CartItem>())).Callback<Guid, CartItem>((userId, cartItem) =>
            {
                var cart = _carts.FirstOrDefault(c => c.UserId == userId);
                if (cart != null)
                {
                    var ci = cart.CartItems.FirstOrDefault(i => i.CatalogItemId == cartItem.CatalogItemId);
                    if (ci != null)
                    {
                        ci.CatalogItemName = cartItem.CatalogItemName;
                        ci.CatalogItemPrice = cartItem.CatalogItemPrice;
                        ci.Quantity = cartItem.Quantity;
                    }
                }
            });
            mockRepo.Setup(repo => repo.DeleteCartItem(It.IsAny<Guid>(), It.IsAny<Guid>())).Callback<Guid, Guid>((userId, cartItemId) =>
            {
                var cart = _carts.FirstOrDefault(c => c.UserId == userId);
                if (cart != null)
                {
                    cart.CartItems.RemoveAll(i => i.CatalogItemId == cartItemId);
                }
            });
            _controller = new CartController(mockRepo.Object);

        }

        [Fact]
        public void GetCartItemsTest()
        {

        }

        [Fact]
        public void InsertCartItemTest()
        {

        }

        [Fact]
        public void UpdateCartItemTest()
        {

        }

        [Fact]
        public void DeleteCartItemTest()
        {

        }
    }
}
