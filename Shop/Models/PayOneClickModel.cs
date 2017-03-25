using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class PayOneClickModel
    {
        public string Classes { get; set; }
        public long ProductId { get; set; }
        public string Phone { get; set; }

        public PayOneClickModel(string classes, long productId, string phone)
        {
            Classes = classes;
            ProductId = productId;
            Phone = phone;
        }

        public PayOneClickModel()
        {
            
        }

    }
}